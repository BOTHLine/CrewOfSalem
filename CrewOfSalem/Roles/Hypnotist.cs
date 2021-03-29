using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Hypnotist : RoleGeneric<Hypnotist>
    {
        // Methods Role
        protected override byte   RoleID => 229;
        public override    string Name   => nameof(Hypnotist);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Deception;

        public override string Description => "You can hypnotize your target to give them false information";

        // Properties Role
        protected override void InitializeAbilities() { }
    }
}