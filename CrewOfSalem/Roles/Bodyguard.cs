using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Bodyguard : RoleGeneric<Bodyguard>
    {
        // Properties Role
        public override byte   RoleID => 217;
        public override string Name   => nameof(Bodyguard);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override string Description { get; }

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Bodyguard, AbilityGuard>();
        }
    }
}