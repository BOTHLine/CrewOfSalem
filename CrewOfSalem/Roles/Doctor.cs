using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Doctor : RoleGeneric<Doctor>
    {
        // Properties Role
        public override byte RoleID => 218;
        public override string Name => nameof(Doctor);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override Color Color => Color.cyan;
        protected override string StartText => "Protect the [000000FF]Crew[]";

        public override bool HasSpecialButton => true;
        public override Sprite SpecialButton { get => DoctorButton; }

        // Properties
        public int ShowShieldedPlayerOption { get; private set; }

        public PlayerControl ShieldedPlayer { get; set; }

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;
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
        }

        protected override void InitializeRoleInternal()
        {
            Player.SetKillTimer(10F);
        }

        protected override void SetConfigSettings()
        {
            Cooldown = Main.OptionDoctorCooldown.GetValue();
            ShowShieldedPlayerOption = Main.OptionDoctorShowShieldedPlayer.GetValue();
        }

        // Methods
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

        public void CheckShowShieldedPlayer()
        {
            if (ShieldedPlayer == null) return;

            int showShielded = ShowShieldedPlayerOption;

            if ((showShielded == 0 || showShielded == 2) && PlayerControl.LocalPlayer == Player)
            {
                ShowShieldedPlayer();
            }
            else if ((showShielded == 1 || showShielded == 2) && PlayerControl.LocalPlayer == ShieldedPlayer)
            {
                ShowShieldedPlayer();
            }
            else if (showShielded == 3)
            {
                ShowShieldedPlayer();
            }
        }

        private void ShowShieldedPlayer()
        {
            ShieldedPlayer.myRend.material.SetColor("_VisorColor", ModdedPalette.shieldedColor);
            ShieldedPlayer.myRend.material.SetFloat("_Outline", 1F);
            ShieldedPlayer.myRend.material.SetColor("_OutlineColor", ModdedPalette.shieldedColor);
        }

    }
}