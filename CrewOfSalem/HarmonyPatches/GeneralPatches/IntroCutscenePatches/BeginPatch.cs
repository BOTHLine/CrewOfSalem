using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    //[HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class BeginPatch
    {
        public static bool Prefix(IntroCutscene.CoBegin__d __instance)
        {
            LocalRole.SetIntro(__instance);
            __instance.__this.ImpostorText.gameObject.SetActive(true);
            return true;
        }
    }
}