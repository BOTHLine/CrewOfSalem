using System;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityMap : Ability
    {
        // Fields
        private Action<MapBehaviour> action;

        // Properties Ability
        protected override Sprite Sprite      => ButtonMap;
        protected override bool   NeedsTarget => false;

        // Constructors
        public AbilityMap(Role owner, float cooldown) : base(owner, cooldown)
        {
            action = (m) => m.ShowCountOverlay();
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            PlayerControl.LocalPlayer.NetTransform.Halt();
            HudManager.Instance.ShowMap(action);

            sendRpc = false;
            setCooldown = true;

            if (!Main.OptionLookoutSharesCooldown) return;

            foreach (Ability ability in owner.GetAllAbilities())
            {
                ability.SetCooldown(Cooldown);
            }
        }
    }
}