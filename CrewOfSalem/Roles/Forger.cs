using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Forger : RoleGeneric<Forger>
    {
        // Properties Role
        protected override byte   RoleID => 227;
        public override    string Name   => nameof(Forger);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can first steal someones appearance, to then make yourself look like them";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityKill(this, 30F));
            AddAbility(new AbilityForge(this, 40F, 10F));
        }
    }
}