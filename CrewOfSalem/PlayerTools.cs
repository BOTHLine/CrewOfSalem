using HarmonyLib;
using UnityEngine;

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

        public static PlayerControl FindClosestTarget(PlayerControl fromPlayer)
        {
            PlayerControl closest = null;
            float maxDistance = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];

            if (!ShipStatus.Instance)
                return null;

            Vector2 fromPosition = fromPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int index = 0; index < allPlayers.Count; index++)
            {
                GameData.PlayerInfo playerInfo = allPlayers[index];
                if (!playerInfo.Disconnected && playerInfo.PlayerId != fromPlayer.PlayerId && !playerInfo.IsDead)
                {
                    PlayerControl current = playerInfo.Object;
                    if (current)
                    {
                        Vector2 distanceVector = current.GetTruePosition() - fromPosition;
                        float distance = distanceVector.magnitude;
                        if (distance <= maxDistance && !PhysicsHelpers.AnyNonTriggersBetween(fromPosition, distanceVector.normalized, distance, Constants.ShipAndObjectsMask))
                        {
                            closest = current;
                            maxDistance = distance;
                        }
                    }
                }
            }
            return closest;
        }
    }
}