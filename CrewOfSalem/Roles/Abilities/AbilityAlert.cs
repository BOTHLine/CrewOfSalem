using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityAlert : AbilityDuration
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonAlert;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.AlertStart;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        protected override RPC               RpcEndAction => RPC.AlertEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnAlerted;
        protected override int                                OnBeforeUsePriority => 5;

        private static readonly Func<Ability, PlayerControl, bool> UseOnAlerted = (source, target) =>
        {
            if (source is AbilityProtect) return true;

            AbilityAlert abilityAlert = GetAllAbilities<AbilityAlert>()
               .FirstOrDefault(alert => alert.owner.Owner == target && alert.HasDurationLeft);
            if (abilityAlert == null) return true;

            source.owner.Owner.RpcKillPlayer(source.owner.Owner, abilityAlert.owner.Owner);
            return false;
        };

        // Constructors
        public AbilityAlert(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = true;
        }

        // Methods Ability Duration
        protected override void EffectEndInternal() { }
    }
}