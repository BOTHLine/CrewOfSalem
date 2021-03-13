using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Escort : RoleGeneric<Escort>
    {
        // Properties
        public float CooldownIncrease { get; private set; }

        // Properties Role
        public override Color Color => Color.green;

        public override bool HasSpecialButton => true;

        public override Sprite SpecialButton => DefaultKillButton;

        public override byte RoleID => 221;

        public override string Name => nameof(Escort);

        public override Faction Faction => Faction.Crew;

        public override Alignment Alignment => Alignment.Support;

        protected override string StartText => "Block the [FF0000FF]Mafia[]";

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            PlayerControl target = PlayerTools.FindClosestTarget(Player);
            if (target == null) return;

            Player.SetKillTimer(Cooldown);
            target.SetKillTimer(target.killTimer + CooldownIncrease);

            MessageWriter writer = GetWriter(RPC.EscortIncreaseCooldown);
            writer.Write(target.PlayerId);
            CloseWriter(writer);

            return;
        }

        protected override void ClearSettingsInternal()
        {

        }

        protected override void InitializeRoleInternal()
        {
            Cooldown = Main.OptionEscortCooldown.GetValue();
            CooldownIncrease = Main.OptionEscortCooldownIncrease.GetValue();
        }

        protected override void SetConfigSettings()
        {

        }
    }
}
