using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityKill : Ability
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonKill;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.Kill;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId, Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityKill(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            IReadOnlyList<AbilityGuard> abilityGuards = GetAllAbilities<AbilityGuard>();
            foreach (AbilityGuard abilityGuard in abilityGuards)
            {
                if (abilityGuard.owner.Owner == target) continue;
                if (!abilityGuard.IsGuarding || abilityGuard.IsInTask) continue;
                if (!PlayerTools.IsPlayerInRange(abilityGuard.owner.Owner, target)) continue;

                abilityGuard.owner.Owner.RpcMurderPlayer(owner.Owner);
                abilityGuard.owner.Owner.RpcMurderPlayer(abilityGuard.owner.Owner);
                sendRpc = false;
                setCooldown = true;
                return;
            }

            IReadOnlyList<AbilityVest> abilityVests = GetAllAbilities<AbilityVest>();
            foreach (AbilityVest abilityVest in abilityVests)
            {
                if (abilityVest.owner.Owner != target) continue;

                abilityVest.RpcEffectEnd();
                sendRpc = false;
                setCooldown = true;
                return;
            }

            IReadOnlyList<AbilityShield> abilityShields = GetAllAbilities<AbilityShield>();
            foreach (AbilityShield abilityShield in abilityShields)
            {
                if (abilityShield.ShieldedPlayer != target) continue;

                abilityShield.RpcEffectEnd();
                sendRpc = false;
                setCooldown = true;
                return;
            }

            owner.Owner.MurderPlayer(target);
            sendRpc = setCooldown = true;
        }

        protected override void UpdateTarget()
        {
            if (owner.Faction == Faction.Mafia || owner.Faction == Faction.Coven)
            {
                Button.SetTarget(PlayerTools.FindClosestTarget(owner.Owner,
                    (player) => player.GetRole()?.Faction != owner.Faction));
            } else
            {
                base.UpdateTarget();
            }
        }
    }
}