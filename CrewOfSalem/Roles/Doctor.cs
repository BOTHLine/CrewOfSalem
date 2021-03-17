using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Doctor : RoleGeneric<Doctor>
    {
        private static readonly int VisorColor    = Shader.PropertyToID("_VisorColor");
        private static readonly int Outline       = Shader.PropertyToID("_Outline");
        private static readonly int OutlineColor1 = Shader.PropertyToID("_OutlineColor");

        // Properties
        public int ShowShieldedPlayerOption { get; private set; }

        public PlayerControl ShieldedPlayer { get; set; }

        // Properties Role
        public override byte   RoleID => 218;
        public override string Name   => nameof(Doctor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override bool   HasSpecialButton => true;
        public override Sprite SpecialButton    => DoctorButton;

        // Methods
        public void CheckShowShieldedPlayer()
        {
            if (ShieldedPlayer == null) return;

            switch (ShowShieldedPlayerOption)
            {
                case 0:
                    if (PlayerControl.LocalPlayer == Player) ShowShieldedPlayer();
                    break;
                case 1:
                    if (PlayerControl.LocalPlayer == ShieldedPlayer) ShowShieldedPlayer();
                    break;
                case 2:
                    if (PlayerControl.LocalPlayer == Player || PlayerControl.LocalPlayer == ShieldedPlayer)
                        ShowShieldedPlayer();
                    break;
                case 3:
                    ShowShieldedPlayer();
                    break;
            }
        }

        private void ShowShieldedPlayer()
        {
            ShieldedPlayer.myRend.material.SetColor(VisorColor, ModdedPalette.shieldedColor);
            ShieldedPlayer.myRend.material.SetFloat(Outline, 1F);
            ShieldedPlayer.myRend.material.SetColor(OutlineColor1, ModdedPalette.shieldedColor);
        }

        public bool CheckShieldedPlayer(byte playerId)
        {
            return ShieldedPlayer != null && ShieldedPlayer.PlayerId == playerId;
        }

        public void BreakShield()
        {
            WriteImmediately(RPC.DoctorBreakShield);
            ShieldedPlayer.myRend.material.SetColor(VisorColor, Palette.VisorColor);
            ShieldedPlayer.myRend.material.SetFloat(Outline, 0F);
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
            CurrentDuration = Mathf.Max(0F, CurrentDuration - deltaTime);
            if (CurrentDuration <= 0F && ShieldedPlayer) BreakShield();
        }

        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown || ShieldedPlayer != null) return;
            CurrentDuration = Duration;
            ShieldedPlayer = PlayerTools.FindClosestTarget(Player);

            Player.SetKillTimer(Cooldown);

            MessageWriter writer = GetWriter(RPC.DoctorSetShielded);
            writer.Write(ShieldedPlayer.PlayerId);
            CloseWriter(writer);
        }

        protected override void ClearSettings()
        {
            ShieldedPlayer = null;
        }
    }
}