using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    public static class BeginImpostorPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            __instance.ImpostorText.gameObject.SetActive(true);
        }
    }
}