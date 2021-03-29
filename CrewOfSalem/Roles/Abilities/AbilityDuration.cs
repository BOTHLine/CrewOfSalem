using System.Collections.Generic;
using Hazel;
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

            MessageWriter writer = GetWriter(RpcEndAction);
            foreach (byte data in RpcEndData) writer.Write(data);
            CloseWriter(writer);

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
        protected override bool CanUse()
        {
            return base.CanUse() && CurrentDuration <= 0F;
        }

        public sealed override void Use(PlayerControl target, out bool sendRpc)
        {
            IReadOnlyList<AbilityAlert> abilityAlerts = GetAllAbilities<AbilityAlert>();
            foreach (AbilityAlert abilityAlert in abilityAlerts)
            {
                if (abilityAlert.owner.Owner != target) continue;

                bool isImpostor = target.Data.IsImpostor;
                target.RpcMurderPlayer(target);
                target.Data.IsImpostor = isImpostor;
                sendRpc = false;
                return;
            }

            UseInternal(target, out sendRpc, out bool setCooldown);

            if (!setCooldown) return;

            isEffectActive = true;
            CurrentCooldown = Cooldown;
            CurrentDuration = Duration;
        }

        protected sealed override void UpdateInternal(float deltaTime)
        {
            CurrentDuration = Mathf.Max(0F, CurrentDuration - deltaTime);
            if (HasDurationLeft || !isEffectActive) return;

            isEffectActive = false;
            RpcEffectEnd();
        }
    }
}