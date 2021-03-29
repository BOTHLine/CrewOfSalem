using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Spy : RoleGeneric<Spy>
    {
        // Properties Role
        protected override byte   RoleID => 211;
        public override    string Name   => nameof(Spy);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description => "You can vent and listen to the mafia chat";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}