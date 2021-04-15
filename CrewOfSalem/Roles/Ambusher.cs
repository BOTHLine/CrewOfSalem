using System;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Ambusher : RoleGeneric<Ambusher>
    {
        // Properties Role
        public override byte   RoleID => 231;
        public override string Name   => nameof(Ambusher);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can kill your target without moving. Even out of Vents";

        private static readonly Func<Ability, PlayerControl, bool> OnKill = (source, target) =>
        {
            if (!(source.owner is Ambusher)) return true;
            target.RpcKillPlayer(target, source.owner.Owner);
            source.SetOnCooldown();
            return false;
        };

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Ambusher, AbilityKill>();
            Ability.AddOnBeforeUse(OnKill, 100);
        }
    }
}