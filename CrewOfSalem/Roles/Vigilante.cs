using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Vigilante : RoleGeneric<Vigilante>
    {
        // Properties Role
        protected override byte   RoleID => 216;
        public override    string Name   => nameof(Vigilante);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => VigilanteButton;

        // Methods
        public void KillPlayer(PlayerControl target)
        {
            MessageWriter writer = GetWriter(RPC.VigilanteKill);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            PlayerControl.LocalPlayer.MurderPlayer(target);
        }

        private bool IsKillable(PlayerControl target)
        {
            TryGetSpecialRoleByPlayer(target.PlayerId, out Role role);
            return target.Data.IsImpostor || role is Jester {CanDieToVigilante: true};
        }

        // Methods Role
        public override bool PerformAction(PlayerControl target)
        {
            if (target == null) return false;

            TryGetSpecialRoleByPlayer(target.PlayerId, out Role targetRole);
            switch (targetRole)
            {
                case Survivor {IsVested: true}:
                    return true;
                case Doctor doctor when doctor.ShieldedPlayer?.PlayerId == target.PlayerId:
                    doctor.BreakShield();
                    return true;
                default:
                    KillPlayer(IsKillable(target) ? target : Player);
                    break;
            }

            return true;
        }
    }
}