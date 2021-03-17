﻿using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Spy : RoleGeneric<Spy>
    {
        // Properties Role
        public override byte RoleID => 211;
        public override string Name => nameof(Spy);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override bool HasSpecialButton => false;
        public override Sprite SpecialButton => null;

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            // Extra Info from Admin / Vitals?
        }
    }
}