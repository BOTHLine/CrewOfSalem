using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.SurvivorPatches
{
    // [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class PlayerControlSetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (__instance.GetRole() is Survivor survivor)
            {
                survivor.Owner?.ClearTasksCustom();
            }
        }
    }
}