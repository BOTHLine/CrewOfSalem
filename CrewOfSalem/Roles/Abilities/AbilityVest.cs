using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityVest : AbilityDuration
    {
        private static readonly Func<Ability, PlayerControl, bool> UseOnVested = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityVest abilityVest = GetAllAbilities<AbilityVest>()
               .FirstOrDefault(vest => vest.owner.Owner == target && vest.HasDurationLeft);

            if (abilityVest == null) return true;

            source.SetOnCooldown();
            return false;
        };

        // Properties Ability
        protected override Sprite Sprite      => ButtonVest;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.VestStart;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        protected override RPC               RpcEndAction => RPC.VestEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnVested;
        protected override int                                OnBeforeUsePriority => 20;

        // Constructors
        public AbilityVest(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = true;
        }

        // Methods Ability Duration
        protected override void EffectEndInternal() { }

        protected override void UpdateButtonSprite()
        {
            if (!HasDurationLeft)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = owner.Owner.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }
    }
}