using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Veteran : RoleGeneric<Veteran>
    {
        // Properties Role
        protected override byte   RoleID => 215;
        public override    string Name   => nameof(Veteran);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can go on alert to kill the next person who tries to use any ability on you, either good or evil";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityAlert(this, 30F, 10F));
        }
    }
}