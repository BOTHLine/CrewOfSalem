using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Mayor : RoleGeneric<Mayor>
    {
        // Properties Role
        protected override byte   RoleID => 222;
        public override    string Name   => nameof(Mayor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "After you reveal yourself as the mayor you have extra votes in the meeting";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}