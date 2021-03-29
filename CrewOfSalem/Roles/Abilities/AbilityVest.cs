using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityVest : AbilityDuration
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonVest;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.VestStart;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId};

        protected override RPC               RpcEndAction => RPC.VestEnd;
        protected override IEnumerable<byte> RpcEndData   => new[] {owner.Owner.PlayerId};

        // Constructors
        public AbilityVest(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = true;
        }

        // Methods Ability Duration
        protected override void EffectEndInternal() { }
    }
}