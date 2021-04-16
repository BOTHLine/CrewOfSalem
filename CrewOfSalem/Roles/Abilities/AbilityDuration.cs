using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public abstract class AbilityDuration : Ability
    {
        // Fields
        protected readonly float duration;
        private            float currentDuration;

        protected bool isEffectActive;

        // Properties
        protected float Duration => duration;

        protected float CurrentDuration
        {
            get => currentDuration;
            set
            {
                currentDuration = value;
                if (HasDurationLeft) UpdateButtonCooldown();
            }
        }

        public bool HasDurationLeft => currentDuration > 0F;

        protected virtual RPC               RpcEndAction => RPC.None;
        protected virtual IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        protected AbilityDuration(Role owner, float cooldown, float duration) : base(owner, cooldown)
        {
            this.duration = duration;
            CurrentDuration = 0F;
        }

        // Methods
        public void RpcEffectEnd()
        {
            if (RpcEndAction == RPC.None) return;
            WriteRPC(RpcEndAction, RpcEndData.ToArray());
            if (AmongUsClient.Instance.AmClient) EffectEnd();
        }

        public void EffectEnd()
        {
            CurrentDuration = 0F;
            isEffectActive = false;
            EffectEndInternal();
        }

        protected abstract void EffectEndInternal();

        // Methods Ability
        protected override bool ShouldShowButton()
        {
            return base.ShouldShowButton() || HasDurationLeft;
        }

        protected override bool CanUse()
        {
            return base.CanUse() && CurrentDuration <= 0F;
        }

        public sealed override void Use(PlayerControl target, out bool sendRpc)
        {
            UseInternal(target, out sendRpc, out bool setCooldown);

            if (!setCooldown) return;

            isEffectActive = true;
            CurrentCooldown = Cooldown;
            CurrentDuration = Duration;
        }

        protected override void UpdateButtonCooldown()
        {
            if (HasDurationLeft)
            {
                Button.SetCoolDown(CurrentDuration, Duration);
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            } else
            {
                base.UpdateButtonCooldown();
            }
        }

        protected override void UpdateButtonSprite()
        {
            if (HasDurationLeft)
            {
                Button.renderer.color = owner.Owner.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            } else
            {
                base.UpdateButtonSprite();
            }
        }

        protected sealed override void UpdateInternal(float deltaTime)
        {
            CurrentDuration = Mathf.Max(0F, CurrentDuration - deltaTime);
            if (HasDurationLeft || !isEffectActive) return;

            isEffectActive = false;
            RpcEffectEnd();
        }

        public override void Destroy()
        {
            base.Destroy();
            if (HasDurationLeft) EffectEnd();
        }
    }
}