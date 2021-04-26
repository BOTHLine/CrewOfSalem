using System;
using System.Collections.Generic;
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

        protected override RPC               RpcAction => RPC.ProtectStart;
        protected override IEnumerable<byte> RpcData   => new[] {ProtectTarget.PlayerId};

        protected override RPC               RpcEndAction => RPC.ProtectEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnProtected;
        protected override int                                OnBeforeUsePriority => 20;

        // Constructors
        public AbilityProtect(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override bool ShouldShowButton()
        {
            return MeetingHud.Instance == null && ExileController.Instance == null;
        }

        protected override bool CanUse()
        {
            return CurrentCooldown <= 0F && CurrentDuration <= 0F && !ProtectTarget.Data.IsDead;
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