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

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnGuarded;
        protected override int                                OnBeforeUsePriority => 10;

        public AbilityGuard(Role owner, float cooldown) : base(owner, cooldown) { }

        private static readonly Func<Ability, PlayerControl, bool> UseOnGuarded = (source, target) =>
        {
            if (!(source is AbilityKill)) return true;

            AbilityGuard abilityGuard = GetAllAbilities<AbilityGuard>().FirstOrDefault(guard =>
                guard.IsGuarding && target != guard.owner.Owner && PlayerTools.IsPlayerInRange(guard.owner.Owner,
                    target, Main.OptionBodyguardGuardRange.GetValue()));
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
                    Button.renderer.color = owner.Owner.GetPlayerColor();
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
                    Button.renderer.color = owner.Owner.GetShadowColor();
                    Button.renderer.material.SetFloat(ShaderDesat, 1F);
                } else
                {
                    Button.renderer.color = Palette.DisabledClear;
                    Button.renderer.material.SetFloat(ShaderDesat, 1F);
                }
            }
        }
    }
}