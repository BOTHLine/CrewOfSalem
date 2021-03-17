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
        public float AlertDuration { get; private set; }

        public bool IsOnAlert => CurrentDuration > 0F;

        // Properties Role
        public override byte RoleID => 215;
        public override string Name => nameof(Veteran);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => VeteranButton;

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
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            Player.SetKillTimer(Cooldown);
            CurrentDuration = AlertDuration;

            MessageWriter writer = GetWriter(RPC.VeteranAlert);
            CloseWriter(writer);
        }
    }
}