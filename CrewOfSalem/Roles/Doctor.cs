using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Doctor : RoleGeneric<Doctor>
    {
        // Properties Role
        public override byte   RoleID => 218;
        public override string Name   => nameof(Doctor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override string Description => "You can shield your target to prevent the next attack within a given time";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Doctor, AbilityShield>();
        }
    }
}