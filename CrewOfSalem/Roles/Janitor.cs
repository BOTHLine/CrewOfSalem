using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Janitor : RoleGeneric<Janitor>
    {
        // Properties Role
        protected override byte   RoleID => 230;
        public override    string Name   => nameof(Janitor);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Deception;

        public override string Description => "You can make corpses disappear";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}