using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreScripts;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityWatch : Ability
    {
        // Fields
        private          PlayerControl watchedPlayer;
        private readonly List<Role>    visitors = new List<Role>();

        // Properties Ability
        protected override Sprite Sprite      => ButtonWatch;
        protected override bool   NeedsTarget => true;

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnWatched;
        protected override int                                OnBeforeUsePriority => 0;

        protected override RPC               RpcAction => RPC.Watch;
        protected override IEnumerable<byte> RpcData   => new[] {Button.CurrentTarget.PlayerId};

        // Constructors
        public AbilityWatch(Role owner, float cooldown) : base(owner, cooldown) { }

        private static readonly Func<Ability, PlayerControl, bool> UseOnWatched = (source, target) =>
        {
            if (source is AbilityProtect) return true;

            IEnumerable<AbilityWatch> watchAbilities =
                GetAllAbilities<AbilityWatch>().Where(watch => watch.watchedPlayer == target);

            foreach (AbilityWatch watch in watchAbilities)
            {
                watch.RpcAddVisitor(source.owner);
            }

            return true;
        };

        // Methods
        private void RpcAddVisitor(Role visitor)
        {
            ConsoleTools.Info("RPC Add Visitor: " + visitor.Name);
            if (AmongUsClient.Instance.AmClient)
            {
                AddVisitor(visitor);
            }

            WriteRPC(RPC.WatchVisitor, owner.Owner.PlayerId, visitor.RoleID);
        }

        public void AddVisitor(Role visitor)
        {
            visitors.Add(visitor);
            ConsoleTools.Info("Add Visitor: " + visitor.Name);
        }

        public void ParseResults()
        {
            if (watchedPlayer == null) return;

            string result = watchedPlayer.name + ": ";
            for (var i = 0; i < visitors.Count; i++)
            {
                result += visitors[i].Name;
                if (i == visitors.Count - 2)
                {
                    result += " and ";
                } else if (i != visitors.Count - 1)
                {
                    result += ", ";
                }
            }

            if (AmongUsClient.Instance.AmClient)
            {
                HudManager.Instance?.Chat.AddChat(owner.Owner, result);
            }

            if (result.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Telemetry.Instance?.SendWho();
            }

            visitors.Clear();
            watchedPlayer = null;
        }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && watchedPlayer == null;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            watchedPlayer = target;
            sendRpc = true;
            setCooldown = false;
        }

        protected override void UpdateButtonSprite()
        {
            if (watchedPlayer == null)
            {
                base.UpdateButtonSprite();
            } else
            {
                Button.renderer.color = watchedPlayer.GetPlayerColor();
                Button.renderer.material.SetFloat(ShaderDesat, 1F);
            }
        }

        protected override void MeetingStartInternal()
        {
            ParseResults();
        }
    }
}