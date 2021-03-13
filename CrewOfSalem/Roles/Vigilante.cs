using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Vigilante : RoleGeneric<Vigilante>
    {
        // Properties
        public bool showReport { get; private set; }

        // Properties Role
        public override Color Color => Color.magenta;

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => VigilanteButton;

        // TODO: Move to Faction/Alignment ?
        protected override string StartText => "Kill the [FF0000FF]Mafia[]";

        public override byte RoleID => 216;

        public override string Name => nameof(Vigilante);

        public override Faction Faction => Faction.Crew;

        public override Alignment Alignment => Alignment.Killing;

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
        protected override void ClearSettingsInternal()
        {

        }

        protected override void SetConfigSettings()
        {
            Cooldown = Main.OptionVigilanteCooldown.GetValue();
        }

        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            PlayerControl target = PlayerTools.FindClosestTarget(Player);
            if (target == null) return;

            Player.SetKillTimer(Cooldown);

            if (SpecialRoleIsAssigned<Doctor>(out var doctorKvp) && doctorKvp.Value.ShieldedPlayer?.PlayerId == target.PlayerId)
            {
                doctorKvp.Value.BreakShield();
                return;
            }

            if (SpecialRoleIsAssigned(out KeyValuePair<byte, Jester> jesterKvp) && target.PlayerId == jesterKvp.Key && jesterKvp.Value.canDieToVigilante || target.Data.IsImpostor)
            {
                KillPlayer(target);
            }
            else
            {
                KillPlayer(Player);
            }
            return;
        }

        protected override void InitializeRoleInternal()
        {

        }
    }
}