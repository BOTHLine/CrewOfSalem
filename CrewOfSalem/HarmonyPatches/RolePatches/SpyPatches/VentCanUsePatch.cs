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
        public static bool Prefix(Vent __instance, ref float __result, GameData.PlayerInfo OMBNGHLFKPJ,
            out bool OFPIPGCNGAK, out bool KHBPLGBBIEC)
        {
            var distance = float.MaxValue;
            PlayerControl player = OMBNGHLFKPJ.Object;

            OFPIPGCNGAK = !player.Data.IsDead && (player.Data.IsImpostor || player.GetRole() is Spy);
            KHBPLGBBIEC = OFPIPGCNGAK;

            if ((DateTime.UtcNow - PlayerVentTimeUtility.GetLastVent(player.PlayerId)).TotalMilliseconds > 100F)
            {
                distance = Vector2.Distance(player.GetTruePosition(), __instance.transform.position);
                KHBPLGBBIEC &= distance <= __instance.UsableDistance;
            }

            __result = distance;
            return false;
        }
    }
}