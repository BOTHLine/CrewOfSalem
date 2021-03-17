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
        public override byte RoleID => 216;
        public override string Name => nameof(Vigilante);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => VigilanteButton;

        // Methods
        public void KillPlayer(PlayerControl target)
        {
            MessageWriter writer = GetWriter(RPC.VigilanteKill);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            PlayerControl.LocalPlayer.MurderPlayer(target);
        }

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            PlayerControl target = PlayerTools.FindClosestTarget(Player);
            if (target == null) return;

            Player.SetKillTimer(Cooldown);

            if (TryGetSpecialRole(out Doctor doctor) && doctor.ShieldedPlayer?.PlayerId == target.PlayerId)
            {
                doctor.BreakShield();
                return;
            }

            if ((TryGetSpecialRole(out Jester jester) && target.PlayerId == jester.Player.PlayerId && jester.CanDieToVigilante) || target.Data.IsImpostor)
            {
                KillPlayer(target);
            }
            else
            {
                KillPlayer(Player);
            }
        }
    }
}