using UnityEngine;

namespace CrewOfSalem.Roles.Abilities
{
    public abstract class AbilityDuration<T> : Ability<T>
        where T : Role
    {
        // Fields
        protected readonly float duration;
        private            float currentDuration;

        // Properties
        protected float CurrentDuration
        {
            get => currentDuration;
            set => currentDuration = value;
        }

        // Constructors
        protected AbilityDuration(T role, float cooldown, float duration, Sprite sprite, Vector3 offset) : base(role,
            cooldown, sprite, offset)
        {
            this.duration = duration;
            currentDuration = 0F;
        }

        // Methods
        protected override bool CanUse()
        {
            return base.CanUse() && currentDuration <= 0F;
        }

        protected override void Use()
        {
            if (!CanUse()) return;

            CurrentCooldown = cooldown;
            currentDuration = duration;
            UseInternal();
        }

        protected override void UpdateInternal(float deltaTime)
        {
            currentDuration = Mathf.Max(0F, currentDuration - deltaTime);
        }
    }
}