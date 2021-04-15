using System;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(KillOverlay), nameof(KillOverlay.ShowOne),
        new Type[] {typeof(GameData.PlayerInfo), typeof(GameData.PlayerInfo)})]
    public static class KillOverlayShowOnePatch
    {
        public static GameData.PlayerInfo killerAnimation;

        public static bool Prefix([HarmonyArgument(0)] ref GameData.PlayerInfo killer)
        {
            killer = killerAnimation ?? killer;
            killerAnimation = null;
            return true;
        }
    }
}