using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Medium : RoleGeneric<Medium>
    {
        // Properties Role
        public override byte   RoleID => 223;
        public override string Name   => nameof(Medium);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description =>
            "You can see the dead, but therefore you can't see colors";

        // Methods
        protected override void ClearSettingsInternal()
        {
            if (Owner == LocalPlayer) ResetPlayerColors();
        }

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Medium, AbilitySeance>();
        }
    }
}