using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
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
        public bool IsGuarding => isGuarding && !isInTask && !owner.Owner.Data.IsDead;
        public bool IsInTask   => isInTask;

        // Properties Ability
        protected override Sprite Sprite => ButtonGuard;

        protected override bool NeedsTarget => false;

        protected override RPC               RpcAction => RPC.ToggleGuard;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        public AbilityGuard(Role owner, float cooldown) : base(owner, cooldown)
        {
            AddOnBeforeUse(UseOnGuarded, 10);
        }

        private static readonly Func<Ability, PlayerControl, bool> UseOnGuarded = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityGuard abilityGuard = GetAllAbilities<AbilityGuard>().FirstOrDefault(guard =>
                guard.isGuarding && PlayerTools.IsPlayerInRange(guard.owner.Owner, target));
            if (abilityGuard == null) return true;

            abilityGuard.owner.Owner.RpcKillPlayer(source.owner.Owner);
            abilityGuard.owner.Owner.RpcKillPlayer(abilityGuard.owner.Owner, source.owner.Owner);
            return false;
        };

        // Methods
        public void RpcToggleInTask(bool inTask)
        {
            if (IsInTask == inTask) return;

            if (AmongUsClient.Instance.AmClient) ToggleInTask();

            WriteRPC(RPC.ToggleInTask);
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

            if (CanUse())
            {
                if (isGuarding)
                {
                    Button.renderer.color = new Color(0F / 255F, 255F / 255F, 0F / 255F, 128F / 255F);
                    Button.renderer.material.SetFloat(ShaderDesat, 0F);
                } else
                {
                    Button.renderer.color = Palette.EnabledColor;
                    Button.renderer.material.SetFloat(ShaderDesat, 0F);
                }
            } else
            {
                if (isGuarding)
                {
                    Button.renderer.color = new Color(0F / 255F, 128F / 255F, 0F / 255F, 255F / 255F);
                    Button.renderer.material.SetFloat(ShaderDesat, 1F);
                } else
                {
                    Button.renderer.color = Palette.DisabledColor;
                    Button.renderer.material.SetFloat(ShaderDesat, 1F);
                }
            }
        }
    }
}