using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{

    public class Investigator : RoleGeneric<Investigator>
    {
        // Properties Role
        public override byte RoleID => 207;
        public override string Name => nameof(Investigator);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override Color Color => Color.green;

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => InvestigatorButton;

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {

        }
    }
}