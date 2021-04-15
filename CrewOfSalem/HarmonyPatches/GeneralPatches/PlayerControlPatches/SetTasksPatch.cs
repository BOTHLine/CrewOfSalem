using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    //[HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__78), nameof(PlayerControl._CoSetTasks_d__78.MoveNext))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class SetTasksPatch
    {
        [HarmonyPriority(Priority.LowerThanNormal)]
        public static void Postfix(PlayerControl __instance)
        {
            Role role = __instance.GetRole();
            role?.SetRoleTask();
        }
    }
}