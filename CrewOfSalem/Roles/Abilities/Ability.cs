using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public abstract class Ability<T>
        where T : Role
    {
        // Fields
        protected readonly T role;

        protected readonly KillButtonManager buttonManager;
        protected readonly float             cooldown;
        private            float             currentCooldown;

        // Properties
        protected abstract bool NeedsTarget { get; }

        protected float CurrentCooldown
        {
            get => currentCooldown;
            set => buttonManager.SetCoolDown(currentCooldown = value, cooldown);
        }

        protected Sprite Sprite
        {
            set => buttonManager.renderer.sprite = value;
        }

        protected Vector2 Offset
        {
            set
            {
                Transform transform = buttonManager.transform;
                Vector3 vector = transform.localPosition;
                vector += new Vector3(value.x, value.y);
                transform.localPosition = vector;
            }
        }

        // Constructors
        protected Ability(T role, float cooldown, Sprite sprite, Vector2 offset)
        {
            this.role = role;
            this.cooldown = cooldown;
            CurrentCooldown = cooldown;
            Sprite = sprite;
            Offset = offset;

            HudManager hudManager = HudManager.Instance;
            buttonManager = Object.Instantiate(hudManager.KillButton, hudManager.transform);
            var button = buttonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction) Use);
        }

        // Methods
        protected virtual bool CanUse()
        {
            return CurrentCooldown <= 0F && (!NeedsTarget || buttonManager.CurrentTarget != null);
        }

        protected virtual void Use()
        {
            if (!CanUse()) return;

            CurrentCooldown = cooldown;
            UseInternal();
        }

        protected abstract void UseInternal();

        public void Update(float deltaTime)
        {
            CurrentCooldown = Mathf.Max(0F, CurrentCooldown - deltaTime);

            if (CanUse())
            {
                buttonManager.renderer.color = Palette.EnabledColor;
                buttonManager.renderer.material.SetFloat(ShaderDesat, 0F);
            } else
            {
                buttonManager.renderer.color = Palette.DisabledColor;
                buttonManager.renderer.material.SetFloat(ShaderDesat, 1F);
            }

            if (NeedsTarget)
            {
                PlayerControl target = PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer);
                buttonManager.SetTarget(target);
            }

            UpdateInternal(deltaTime);
        }

        protected abstract void UpdateInternal(float deltaTime);

        public void AddCooldown(float addedTime)
        {
            CurrentCooldown += addedTime;
        }

        public void Destroy()
        {
            Object.Destroy(buttonManager);
        }
    }
}