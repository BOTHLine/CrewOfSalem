using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Consort : RoleGeneric<Consort>
    {
        // Properties Role
        protected override byte   RoleID => 236;
        public override    string Name   => nameof(Consort);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can block your target to increase their ability cooldown and prevent them from doing tasks";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityBlock(this, 30F, 15F));
        }
    }
}