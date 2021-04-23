using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilitySeance : AbilityDuration
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonSeance;
        protected override bool   NeedsTarget => false;

        // Constructors
        public AbilitySeance(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = false;
            setCooldown = true;
        }

        // Methods Ability Duration
        protected override void EffectEndInternal() { }
    }
}