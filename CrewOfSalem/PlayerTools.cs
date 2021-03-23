using System;
using HarmonyLib;
using UnityEngine;
using System.Linq;

namespace CrewOfSalem
{
    [HarmonyPatch]
    public static class PlayerTools
    {
        public static PlayerControl GetPlayerById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }

            return null;
        }

        public static PlayerControl FindClosestTarget(PlayerControl fromPlayer,
            Func<PlayerControl, bool> predicate = null)
        {
            PlayerControl closest = null;
            float maxDistance =
                GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
            if (!ShipStatus.Instance) return null;
            Vector2 fromPosition = fromPlayer.GetTruePosition();
            GameData.PlayerInfo[] allPlayers = GameData.Instance.AllPlayers.ToArray();
            foreach (GameData.PlayerInfo playerInfo in allPlayers)
            {
                if (playerInfo.Disconnected || playerInfo.PlayerId == fromPlayer.PlayerId || playerInfo.IsDead)
                {
                    continue;
                }

                PlayerControl current = playerInfo.Object;
                if (!current)
                {
                    continue;
                }

                if (predicate != null && !predicate.Invoke(current))
                {
                    continue;
                }

                Vector2 distanceVector = current.GetTruePosition() - fromPosition;
                float distance = distanceVector.magnitude;
                if (!(distance <= maxDistance) || PhysicsHelpers.AnyNonTriggersBetween(fromPosition,
                    distanceVector.normalized, distance, Constants.ShipAndObjectsMask))
                {
                    continue;
                }

                closest = current;
                maxDistance = distance;
            }

            return closest;
        }
    }
}