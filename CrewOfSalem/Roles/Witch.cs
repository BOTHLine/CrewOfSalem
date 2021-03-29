using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Witch : RoleGeneric<Witch>
    {
        // Properties Role
        protected override byte   RoleID => 246;
        public override    string Name   => nameof(Witch);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        public override string Description => "You can copy the ability of another player and use it. You will also learn their role. You win with everyone but the crew";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}