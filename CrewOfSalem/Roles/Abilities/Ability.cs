using System;
using System.Collections.Generic;
using System.Linq;
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

        // TODO: Make Kills to use AbilityKill (Like the one from Bodyguard, so that he/his target can be protected by shield)?
        // TODO: Instead of only returning bool, return a struct with multiple states (doContinue, doSetOnCooldown etc.)
        private static readonly SortedDictionary<int, Func<Ability, PlayerControl, bool>> OnBeforeUse =
            new SortedDictionary<int, Func<Ability, PlayerControl, bool>>();

        // Properties
        protected          KillButtonManager Button      => button;
        protected abstract bool              NeedsTarget { get; }

        public float Cooldown => cooldown;

        protected float CurrentCooldown
        {
            get => currentCooldown;
            set
            {
                currentCooldown = value;
                if (Button == null)
                {
                    ConsoleTools.Error("Button is null in: " + GetType().Name);
                    return;
                } else if (Button.renderer == null)
                {
                    ConsoleTools.Error("Button.renderer is null in: " + GetType().Name);
                    return;
                } else if (Button.TimerText == null)
                {
                    ConsoleTools.Error("Button.TimerText is null in: " + GetType().Name);
                    return;
                }

                UpdateButtonCooldown();
            }
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

        // Constructors
        // TODO: Add Start-Cooldown and AfterMeeting-Cooldown?
        protected Ability(Role owner, float cooldown)
        {
            AddNewAbility();

            this.owner = owner;
            this.cooldown = cooldown > 0F ? cooldown : 1F;

            HudManager hudManager = HudManager.Instance;
            button = Object.Instantiate(hudManager.KillButton, hudManager.transform);
            var buttonClick = Button.GetComponent<PassiveButton>();
            buttonClick.transform.position = hudManager.KillButton.transform.position;
            buttonClick.OnClick = new Button.ButtonClickedEvent();
            buttonClick.OnClick.AddListener((UnityAction) TryUse);

            Button.name = GetType().Name;
            Button.renderer.material.name = Button.name;

            CurrentCooldown = 10F;

            SetActive(false);

            foreach (PlayerControl player in AllPlayers)
            {
                player.GetComponent<SpriteRenderer>().material.SetFloat(ShaderOutline, 0F);
            }
        }

        // Methods
        public static void AddOnBeforeUse(Func<Ability, PlayerControl, bool> func, int priority)
        {
            while (OnBeforeUse.ContainsKey(priority))
            {
                priority++;
            }

            OnBeforeUse.Add(priority, func);
        }

        protected virtual bool ShouldShowButton()
        {
            return !owner.Owner.Data.IsDead && MeetingHud.Instance == null;
        }

        protected virtual bool CanUse()
        {
            if (MeetingHud.Instance) return false;
            if (ExileController.Instance) return false;
            if (Minigame.Instance) return false;
            if (Button == null)
            {
                ConsoleTools.Error("Button is null in " + GetType().Name);
                return false;
            }

            return Button.isActiveAndEnabled && CurrentCooldown <= 0F &&
                   (!NeedsTarget || Button.CurrentTarget != null) && !owner.Owner.Data.IsDead;
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
                PlayerControl target = Button.CurrentTarget;
                foreach (KeyValuePair<int, Func<Ability, PlayerControl, bool>> keyValuePair in OnBeforeUse)
                {
                    if (!keyValuePair.Value.Invoke(this, target)) return;
                }

                Use(target, out sendRpc);
            }

            if (!sendRpc || RpcAction == RPC.None) return;
            WriteRPC(RpcAction, RpcData.ToArray());
        }

        public virtual void Use(PlayerControl target, out bool sendRpc)
        {
            UseInternal(target, out sendRpc, out bool setCooldown);

            if (!setCooldown) return;

            CurrentCooldown = Cooldown;
        }

        protected abstract void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown);

        public void Update(float deltaTime)
        {
            if (Button == null) return;
            SetActive(ShouldShowButton());
            if (MeetingHud.Instance || ExileController.Instance) return;
            if (LocalData == null)
            {
                SetActive(false);
                return;
            }

            UpdateCooldown(deltaTime);
            UpdateTarget();
            UpdateButtonSprite();
            UpdatePosition();

            CooldownHelpers.SetCooldownNormalizedUvs(Button.renderer);

            UpdateInternal(deltaTime);
        }

        private void SetActive(bool active)
        {
            if (HudManager.Instance?.KillButton != null)
            {
                HudManager.Instance.KillButton.gameObject.SetActive(false);
                HudManager.Instance.KillButton.renderer.enabled = false;
                HudManager.Instance.KillButton.isActive = false;
                HudManager.Instance.KillButton.enabled = false;
                HudManager.Instance.KillButton.SetTarget(null);
            }

            if (Button != null)
            {
                Button.gameObject.SetActive(active);
                Button.renderer.enabled = active;
                Button.isActive = active;
                Button.enabled = active;
            }
        }

        private void UpdateCooldown(float deltaTime)
        {
            CurrentCooldown = Mathf.Max(0F, CurrentCooldown - deltaTime);
        }

        protected virtual void UpdateButtonCooldown()
        {
            /*
            if (Button == null) return;
            float percent = Mathf.Clamp01(CurrentCooldown / Cooldown);
            ConsoleTools.Info("Percent: " + percent);
            Button.renderer.material.SetFloat(ShaderPercent, percent);
            Button.isCoolingDown = percent > 0F;
            if (Button.isCoolingDown)
            {
                Button.TimerText.Text = Mathf.CeilToInt(CurrentCooldown).ToString();
            }

            Button.TimerText.gameObject.SetActive(Button.isCoolingDown);
            */
            if (Button == null || Button.renderer == null) return;
            Button?.SetCoolDown(CurrentCooldown, Cooldown);
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
                Button.renderer.color = Palette.DisabledClear;
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }

        protected virtual void UpdateTarget()
        {
            Button.SetTarget(NeedsTarget && LocalPlayer == owner.Owner
                ? PlayerTools.FindClosestTarget(owner.Owner)
                : null);
        }

        private void UpdatePosition()
        {
            // if (Button.transform.position != HudManager.Instance.KillButton.transform.position) return;

            Button.transform.position = HudManager.Instance.UseButton.transform.position + Offset;
        }

        protected virtual void UpdateInternal(float deltaTime) { }

        public void AddCooldown(float addedTime)
        {
            CurrentCooldown += addedTime;
        }

        public void SetCooldown(float cooldown)
        {
            CurrentCooldown = cooldown;
        }

        public void SetOnCooldown()
        {
            CurrentCooldown = Cooldown;
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

        public virtual void Destroy()
        {
            if (Button != null && Button.gameObject != null) Object.Destroy(Button.gameObject);
            AllAbilities[GetType()].Remove(this);
        }
    }
}