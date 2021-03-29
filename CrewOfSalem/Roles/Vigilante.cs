using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Vigilante : RoleGeneric<Vigilante>
    {
        // Properties Role
        protected override byte   RoleID => 216;
        public override    string Name   => nameof(Vigilante);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can shoot a person. But if they are good, you will kill yourself instead";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility(new AbilityKill(this, 30F));
        }
    }
}