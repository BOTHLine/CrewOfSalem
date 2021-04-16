using System.Linq;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

/*
using static CrowdedMod.Patches.CreateGameOptionsPatches.CreateOptionsPickerStart;

using PlayerControl = FFGALNAPKCD;
using PlayerTab = MAOILGPNFND;
using GameData = EGLJNOMOGNP;
using Palette = LOCPGOACAJF;
using PingTracker = ELDIDNABIPI;
using ShipStatus = HLBNNHFCNAJ;
using GameSettingMenu = JCLABFFHPEO;
using GameOptionsMenu = PHCKLDDNJNP;
using NumberOption = PCGDGFIAJJI;
*/

namespace CrowdedMod.Patches
{
    internal static class GenericPatches
    {
        // patched because 10 is hardcoded in `for` loop
        //[HarmonyPatch(typeof(GameData), nameof(GameData.GetAvailableId))]
        private static class GameDataAvailableIdPatch
        {
            public static bool Prefix(ref GameData __instance, out sbyte __result)
            {
                for (sbyte i = 0; i <= CreateGameOptionsPatches.CreateOptionsPickerStart.MAXPlayers; i++)
                {
                    if (!CheckId(__instance, i)) continue;

                    __result = i;
                    return false;
                }

                __result = -1;
                return false;
            }

            private static bool CheckId(GameData __instance, int playerId)
            {
                foreach (GameData.PlayerInfo player in __instance.AllPlayers)
                {
                    if (player.PlayerId == playerId) return false;
                }

                return true;
            }
        }

        //[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor), typeof(byte))]
        private static class PlayerControlCheckColorPatch
        {
            public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte colorId)
            {
                __instance.RpcSetColor(colorId);
                return false;
            }
        }

        //[HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.UpdateAvailableColors))]
        private static class PlayerTabUpdateAvailableColorsPatch
        {
            public static bool Prefix(PlayerTab __instance)
            {
                PlayerControl.SetPlayerMaterialColors(LocalData.ColorId, __instance.DemoImage);
                for (var i = 0; i < Palette.PlayerColors.Length; i++)
                {
                    __instance.AvailableColors.Add(i);
                }

                return false;
            }
        }

        /*
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.GetSpawnLocation))]
            public static class ShipStatusGetSpawnLocationPatch
        {
            public static void Prefix(ShipStatus __instance, [HarmonyArgument(0)] ref int playerId, [HarmonyArgument(1)] ref int numPlayer)
            {
            playerId %= 10;
            if (numPlayer > 10) numPlayer = 10;
            }
        }
        */

        //[HarmonyPriority(Priority.VeryHigh)] // to show this message first, or be overrided if any plugins do
        //[HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static class PingShowerPatch
        {
            public static void Postfix(PingTracker __instance)
            {
                __instance.text.Text += "\n[FFB793FF]> CrowdedMod <";
            }
        }

        // Map/Impostor Change in Online Lobby
        [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.OnEnable))]
        private static class GameSettingMenuOnEnable
        {
            private static void Prefix(ref GameSettingMenu __instance)
            {
                __instance.HideForOnline = new Il2CppReferenceArray<Transform>(1) {[0] = __instance.HideForOnline[0]};
            }
        }

        //[HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        private static class GameOptionsMenuStart
        {
            private static void Postfix(ref GameOptionsMenu __instance)
            {
                __instance.GetComponentsInChildren<NumberOption>()
                   .First(o => o.Title == StringNames.GameNumImpostors)
                   .ValidRange = new FloatRange(1,
                    (int) (CreateGameOptionsPatches.CreateOptionsPickerStart.MAXPlayers - 0.5f) / 2);
            }
        }
    }
}