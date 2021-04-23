using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    //[HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
    public static class BeginPatch
    {
        public static bool Prefix(IntroCutscene.Nested_0 __instance)
        {
            LocalRole.SetIntro(__instance);
            __instance.__this.ImpostorText.gameObject.SetActive(true);
            return true;
        }
    }
}