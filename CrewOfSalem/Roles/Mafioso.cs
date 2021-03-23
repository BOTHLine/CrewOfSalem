using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Mafioso : RoleGeneric<Mafioso>
    {
        // Properties
        protected override byte   RoleID => 233;
        public override    string Name   => nameof(Mafioso);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => MafiosoButton;

        // Methods
        public void KillPlayer(PlayerControl target)
        {
            MessageWriter writer = GetWriter(RPC.MafiosoKill);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            Player.MurderPlayer(target);
        }

        // Methods Role
        // TODO: Move "KillPlayer" And DoctorShield-Checks etc. to global method?
        protected override bool PerformActionInternal()
        {
            ConsoleTools.Info("Perform Action");
            TryGetSpecialRoleByPlayer(SpecialButton.Target.PlayerId, out Role targetRole);
            switch (targetRole)
            {
                case Survivor {HasDurationLeft: true}:
                    return true;
                case Doctor doctor when doctor.ShieldedPlayer?.PlayerId == SpecialButton.Target.PlayerId:
                    doctor.BreakShield();
                    return true;
                default:
                    KillPlayer(SpecialButton.Target);
                    return true;
            }
        }
    }
}