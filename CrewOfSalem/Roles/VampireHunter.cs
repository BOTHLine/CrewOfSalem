using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class VampireHunter : RoleGeneric<VampireHunter>
    {
        // Properties Role
        protected override byte   RoleID => 214;
        public override    string Name   => "Vampire Hunter";

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "If a vampire tries to bite you or you check a target and they are a vampire, they will die";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}