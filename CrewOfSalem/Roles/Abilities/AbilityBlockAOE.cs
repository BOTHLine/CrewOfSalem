using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityBlockAOE : AbilityDuration
    {
        // Fields
        private readonly List<PlayerControl> blockedPlayers = new List<PlayerControl>();

        // Properties
        public List<PlayerControl> BlockedPlayers => blockedPlayers;

        // Properties Ability
        protected override Sprite Sprite      => ButtonBlock;
        protected override bool   NeedsTarget => true;

        protected override RPC RpcAction => RPC.BlockAOEStart;

        protected override IEnumerable<byte> RpcData
        {
            get
            {
                var bytes = new byte[blockedPlayers.Count + 1];
                bytes[0] = (byte) blockedPlayers.Count;
                for (var i = 0; i < blockedPlayers.Count; i++)
                {
                    bytes[i + 1] = blockedPlayers[i].PlayerId;
                }

                return bytes;
            }
        }

        protected override RPC               RpcEndAction => RPC.BlockAOEEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityBlockAOE(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            foreach (PlayerControl player in AllPlayers)
            {
                if (player == LocalPlayer) continue;
                if (owner.Owner.GetRole().Faction == Faction.Mafia &&
                    player.GetRole().Faction == Faction.Mafia) continue;
                if (!PlayerTools.IsPlayerInRange(owner.Owner, player)) continue;

                blockedPlayers.Add(player);
            }

            foreach (Ability ability in target.GetRole().GetAllAbilities())
            {
                ability.AddCooldown(Duration);
            }

            sendRpc = setCooldown = true;
        }

        // TODO: Add Target and AOE everything around it? Use OnBeforeUses on every target in UseInternal? Or only on selected Target?
        protected override void UpdateTarget()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                if (player == LocalPlayer) continue;
                if (owner.Owner.GetRole().Faction == Faction.Mafia &&
                    player.GetRole().Faction == Faction.Mafia) continue;
                if (!PlayerTools.IsPlayerInRange(owner.Owner, player)) continue;

                player.myRend.material.SetFloat(ShaderOutline, 1F);
                player.myRend.material.SetColor(ShaderOutlineColor, Color.red);
            }
        }

        protected override void EffectEndInternal()
        {
            blockedPlayers.Clear();
        }
    }
}