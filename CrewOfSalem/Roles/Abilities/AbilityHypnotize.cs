using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityHypnotize : AbilityDuration
    {
        // Properties
        public          PlayerControl          HypnotizeTarget  { get; set; }
        public          PlayerControl          HypnotizedPlayer { get; set; }
        public readonly Dictionary<byte, byte> playerMappings = new Dictionary<byte, byte>();

        // Properties Ability
        protected override Sprite Sprite      => ButtonHypnotize;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.HypnotizeStart;
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        protected override RPC               RpcEndAction => RPC.HypnotizeEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityHypnotize(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods
        private void PrepareHypnotize(PlayerControl target)
        {
            HypnotizeTarget = target;
        }

        public void Hypnotize()
        {
            CurrentDuration = Duration;
            isEffectActive = true;
            HypnotizedPlayer = HypnotizeTarget;
            HypnotizeTarget = null;
            playerMappings.Clear();
            if (HypnotizedPlayer != LocalPlayer) return;

            List<byte> originalIds = AllPlayers.Where(player => !player.Data.IsDead).Select(player => player.PlayerId)
               .ToList();
            List<byte> visualIds = originalIds.ToList();

            originalIds.Remove(HypnotizedPlayer.PlayerId);
            visualIds.Remove(HypnotizedPlayer.PlayerId);
            for (int i = originalIds.Count - 1; i >= 0; i--)
            {
                for (int j = visualIds.Count - 1; j >= 0; j--)
                {
                    int originalIndex = Rng.Next(originalIds.Count);
                    int visualIndex = Rng.Next(visualIds.Count);
                    playerMappings.Add(originalIds[originalIndex], visualIds[visualIndex]);
                    originalIds.RemoveAt(originalIndex);
                    visualIds.RemoveAt(visualIndex);
                }
            }
        }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && HypnotizeTarget == null;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            PrepareHypnotize(target);
            isEffectActive = false;
            CurrentDuration = 0F;
            sendRpc = true;
            setCooldown = false;
        }

        protected override void EffectEndInternal()
        {
            HypnotizedPlayer = null;
            playerMappings.Clear();
        }

        protected override void UpdateButtonSprite()
        {
            if (HypnotizedPlayer != null)
            {
                Button.renderer.color = HypnotizedPlayer.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            } else if (HypnotizeTarget != null)
            {
                Button.renderer.color = HypnotizeTarget.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            } else
            {
                base.UpdateButtonSprite();
            }
        }

        protected override void ExileEndInternal()
        {
            Hypnotize();
        }
    }
}