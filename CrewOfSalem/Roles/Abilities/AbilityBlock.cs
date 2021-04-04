using System.Collections.Generic;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityBlock : AbilityDuration
    {
        // Fields
        private PlayerControl blockedPlayer;

        // Properties
        public PlayerControl BlockedPlayer => blockedPlayer;

        // Properties Ability
        protected override Sprite Sprite      => ButtonBlock;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.Block;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId, Button.CurrentTarget.PlayerId};

        protected override RPC               RpcEndAction => RPC.BlockEnd;
        protected override IEnumerable<byte> RpcEndData   => new[] {owner.Owner.PlayerId};

        // Constructors
        public AbilityBlock(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            blockedPlayer = target;
            foreach (Ability ability in target.GetRole().GetAllAbilities())
            {
                ability.AddCooldown(Duration);
            }

            sendRpc = setCooldown = true;
        }

        protected override void EffectEndInternal()
        {
            blockedPlayer = null;
        }
    }
}