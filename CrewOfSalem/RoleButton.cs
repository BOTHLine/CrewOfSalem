using System;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;
using Object = UnityEngine.Object;

namespace CrewOfSalem
{
    public class RoleButton
    {
        private readonly KillButtonManager killButtonManager;

        private readonly float cooldown;
        private          float currentCooldown;

        private readonly Sprite  sprite;
        private readonly Vector3 positionOffset;

        private readonly Action     onMeetingEnds;
        private readonly Func<bool> canUse;
        private readonly Func<bool> onUse;

        public PlayerControl Target
        {
            get => killButtonManager.CurrentTarget;
            set => killButtonManager.SetTarget(value);
        }

        public RoleButton(float cooldown, Func<bool> onUse, Func<bool> canUse, Action onMeetingEnds,
            Sprite sprite, Vector3 positionOffset)
        {
            this.cooldown = cooldown;
            currentCooldown = cooldown;

            this.canUse = canUse;
            this.onUse = onUse;
            this.onMeetingEnds = onMeetingEnds;

            this.sprite = sprite;
            this.positionOffset = positionOffset;

            HudManager hudManager = HudManager.Instance;
            killButtonManager = hudManager.KillButton;
            var button = killButtonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction) Use);

            SetActive(false);
        }

        public void Use()
        {
            ConsoleTools.Info("Use Button");
            if (!canUse() || currentCooldown > 0F || !onUse()) return;

            killButtonManager.renderer.color = new Color(1F, 1F, 1F, 0.3F);
            currentCooldown = cooldown;
            killButtonManager.SetCoolDown(this.cooldown, this.cooldown);
        }

        public void HudUpdate()
        {
            if (MeetingHud.Instance || ExileController.Instance) return;
            Update();
        }

        public void OnMeetingEnds()
        {
            onMeetingEnds?.Invoke();
            Update();
        }

        public void ResetCooldown()
        {
            currentCooldown = cooldown;
            Update();
        }

        private void SetActive(bool isActive)
        {
            killButtonManager.gameObject.SetActive(isActive);
            killButtonManager.renderer.enabled = isActive;
        }

        private void Update()
        {
            if (PlayerControl.LocalPlayer.Data == null)
            {
                SetActive(false);
                return;
            }

            SetActive(HudManager.Instance.UseButton.isActiveAndEnabled);

            killButtonManager.renderer.sprite = sprite;

            // if (killButtonManager.transform.position == HudManager.Instance.KillButton.transform.position)
            {
                Transform transform = killButtonManager.transform;
                Vector3 vector = transform.localPosition;
                vector += new Vector3(positionOffset.x, positionOffset.y);
                transform.localPosition = vector;
            }

            if (canUse())
            {
                killButtonManager.renderer.color = Palette.EnabledColor;
                killButtonManager.renderer.material.SetFloat(ShaderDesat, 0F);
            } else
            {
                killButtonManager.renderer.color = Palette.DisabledColor;
                killButtonManager.renderer.material.SetFloat(ShaderDesat, 1F);
            }

            currentCooldown = Mathf.Max(0F, currentCooldown - Time.deltaTime);

            killButtonManager.SetCoolDown(currentCooldown, cooldown);
        }

        public void AddCooldown(float time)
        {
            currentCooldown += time;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(killButtonManager);
        }
    }
}