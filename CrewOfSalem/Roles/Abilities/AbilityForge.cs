using System.Collections.Generic;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityForge : AbilityDuration
    {
        // Fields
        public PlayerControl currentSample;

        // Properties Ability
        protected override Sprite Sprite => currentSample == null ? ButtonSteal : ButtonForge;

        protected override bool NeedsTarget => currentSample == null;

        protected override RPC               RpcAction => RPC.ForgeStart;
        protected override IEnumerable<byte> RpcData   => new[] {currentSample.PlayerId};

        protected override RPC               RpcEndAction => RPC.ForgeEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityForge(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods
        public void ForgeStart(PlayerControl target)
        {
            CurrentDuration = Duration;
            currentSample = target;
            Forge();
        }

        public void Forge()
        {
            owner.Owner.SetVisuals(currentSample);
        }

        private void ForgeEnd()
        {
            currentSample = null;
            owner.Owner?.SetVisuals(owner.Owner);
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = false;
            if (NeedsTarget && target == null) return;
            if (currentSample == null)
            {
                currentSample = target;
                CurrentCooldown = 1F;
                isEffectActive = false;
            } else
            {
                ForgeStart(currentSample);
                sendRpc = setCooldown = true;
            }
        }

        // Methods AbilityDuration
        protected override void EffectEndInternal()
        {
            ForgeEnd();
        }

        protected override void UpdateButtonSprite()
        {
            if (currentSample != null)
            {
                Button.renderer.sprite = Sprite;
                Button.renderer.color = currentSample.GetPlayerColor();
            } else
            {
                base.UpdateButtonSprite();
            }
        }
    }
}