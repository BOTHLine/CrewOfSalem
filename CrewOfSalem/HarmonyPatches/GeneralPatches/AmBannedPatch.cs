namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    // [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned))]
    public static class AmBannedPatch
    {
        public static bool Prefix(out bool __result)
        {
            __result = false;
            return false;
        }
    }
}