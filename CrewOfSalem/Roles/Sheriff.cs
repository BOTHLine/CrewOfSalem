using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Sheriff : RoleGeneric<Sheriff>
    {
        // Properties Role
        protected override byte   RoleID => 210;
        public override    string Name   => nameof(Sheriff);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description => "You gain more information for reporting corpses. The faster you report the more information you gain";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}