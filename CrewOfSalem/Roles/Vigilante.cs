using System;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Vigilante : RoleGeneric<Vigilante>
    {
        // Properties Role
        public override byte   RoleID => 216;
        public override string Name   => nameof(Vigilante);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description =>
            "You can shoot a person. But if they are good, you will kill yourself instead";

        private static readonly Func<Ability, PlayerControl, bool> UseKillAsVigilante = (source, target) =>
        {
            if (source.owner == Instance && target.GetRole().Faction == Faction.Crew)
            {
                source.owner.Owner.RpcKillPlayer(source.owner.Owner, source.owner.Owner);
                return false;
            }

            return true;
        };

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Vigilante, AbilityKill>();
            Ability.AddOnBeforeUse(UseKillAsVigilante, 100);
        }

        protected override void ClearSettingsInternal()
        {
            Ability.RemoveOnBeforeUse(UseKillAsVigilante);
        }
    }
}