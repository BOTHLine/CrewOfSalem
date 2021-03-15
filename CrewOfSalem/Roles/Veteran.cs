using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using System;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Veteran : RoleGeneric<Veteran>
    {
        // Properties
        public float AlertDuration { get; private set; }

        public float CurrentAlertDuration { get; set; }
        public bool IsOnAlert => CurrentAlertDuration > 0F;

        // Properties Role
        public override byte RoleID => 215;
        public override string Name => nameof(Veteran);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override Color Color => Color.green;
        protected override string StartText => "Survive the Impostors";

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton => VeteranButton;

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            Player.SetKillTimer(Cooldown);
            CurrentAlertDuration = AlertDuration;

            MessageWriter writer = GetWriter(RPC.VeteranAlert);
            CloseWriter(writer);

            return;
        }

        protected override void ClearSettingsInternal()
        {
            CurrentAlertDuration = 0F;
        }

        protected override void SetConfigSettings()
        {
            AlertDuration = Main.OptionVeteranAlertDuration.GetValue();
        }

        public override void UpdateCooldown(float deltaTime)
        {
            base.UpdateCooldown(deltaTime);
            CurrentAlertDuration = Mathf.Max(0F, CurrentAlertDuration - deltaTime);
        }
    }
}
