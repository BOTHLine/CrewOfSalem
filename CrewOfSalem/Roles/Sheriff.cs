using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Sheriff : RoleGeneric<Sheriff>
    {
        // Properties Role
        public override byte   RoleID => 210;
        public override string Name   => nameof(Sheriff);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override bool   HasSpecialButton => false;
        public override Sprite SpecialButton    => null;

        // Methods Role
        protected override void SetConfigSettings()
        {
            // TODO Report Times
        }
    }
}