using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Veteran : RoleGeneric<Veteran>
    {
        // Properties
        public bool IsAlerted => CurrentDuration > 0F;

        // Properties Role
        protected override byte   RoleID => 215;
        public override    string Name   => nameof(Veteran);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => VeteranButton;

        // Methods
        public void KillPlayer(PlayerControl target)
        {
            MessageWriter writer = GetWriter(RPC.VeteranKill);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            PlayerControl.LocalPlayer.MurderPlayer(target);
        }

        // Methods Role
        public override bool PerformAction(PlayerControl target)
        {
            CurrentDuration = Duration;

            WriteImmediately(RPC.VeteranAlert);
            return true;
        }

        public override void UpdateDuration(float deltaTime)
        {
            base.UpdateDuration(deltaTime);
            if (CurrentDuration <= 0F) WriteImmediately(RPC.VeteranAlertEnd);
        }
    }
}