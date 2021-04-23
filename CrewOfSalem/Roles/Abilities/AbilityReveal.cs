using System.Collections.Generic;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityReveal : Ability
    {
        // Properties
        public bool hasRevealed { get; private set; }

        // Properties Ability
        protected override Sprite Sprite      => ButtonReveal;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.Reveal;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        // Constructors
        public AbilityReveal(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && !hasRevealed;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            hasRevealed = true;
            sendRpc = true;
            setCooldown = false;
        }

        protected override void UpdateButtonSprite()
        {
            if (!hasRevealed)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = owner.Owner.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }
    }
}