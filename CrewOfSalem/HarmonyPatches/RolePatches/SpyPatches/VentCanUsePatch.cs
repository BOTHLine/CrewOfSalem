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
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc,
            [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            var distance = float.MaxValue;
            PlayerControl player = pc.Object;

            couldUse = !player.Data.IsDead && (player.Data.IsImpostor || player.GetRole() is Spy);
            canUse = false;

            if ((DateTime.UtcNow - PlayerVentTimeUtility.GetLastVent(player.PlayerId)).TotalMilliseconds > 100F)
            {
                distance = Vector2.Distance(player.GetTruePosition(), __instance.transform.position);
                canUse = couldUse & distance <= __instance.UsableDistance;
            }

            __result = distance;
            return false;
        }
    }
}