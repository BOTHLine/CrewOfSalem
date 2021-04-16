using System;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityProtect : AbilityDuration
    {
        // Fields
        private static readonly Func<Ability, PlayerControl, bool> UseOnProtected = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityProtect abilityProtect = GetAllAbilities<AbilityProtect>()
               .FirstOrDefault(protect => protect.ProtectTarget == target && protect.HasDurationLeft);

            if (abilityProtect == null) return true;

            source.SetOnCooldown();
            return false;
        };

        // Properties
        public PlayerControl ProtectTarget { get; set; }

        // Properties Ability
        protected override Sprite Sprite      => ButtonProtect;
        protected override bool   NeedsTarget => false;

        // Constructors
        public AbilityProtect(Role owner, float cooldown, float duration) : base(owner, cooldown, duration)
        {
            AddOnBeforeUse(UseOnProtected, 20);
        }

        // Methods Ability
        protected override bool ShouldShowButton()
        {
            return true;
        }

        protected override bool CanUse()
        {
            return CurrentCooldown <= 0F && CurrentDuration <= 0F;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = true;
        }

        protected override void EffectEndInternal() { }

        protected override void UpdateButtonSprite()
        {
            if (HasDurationLeft)
            {
                Button.renderer.color = ProtectTarget.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            } else
            {
                base.UpdateButtonSprite();
            }
        }
    }
}