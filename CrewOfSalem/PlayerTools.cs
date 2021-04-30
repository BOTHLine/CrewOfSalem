using System;
using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem
{
    [HarmonyPatch]
    public static class PlayerTools
    {
        public static PlayerControl GetPlayerById(byte id)
        {
            return GameData.Instance.GetPlayerById(id).Object;
        }

        public static PlayerControl FindClosestTarget(PlayerControl fromPlayer,
            Func<PlayerControl, bool> predicate = null)
        {
            PlayerControl closest = null;
            float maxDistance = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
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

        public static bool IsPlayerInUseRange(PlayerControl fromPlayer, PlayerControl toPlayer, float range = 0F)
        {
            float maxDistance = range > 0F
                ? range
                : GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];

            if (!ShipStatus.Instance) return false;
            Vector2 fromPosition = fromPlayer.GetTruePosition();
            Vector2 distanceVector = toPlayer.GetTruePosition() - fromPosition;
            float distance = distanceVector.magnitude;
            return distance <= maxDistance && !PhysicsHelpers.AnyNonTriggersBetween(fromPosition,
                distanceVector.normalized, distance, Constants.ShipAndObjectsMask);
        }

        public static bool IsPlayerInRange(PlayerControl fromPlayer, PlayerControl toPlayer, float range = 0F)
        {
            float maxDistance = range > 0F
                ? range
                : GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];

            if (!ShipStatus.Instance) return false;
            Vector2 fromPosition = fromPlayer.GetTruePosition();
            Vector2 distanceVector = toPlayer.GetTruePosition() - fromPosition;
            float distance = distanceVector.magnitude;
            return distance <= maxDistance;
        }
    }
}