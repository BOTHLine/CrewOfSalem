using CrewOfSalem.Extensions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPriority(Priority.HigherThanNormal)]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class SetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            __instance.GetRole().SetRoleTask();
        }
    }
}