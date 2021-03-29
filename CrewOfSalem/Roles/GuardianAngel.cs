using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class GuardianAngel : RoleGeneric<GuardianAngel>
    {
        // Properties Role
        protected override byte   RoleID => 238;
        public override    string Name   => "Guardian Angel";

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;

        public override string Description => "You can protect your target to prevent the next attack within a given time. You win if they live until they end";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}