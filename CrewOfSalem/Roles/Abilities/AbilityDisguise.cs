using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityDisguise : AbilityDuration
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonDisguise;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.DisguiseStart;
        protected override IEnumerable<byte> RpcData   => new byte[0];

        protected override RPC               RpcEndAction => RPC.DisguiseEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityDisguise(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods
        public bool IsPlayerInRange(PlayerControl player)
        {
            return PlayerTools.IsPlayerInRange(owner.Owner, player, Main.OptionDisguiserRange);
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = true;
        }

        // Methods AbilityDuration
        protected override void EffectEndInternal() { }
    }
}