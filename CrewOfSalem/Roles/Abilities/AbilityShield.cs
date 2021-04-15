using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityShield : AbilityDuration
    {
        // Fields
        private PlayerControl shieldedPlayer;

        // Properties
        public PlayerControl ShieldedPlayer => shieldedPlayer;

        // Properties Ability
        protected override Sprite Sprite      => ButtonShield;
        protected override bool   NeedsTarget => true;

        protected override RPC               RpcAction => RPC.ShieldStart;
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        protected override RPC               RpcEndAction => RPC.ShieldEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityShield(Role owner, float cooldown, float duration) : base(owner, cooldown, duration)
        {
            AddOnBeforeUse(UseOnShielded, 30);
        }

        private static readonly Func<Ability, PlayerControl, bool> UseOnShielded = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityShield abilityShield = GetAllAbilities<AbilityShield>()
               .FirstOrDefault(shield => shield.ShieldedPlayer == target && shield.HasDurationLeft);

            if (abilityShield == null) return true;

            source.SetOnCooldown();
            return false;
        };

        // Methods
        public bool CheckShieldedPlayer(PlayerControl player)
        {
            return shieldedPlayer != null && shieldedPlayer.PlayerId == player.PlayerId;
        }

        private void CheckShowShieldedPlayer()
        {
            if (shieldedPlayer == null) return;

            switch (Main.OptionDoctorShowShieldedPlayer.GetValue())
            {
                case 0:
                    if (PlayerControl.LocalPlayer == owner.Owner) ShowShieldedPlayer();
                    break;
                case 1:
                    if (PlayerControl.LocalPlayer == shieldedPlayer) ShowShieldedPlayer();
                    break;
                case 2:
                    if (PlayerControl.LocalPlayer == owner.Owner || PlayerControl.LocalPlayer == shieldedPlayer)
                        ShowShieldedPlayer();
                    break;
                case 3:
                    ShowShieldedPlayer();
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
            sendRpc = setCooldown = true;
        }

        // Methods AbilityDuration
        protected override void EffectEndInternal()
        {
            UnshowShieldedPlayer();
            shieldedPlayer = null;
        }


        protected override void UpdateButtonSprite()
        {
            if (ShieldedPlayer == null)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = Palette.PlayerColors[ShieldedPlayer.Data.ColorId];
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }
    }
}