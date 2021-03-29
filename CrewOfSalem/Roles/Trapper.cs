using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Trapper : RoleGeneric<Trapper>
    {
        // Properties Role
        protected override byte   RoleID => 220;
        public override    string Name   => nameof(Trapper);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override string Description => "You can lay out traps for the Mafia";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}