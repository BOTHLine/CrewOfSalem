﻿namespace CrewOfSalem.HarmonyPatches.GameSettingPatches
{
    // [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            __instance.GameSettings.scale = 0.45F;
        }
    }
}