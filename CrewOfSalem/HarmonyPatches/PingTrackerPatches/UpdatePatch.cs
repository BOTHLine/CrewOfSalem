using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.PingTrackerPatches
{
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class UpdatePatch
    {
        public static void Postfix(PingTracker __instance)
        {
            __instance.text.Text += $"\n{Main.Name} {Main.Version}";
        }
    }
}