using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Consigliere : RoleGeneric<Consigliere>
    {
        // Properties Role
        protected override byte   RoleID => 235;
        public override    string Name   => nameof(Consigliere);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can investigate your target to know their exact role";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityCheckRole(this, 50F));
        }
    }
}