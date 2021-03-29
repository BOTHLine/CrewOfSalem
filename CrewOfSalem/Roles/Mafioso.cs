using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Mafioso : RoleGeneric<Mafioso>
    {
        // Properties
        protected override byte   RoleID => 233;
        public override    string Name   => nameof(Mafioso);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You have to kill the crew";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityKill(this, 25F));
        }
    }
}