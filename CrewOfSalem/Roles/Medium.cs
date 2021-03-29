using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Medium : RoleGeneric<Medium>
    {
        // Properties Role
        protected override byte   RoleID => 223;
        public override    string Name   => nameof(Medium);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can interact with the dead and can once interact with a living playing while being dead yourself";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}