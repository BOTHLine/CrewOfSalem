using System;
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
        private int ShowShieldedPlayerOption { get; set; }

        public PlayerControl ShieldedPlayer { get; set; }

        // Properties Role
        protected override byte   RoleID => 218;
        public override    string Name   => nameof(Doctor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => DoctorButton;

        protected override Func<bool> CanUse => () => ShieldedPlayer == null;

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
            ShieldedPlayer.myRend.material.SetColor(ShaderVisorColor, ModdedPalette.shieldedColor);
            ShieldedPlayer.myRend.material.SetFloat(ShaderOutline, 1F);
            ShieldedPlayer.myRend.material.SetColor(ShaderOutlineColor, ModdedPalette.shieldedColor);
        }

        public bool CheckShieldedPlayer(byte playerId)
        {
            return ShieldedPlayer != null && ShieldedPlayer.PlayerId == playerId;
        }

        public void BreakShield()
        {
            WriteImmediately(RPC.DoctorBreakShield);
            ShieldedPlayer.myRend.material.SetColor(ShaderVisorColor, Palette.VisorColor);
            ShieldedPlayer.myRend.material.SetFloat(ShaderOutline, 0F);
            ShieldedPlayer = null;
        }

        // Methods Role
        protected override void SetConfigSettings()
        {
            base.SetConfigSettings();
            ShowShieldedPlayerOption = Main.OptionDoctorShowShieldedPlayer.GetValue();
        }

        public override void UpdateDuration(float deltaTime)
        {
            base.UpdateDuration(deltaTime);
            if (!HasDurationLeft && ShieldedPlayer) BreakShield();
        }

        protected override bool PerformActionInternal()
        {
            ShieldedPlayer = SpecialButton.Target;

            MessageWriter writer = GetWriter(RPC.DoctorSetShielded);
            writer.Write(ShieldedPlayer.PlayerId);
            CloseWriter(writer);
            return true;
        }

        protected override void ClearSettingsInternal()
        {
            ShieldedPlayer = null;
        }
    }
}