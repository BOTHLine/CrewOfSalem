using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityGuard : AbilityDuration
    {
        // Fields
        private bool isGuarding = false;

        // Properties Ability
        protected override Sprite Sprite => ButtonGuard;

        protected override bool NeedsTarget => false;

        protected override RPC               RpcAction => RPC.GuardStart;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        protected override RPC               RpcEndAction => RPC.GuardEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnGuarded;
        protected override int                                OnBeforeUsePriority => 10;

        public AbilityGuard(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        private static readonly Func<Ability, PlayerControl, bool> UseOnGuarded = (source, target) =>
        {
            ConsoleTools.Info("Pre Check OnGuarded");
            if (!(source is AbilityKill)) return true;

            AbilityGuard abilityGuard = GetAllAbilities<AbilityGuard>().FirstOrDefault(guard =>
                guard.HasDurationLeft && target != guard.owner.Owner &&
                PlayerTools.IsPlayerInUseRange(guard.owner.Owner, target, Main.OptionBodyguardGuardRange));
            ConsoleTools.Info("Check OnGuarded");
            if (abilityGuard == null) return true;
            ConsoleTools.Info("Use OnGuarded");

            abilityGuard.owner.Owner.RpcKillPlayer(source.owner.Owner);
            abilityGuard.owner.Owner.RpcKillPlayer(abilityGuard.owner.Owner, source.owner.Owner);
            return false;
        };

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            isGuarding = !isGuarding;
            sendRpc = setCooldown = true;
        }

        protected override void EffectEndInternal() { }
    }
}