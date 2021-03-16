using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Doctor : RoleGeneric<Doctor>
    {
        // Properties
        public float CurrentShieldDuration { get; set; }
        public int ShowShieldedPlayerOption { get; private set; }

        public PlayerControl ShieldedPlayer { get; set; }

        // Properties Role
        public override byte RoleID => 218;
        public override string Name => nameof(Doctor);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override Color Color => Color.cyan;

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton { get => DoctorButton; }

        // Methods
        public void CheckShowShieldedPlayer()
        {
            if (ShieldedPlayer == null) return;

            switch (ShowShieldedPlayerOption)
            {
                case 0: if (PlayerControl.LocalPlayer == Player) ShowShieldedPlayer(); break;
                case 1: if (PlayerControl.LocalPlayer == ShieldedPlayer) ShowShieldedPlayer(); break;
                case 2: if (PlayerControl.LocalPlayer == Player || PlayerControl.LocalPlayer == ShieldedPlayer) ShowShieldedPlayer(); break;
                case 3: ShowShieldedPlayer(); break;
            }
        }

        private void ShowShieldedPlayer()
        {
            ShieldedPlayer.myRend.material.SetColor("_VisorColor", ModdedPalette.shieldedColor);
            ShieldedPlayer.myRend.material.SetFloat("_Outline", 1F);
            ShieldedPlayer.myRend.material.SetColor("_OutlineColor", ModdedPalette.shieldedColor);
        }

        public bool CheckShieldedPlayer(byte playerId)
        {
            return ShieldedPlayer != null && ShieldedPlayer.PlayerId == playerId;
        }

        public void BreakShield()
        {
            WriteImmediately(RPC.DoctorBreakShield);
            ShieldedPlayer.myRend.material.SetColor("_VisorColor", Palette.VisorColor);
            ShieldedPlayer.myRend.material.SetFloat("_Outline", 0F);
            ShieldedPlayer = null;
        }

        // Methods Role
        protected override void SetConfigSettings()
        {
            base.SetConfigSettings();
            ShowShieldedPlayerOption = Main.OptionDoctorShowShieldedPlayer.GetValue();
        }

        public override void CheckSpecialButton(HudManager instance)
        {
            if (instance.UseButton == null || !instance.UseButton.isActiveAndEnabled || Player.Data.IsDead) return;

            KillButtonManager killButton = instance.KillButton;
            killButton.gameObject.SetActive(true);
            killButton.renderer.enabled = true;
            killButton.isActive = true;
            killButton.renderer.sprite = SpecialButton;
            killButton.SetTarget(ShieldedPlayer == null ? PlayerTools.FindClosestTarget(Player) : null);
        }

        public override void UpdateCooldown(float deltaTime)
        {
            base.UpdateCooldown(deltaTime);
            CurrentShieldDuration = Mathf.Max(0F, CurrentShieldDuration - deltaTime);
            if (CurrentShieldDuration <= 0F && ShieldedPlayer) BreakShield();
        }

        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown || ShieldedPlayer != null) return;
            CurrentShieldDuration = Duration;
            ShieldedPlayer = PlayerTools.FindClosestTarget(Player);

            Player.SetKillTimer(Cooldown);

            MessageWriter writer = GetWriter(RPC.DoctorSetShielded);
            writer.Write(ShieldedPlayer.PlayerId);
            CloseWriter(writer);
            return;
        }

        protected override void ClearSettingsInternal()
        {
            ShieldedPlayer = null;
            CurrentShieldDuration = 0F;
        }
    }
}