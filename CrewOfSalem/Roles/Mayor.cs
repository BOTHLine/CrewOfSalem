using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Mayor : RoleGeneric<Mayor>
    {
        // Properties Role
        public override byte   RoleID => 222;
        public override string Name   => nameof(Mayor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "After you publicly reveal yourself as the mayor, your vote in the meeting counts twice";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Mayor, AbilityReveal>();
        }
    }
}