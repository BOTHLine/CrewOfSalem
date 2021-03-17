using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Escort : RoleGeneric<Escort>
    {
        // Properties Role
        public override byte   RoleID => 221;
        public override string Name   => nameof(Escort);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override bool   HasSpecialButton => true;
        public override Sprite SpecialButton    => EscortButton;

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            PlayerControl target = PlayerTools.FindClosestTarget(Player);
            if (target == null) return;

            Player.SetKillTimer(Cooldown);
            target.SetKillTimer(target.killTimer + Duration);

            MessageWriter writer = GetWriter(RPC.EscortIncreaseCooldown);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
        }
    }
}