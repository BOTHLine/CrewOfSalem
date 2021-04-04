using CrewOfSalem.Extensions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    // TODO: 2021.3.5s
    [HarmonyPatch(typeof(IntroCutscene._CoBegin_d__11), nameof(IntroCutscene._CoBegin_d__11.MoveNext))]
    public static class MoveNextPatch
    {
        public static bool Prefix(IntroCutscene._CoBegin_d__11 __instance)
        {
            PlayerControl.LocalPlayer.GetRole().SetIntro(__instance);
            return true;
        }
    }
}