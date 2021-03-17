using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Survivor : RoleGeneric<Survivor>
    {
        // Properties
        public bool IsVested => CurrentDuration > 0F;

        // Properties Role
        protected override byte   RoleID => 239;
        public override    string Name   => nameof(Survivor);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;

        protected override Color Color => new Color(200F / 255F, 200F / 255F, 0F / 255F, 1F);

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => SurvivorButton;

        // Methods Role
        public override bool PerformAction(PlayerControl target)
        {
            WriteImmediately(RPC.SurvivorVest);
            return true;
        }

        public override void UpdateDuration(float deltaTime)
        {
            base.UpdateDuration(deltaTime);
            if (CurrentDuration <= 0F) WriteImmediately(RPC.SurvivorVestEnd);
        }
    }
}