using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.IntroCutscenePatches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    public static class BeginImpostorPatch
    {
        public static void Postfix(IntroCutscene __instance)
        {
            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role _))
            {
                __instance.ImpostorText.gameObject.SetActive(true);
            }
        }
    }
}