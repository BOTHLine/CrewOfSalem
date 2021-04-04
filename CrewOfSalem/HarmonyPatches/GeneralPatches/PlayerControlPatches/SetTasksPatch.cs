using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class SetTasksPatch
    {
        [HarmonyPriority(Priority.HigherThanNormal)]
        public static void Postfix(PlayerControl __instance)
        {
            Role role = __instance.GetRole();
            role.SetRoleTask();
        }
    }
}