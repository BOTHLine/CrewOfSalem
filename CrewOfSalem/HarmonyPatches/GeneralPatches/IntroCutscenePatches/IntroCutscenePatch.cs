using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
    public static class IntroCutscenePatch
    {
        public static void Prefix(IntroCutscene.Nested_0 __instance)
        {
            LocalRole.SetIntro(__instance);
            __instance.__this.ImpostorText.gameObject.SetActive(true);
        }
    }
}