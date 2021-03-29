using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Lookout : RoleGeneric<Lookout>
    {
        // Properties Role
        protected override byte   RoleID => 208;
        public override    string Name   => nameof(Lookout);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description => "You can activate your ability to gain special information from lifeline/admin for a specific time";

        // Method Role
        protected override void InitializeAbilities() { }
    }
}