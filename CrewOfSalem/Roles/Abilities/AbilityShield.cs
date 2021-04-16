using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityShield : Ability
    {
        // Fields
        private PlayerControl shieldedPlayer;

        // Properties
        public PlayerControl ShieldedPlayer => shieldedPlayer;

        // Properties Ability
        protected override Sprite Sprite      => ButtonShield;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.Shield;
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityShield(Role owner, float cooldown) : base(owner, cooldown)
        {
            AddOnBeforeUse(UseOnShielded, 30);
        }

        private static readonly Func<Ability, PlayerControl, bool> UseOnShielded = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityShield abilityShield = GetAllAbilities<AbilityShield>()
               .FirstOrDefault(shield => shield.ShieldedPlayer == target);

            if (abilityShield == null) return true;

            source.SetOnCooldown();
            abilityShield.RpcBreakShield();
            return false;
        };

        // Methods
        public void RpcBreakShield()
        {
            if (AmongUsClient.Instance.AmClient)
            {
                BreakShield();
            }
            WriteRPC(RPC.ShieldBreak, owner.Owner.PlayerId);
        }
        
        public void BreakShield()
        {
            UnshowShieldedPlayer();
            shieldedPlayer = null;
            SetOnCooldown();
        }

        private void CheckShowShieldedPlayer()
        {
            if (shieldedPlayer == null) return;

            switch (Main.OptionDoctorShowShieldedPlayer.GetValue())
            {
                case 0:
                    if (LocalPlayer == owner.Owner) ShowShieldedPlayer(); // Doctor
                    break;
                case 1:
                    if (LocalPlayer == shieldedPlayer) ShowShieldedPlayer(); // Target
                    break;
                case 2:
                    if (LocalPlayer == owner.Owner || LocalPlayer == shieldedPlayer) // Doctor & Target
                        ShowShieldedPlayer();
                    break;
                case 3:
                    ShowShieldedPlayer(); // All
                    break;
            }
        }

        private void ShowShieldedPlayer()
        {
            shieldedPlayer.myRend.material.SetColor(ShaderVisorColor, ModdedPalette.shieldedColor);
            shieldedPlayer.myRend.material.SetFloat(ShaderOutline, 1F);
            shieldedPlayer.myRend.material.SetColor(ShaderOutlineColor, ModdedPalette.shieldedColor);
        }

        private void UnshowShieldedPlayer()
        {
            if (shieldedPlayer == null) return;
            if (shieldedPlayer.myRend.material.GetColor(ShaderVisorColor) != ModdedPalette.shieldedColor) return;
            
            shieldedPlayer.myRend.material.SetColor(ShaderVisorColor, Palette.VisorColor);
            shieldedPlayer.myRend.material.SetFloat(ShaderOutline, 0F);
        }

        public static void CheckShowShieldedPlayers()
        {
            IReadOnlyList<AbilityShield> abilityShields = GetAllAbilities<AbilityShield>();
            if (abilityShields == null) return;
            foreach (AbilityShield abilityShield in abilityShields)
            {
                abilityShield.UnshowShieldedPlayer();
                abilityShield.CheckShowShieldedPlayer();
            }
        }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && ShieldedPlayer == null;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            shieldedPlayer = target;
            CheckShowShieldedPlayer();
            sendRpc = true;
            setCooldown = false;
        }

        // Methods AbilityDuration
        protected override void UpdateButtonSprite()
        {
            if (ShieldedPlayer == null)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = shieldedPlayer.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
                Button.TimerText.gameObject.SetActive(false);
            }
        }
    }
}