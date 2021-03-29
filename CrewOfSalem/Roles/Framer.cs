using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Framer : RoleGeneric<Framer>
    {
        // Properties Role
        protected override byte   RoleID => 228;
        public override    string Name   => nameof(Framer);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Deception;

        public override string Description => "You can frame your target to make them look suspicious to others";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}