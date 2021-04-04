using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Investigator : RoleGeneric<Investigator>
    {
        // Properties Role
        public override byte   RoleID => 207;
        public override string Name   => nameof(Investigator);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description => "You can investigate your target to learn something about their role";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Investigator, AbilityInvestigate>();
        }
    }
}