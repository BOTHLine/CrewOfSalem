using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static CrewOfSalem.CrewOfSalem;
using Object = UnityEngine.Object;

namespace CrewOfSalem.Roles.Abilities
{
    public abstract class Ability
    {
        // Fields
        private static readonly Dictionary<Type, List<Ability>> AllAbilities = new Dictionary<Type, List<Ability>>();

        public readonly  Role              owner;
        private readonly KillButtonManager button;

        private readonly float cooldown;
        private          float currentCooldown;

        protected abstract Sprite Sprite { get; }

        protected virtual RPC               RpcAction => RPC.None;
        protected virtual IEnumerable<byte> RpcData   => new byte[0];

        private Vector3 offset;

        // Properties
        public             KillButtonManager Button      => button;
        protected abstract bool              NeedsTarget { get; }

        protected float Cooldown => cooldown;

        protected float CurrentCooldown
        {
            get => currentCooldown;
            set
            {
                currentCooldown = value;
                UpdateButtonCooldown();
            }
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

        // Constructors
        protected Ability(Role owner, float cooldown)
        {
            AddNewAbility();

            this.owner = owner;
            this.cooldown = cooldown;

            HudManager hudManager = HudManager.Instance;
            button = Object.Instantiate(hudManager.KillButton, hudManager.transform);
            var buttonClick = button.GetComponent<PassiveButton>();
            buttonClick.transform.position = hudManager.KillButton.transform.position;
            buttonClick.OnClick = new Button.ButtonClickedEvent();
            buttonClick.OnClick.AddListener((UnityAction) TryUse);

            CurrentCooldown = 10F;

            Update(0F);

            buttonClick.OnClick.Invoke();
            SetActive(false);
        }

        // Methods
        protected virtual bool CanUse()
        {
            return CurrentCooldown <= 0F && (!NeedsTarget || Button.CurrentTarget != null);
        }

        public void TryUse()
        {
            if (!CanUse()) return;

            RpcUse();
        }

        private void RpcUse()
        {
            var sendRpc = false;
            if (AmongUsClient.Instance.AmClient)
            {
                Use(Button.CurrentTarget, out sendRpc);
            }

            if (!sendRpc || RpcAction == RPC.None) return;
            MessageWriter writer = GetWriter(RpcAction);
            foreach (byte data in RpcData) writer.Write(data);
            CloseWriter(writer);
        }

        public virtual void Use(PlayerControl target, out bool sendRpc)
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

            CurrentCooldown = Cooldown;
        }

        protected abstract void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown);

        public void Update(float deltaTime)
        {
            if (HudManager.Instance.KillButton == null) Destroy();
            if (MeetingHud.Instance || ExileController.Instance) return;
            if (PlayerControl.LocalPlayer.Data == null)
            {
                SetActive(false);
                return;
            }

            SetActive(HudManager.Instance.isActiveAndEnabled);
            UpdateCooldown(deltaTime);
            UpdateButtonSprite();
            UpdateTarget();
            UpdatePosition();

            UpdateInternal(deltaTime);
        }

        private void SetActive(bool active)
        {
            HudManager.Instance.KillButton.gameObject.SetActive(false);
            HudManager.Instance.KillButton.renderer.enabled = false;
            HudManager.Instance.KillButton.isActive = false;
            HudManager.Instance.KillButton.enabled = false;

            Button.gameObject.SetActive(active);
            Button.renderer.enabled = active;
            Button.isActive = active;
            Button.enabled = active;
        }

        private void UpdateCooldown(float deltaTime)
        {
            CurrentCooldown = Mathf.Max(0F, CurrentCooldown - deltaTime);
        }

        protected void UpdateButtonCooldown()
        {
            Button.SetCoolDown(CurrentCooldown, Cooldown);
        }

        protected virtual void UpdateButtonSprite()
        {
            Button.renderer.sprite = Sprite;

            if (CanUse())
            {
                Button.renderer.color = Palette.EnabledColor;
                Button.renderer.material.SetFloat(ShaderDesat, 0F);
            } else
            {
                Button.renderer.color = Palette.DisabledColor;
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }

        protected virtual void UpdateTarget()
        {
            Button.SetTarget(PlayerTools.FindClosestTarget(owner.Owner));
        }

        private void UpdatePosition()
        {
            if (Button.transform.position != HudManager.Instance.KillButton.transform.position) return;

            Button.transform.position = HudManager.Instance.KillButton.transform.position + offset;
        }

        protected virtual void UpdateInternal(float deltaTime) { }

        public void AddCooldown(float addedTime)
        {
            CurrentCooldown += addedTime;
        }

        private void AddNewAbility()
        {
            Type type = GetType();
            if (AllAbilities.ContainsKey(type))
            {
                if (AllAbilities[type].Contains(this)) return;
                AllAbilities[type].Add(this);
            } else
            {
                AllAbilities.Add(type, new List<Ability> {this});
            }
        }

        public static T[] GetAllAbilities<T>()
            where T : Ability
        {
            Type type = typeof(T);
            return AllAbilities.TryGetValue(type, out List<Ability> list)
                ? list.Select(ability => (T) ability).ToArray()
                : new T[0];
        }

        public void Destroy()
        {
            Object.Destroy(Button);
            AllAbilities[GetType()].Remove(this);
        }
    }
}