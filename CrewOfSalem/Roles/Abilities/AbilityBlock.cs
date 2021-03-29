using System.Collections.Generic;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityBlock : Ability
    {
        // Fields
        private readonly float blockDuration;

        // Properties Ability
        protected override Sprite Sprite      => ButtonBlock;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.Block;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId, Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityBlock(Role owner, float cooldown, float duration) : base(owner, cooldown)
        {
            blockDuration = duration;
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            foreach (Ability ability in target.GetRole().GetAllAbilities())
            {
                ability.AddCooldown(blockDuration);
            }

            sendRpc = setCooldown = true;
        }
    }
}