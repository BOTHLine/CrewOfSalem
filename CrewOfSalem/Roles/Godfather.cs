using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Godfather : RoleGeneric<Godfather>
    {
        // Properties Role
        protected override byte   RoleID => 232;
        public override    string Name   => nameof(Godfather);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You have to kill the crew";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}