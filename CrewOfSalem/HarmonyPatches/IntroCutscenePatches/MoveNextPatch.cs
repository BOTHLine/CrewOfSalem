using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    [HarmonyPatch(typeof(IntroCutscene.CoBegin__d), nameof(IntroCutscene.CoBegin__d.MoveNext))]
    public static class MoveNextPatch
    {
        public static bool Prefix(IntroCutscene.CoBegin__d __instance)
        {
            GetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId)?.SetIntro(__instance);
            return true;
        }
    }
}