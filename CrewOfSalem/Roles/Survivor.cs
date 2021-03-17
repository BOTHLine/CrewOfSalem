using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Survivor : RoleGeneric<Survivor>
    {
        // Properties Role
        public override byte RoleID => 239;
        public override string Name => nameof(Survivor);

        public override Faction Faction => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;

        public override Color Color => new Color(200F / 255F, 200F / 255F, 0F / 255F, 1F);

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => SurvivorButton;

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            base.PerformAction(instance);
        }
    }
}