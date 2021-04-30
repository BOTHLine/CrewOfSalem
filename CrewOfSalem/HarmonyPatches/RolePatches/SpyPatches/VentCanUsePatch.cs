using System;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SpyPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(Vent __instance, out float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            var distance = float.MaxValue;
            PlayerControl player = pc.Object;

            couldUse = !player.Data.IsDead && (player.Data.IsImpostor || player.GetRole() is Spy) &&
                       (player.CanMove || player.inVent);
            canUse = false;

            if ((DateTime.UtcNow - PlayerVentTimeUtility.GetLastVent(player.PlayerId)).TotalMilliseconds > 550F)
            {
                Vector2 truePosition = player.GetTruePosition();
                Vector3 position = __instance.transform.position;
                distance = Vector2.Distance(truePosition, position);
                canUse = couldUse & distance <= __instance.UsableDistance &&
                         !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false);
            }

            __result = distance;
            return false;
        }
    }
}