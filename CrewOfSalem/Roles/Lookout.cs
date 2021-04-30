using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Lookout : RoleGeneric<Lookout>
    {
        // Properties Role
        public override byte   RoleID => 208;
        public override string Name   => nameof(Lookout);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description => "Access important information from anywhere on the map!";

        // Method Role
        protected override void InitializeAbilities()
        {
            // AddAbility<Lookout, AbilityMap>();
            // AddAbility<Lookout, AbilitySurveillance>();
            // AddAbility<Lookout, AbilityVitals>();
            AddAbility<Lookout, AbilityWatch>();
        }
    }
}