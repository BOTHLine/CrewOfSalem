using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Blackmailer : RoleGeneric<Blackmailer>
    {
        // Properties Role
        public override byte   RoleID => 234;
        public override string Name   => nameof(Blackmailer);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can blackmail your target to prevent them from talking and voting in the next meeting";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Blackmailer, AbilityBlackmail>();
        }
    }
}