using CrewOfSalem.Roles;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.VentPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class CanUsePatch
    {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            float distance = float.MaxValue;
            PlayerControl localPlayer = pc.Object;
            if (SpecialRoleIsAssigned(out KeyValuePair<byte, Spy> spyKvp))
            {
                couldUse = (spyKvp.Value.Player.PlayerId == PlayerControl.LocalPlayer.PlayerId || localPlayer.Data.IsImpostor) && !localPlayer.Data.IsDead;
            }
            else
            {
                couldUse = localPlayer.Data.IsImpostor && !localPlayer.Data.IsDead;
            }
            canUse = couldUse;
            if ((DateTime.UtcNow - PlayerVentTimeUtility.GetLastVent(pc.Object.PlayerId)).TotalMilliseconds > 100F)
            {
                distance = Vector2.Distance(localPlayer.GetTruePosition(), __instance.transform.position);
                canUse &= distance <= __instance.UsableDistance;
            }
            __result = distance;
            return false;
        }
    }
}