using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    //[HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__78), nameof(PlayerControl._CoSetTasks_d__78.MoveNext))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class PlayerControlSetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (__instance.GetRole() is Executioner executioner)
            {
                executioner.Owner?.ClearTasksCustom();
            }
        }
    }
}