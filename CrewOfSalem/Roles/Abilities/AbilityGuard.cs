using System.Collections.Generic;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityGuard : Ability
    {
        // Fields
        private bool isGuarding = false;
        private bool isInTask   = false;

        // Properties
        public bool IsGuarding => isGuarding;
        public bool IsInTask   => isInTask;

        // Properties Ability
        public AbilityGuard(Role owner, float cooldown) : base(owner, cooldown) { }
        protected override Sprite Sprite => ButtonKill;

        protected override bool NeedsTarget => false;

        protected override RPC               RpcAction => RPC.ToggleGuard;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId};

        // Methods
        public void RpcToggleInTask(bool inTask)
        {
            if (IsInTask == inTask) return;

            if (AmongUsClient.Instance.AmClient) ToggleInTask();

            MessageWriter writer = GetWriter(RPC.ToggleInTask);
            writer.Write(owner.Owner.PlayerId);
            CloseWriter(writer);
        }

        public void ToggleInTask()
        {
            isInTask = !IsInTask;
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            isGuarding = !isGuarding;
            sendRpc = setCooldown = true;
        }

        protected override void UpdateButtonSprite()
        {
            Button.renderer.sprite = Sprite;

            if (isGuarding)
            {
                Button.renderer.color = Palette.EnabledColor;
                Button.renderer.material.SetFloat(ShaderDesat, 0F);
            } else
            {
                Button.renderer.color = Palette.DisabledColor;
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }
    }
}