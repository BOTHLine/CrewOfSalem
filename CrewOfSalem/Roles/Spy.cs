using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Spy : RoleGeneric<Spy>
    {
        // Properties Role
        protected override byte   RoleID => 211;
        public override    string Name   => nameof(Spy);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        protected override bool   HasSpecialButton    => false;
        protected override Sprite SpecialButtonSprite => null;

        // Methods Role
        public override bool PerformAction(PlayerControl target)
        {
            return true;
            // Extra Info from Admin / Vitals?
        }
    }
}