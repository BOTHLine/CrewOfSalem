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
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityKill(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            owner.Owner.RpcKillPlayer(target, owner.Owner);
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