using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.VersionShowerPatches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class StartPatch
    {
        public static void Postfix(VersionShower __instance)
        {
            __instance.text.text += $"  -  {Main.Name} {Main.Version}  -  by BothLine";
        }
    }
}