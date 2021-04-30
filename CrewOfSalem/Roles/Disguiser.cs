using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Disguiser : RoleGeneric<Disguiser>
    {
        // Properties Role
        public override byte   RoleID => 226;
        public override string Name   => nameof(Disguiser);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Deception;

        public override string Description => "You can disguise everyone in your range to make them look the same";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Disguiser, AbilityDisguise>();
        }
    }
}