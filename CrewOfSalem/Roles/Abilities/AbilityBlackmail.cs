using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityBlackmail : Ability
    {
        // Properties
        public PlayerControl BlackmailedPlayer { get; set; }

        // Properties Ability
        protected override Sprite Sprite      => ButtonBlackmail;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.Blackmail;
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityBlackmail(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && BlackmailedPlayer == null;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            BlackmailedPlayer = target;
            sendRpc = true;
            setCooldown = false;
        }

        protected override void UpdateButtonSprite()
        {
            if (BlackmailedPlayer == null)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = Palette.PlayerColors[BlackmailedPlayer.Data.ColorId];
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }
    }
}