using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.JesterPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class PlayerControlSetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (__instance.GetRole() is Jester jester)
            {
                jester.ClearTasks();
            }
        }
    }
}