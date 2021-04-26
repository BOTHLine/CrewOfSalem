using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityWatch : Ability
    {
        // Fields
        private          PlayerControl watchedPlayer;
        private readonly List<Ability> targetedAbilities = new List<Ability>();

        // Properties Ability
        protected override Sprite Sprite      => ButtonWatch;
        protected override bool   NeedsTarget => true;

        protected override Func<Ability, PlayerControl, bool> OnBeforeUse         => UseOnWatched;
        protected override int                                OnBeforeUsePriority => 0;

        // Constructors
        public AbilityWatch(Role owner, float cooldown) : base(owner, cooldown) { }

        private static readonly Func<Ability, PlayerControl, bool> UseOnWatched = (source, target) =>
        {
            if (source is AbilityProtect) return true; // TODO: Show Guardian Angel on Target?

            IEnumerable<AbilityWatch> watchAbilities =
                GetAllAbilities<AbilityWatch>().Where(watch => watch.watchedPlayer == target);

            foreach (AbilityWatch watch in watchAbilities)
            {
                watch.AddTargetedAbility(source);
            }

            AbilityAlert abilityAlert = GetAllAbilities<AbilityAlert>()
               .FirstOrDefault(alert => alert.owner.Owner == target && alert.HasDurationLeft);
            if (abilityAlert == null) return true;

            source.owner.Owner.RpcKillPlayer(source.owner.Owner, abilityAlert.owner.Owner);
            return false;
        };

        // Methods
        private void AddTargetedAbility(Ability ability)
        {
            targetedAbilities.Add(ability);
        }

        public void ParseResults()
        {
            foreach (Ability ability in targetedAbilities)
            {
                HudManager.Instance.Chat.AddChat(owner.Owner, ability.GetType().Name);
            }

            targetedAbilities.Clear();
        }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && watchedPlayer == null;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            watchedPlayer = target;
            sendRpc = setCooldown = false;
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
    }
}