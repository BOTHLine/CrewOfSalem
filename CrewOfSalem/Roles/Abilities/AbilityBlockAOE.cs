using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override RPC RpcAction => RPC.None;

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

        protected override RPC               RpcEndAction => RPC.BlockAoeEnd;
        protected override IEnumerable<byte> RpcEndData   => new byte[0];

        // Constructors
        public AbilityBlockAOE(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods
        private void RPCBlockPlayers(PlayerControl target, IEnumerable<PlayerControl> players)
        {
            if (AmongUsClient.Instance.AmClient)
            {
                foreach (PlayerControl player in players)
                {
                    var doContinue = false;

                    if (player != target)
                    {
                        foreach (KeyValuePair<int, Func<Ability, PlayerControl, bool>> keyValuePair in OnBeforeUses)
                        {
                            if (!keyValuePair.Value.Invoke(this, player))
                            {
                                doContinue = true;
                                break;
                            }
                        }
                    }

                    if (doContinue) continue;

                    foreach (Ability ability in player.GetRole().GetAllAbilities())
                    {
                        ability.AddCooldown(Duration);
                        if (ability is AbilityDuration abilityDuration) abilityDuration.EffectEnd();
                    }
                }
            }

            WriteRPC(RPC.BlockAoeStart, RpcData.ToArray());
        }

        public void BlockPlayers(IEnumerable<PlayerControl> players)
        {
            foreach (PlayerControl player in players)
            {
                foreach (Ability ability in player.GetRole().GetAllAbilities())
                {
                    ability.AddCooldown(Duration);
                    if (ability is AbilityDuration abilityDuration) abilityDuration.EffectEnd();
                }
            }
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            blockedPlayers.Clear();

            foreach (PlayerControl player in AllPlayers)
            {
                if (player == LocalPlayer) continue;
                if (owner.Owner.GetRole().Faction == Faction.Mafia &&
                    player.GetRole().Faction == Faction.Mafia) continue;
                if (!PlayerTools.IsPlayerInUseRange(owner.Owner, player)) continue;

                blockedPlayers.Add(player);
            }

            RPCBlockPlayers(target, blockedPlayers);

            sendRpc = false;
            setCooldown = true;
        }

        // TODO: Add Target and AOE everything around it? Use OnBeforeUses on every target in UseInternal? Or only on selected Target?
        protected override void UpdateTarget()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                if (player == LocalPlayer) continue;
                if (owner.Owner.GetRole().Faction == Faction.Mafia &&
                    player.GetRole().Faction == Faction.Mafia) continue;
                if (!PlayerTools.IsPlayerInUseRange(owner.Owner, player))
                {
                    player.myRend.material.SetFloat(ShaderOutline, 0F);
                    continue;
                }

                player.myRend.material.SetFloat(ShaderOutline, 1F);
                player.myRend.material.SetColor(ShaderOutlineColor, Color.green);
            }

            Button.SetTarget(LocalPlayer == owner.Owner ? PlayerTools.FindClosestTarget(owner.Owner) : null);
        }

        protected override void EffectEndInternal()
        {
            blockedPlayers.Clear();
        }
    }
}