using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Amnesiac : RoleGeneric<Amnesiac>
    {
        // Properties Role
        protected override byte   RoleID => 237;
        public override    string Name   => nameof(Amnesiac);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;

        public override string Description { get; }

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}