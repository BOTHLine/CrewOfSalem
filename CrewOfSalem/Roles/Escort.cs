using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Escort : RoleGeneric<Escort>
    {
        // Properties Role
        protected override byte   RoleID => 221;
        public override    string Name   => nameof(Escort);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can block your target to increase their ability cooldown";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityBlock(this, 30F, 15F));
        }
    }
}