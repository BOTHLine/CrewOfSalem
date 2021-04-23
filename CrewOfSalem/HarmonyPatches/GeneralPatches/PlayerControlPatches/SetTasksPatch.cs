using CrewOfSalem.Extensions;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    // [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    // [HarmonyPatch(typeof(PlayerControl._CoSetTasks_d__78), nameof(PlayerControl._CoSetTasks_d__78.MoveNext))]
    // [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl))]
    // TODO:
    public static class SetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (ShipStatus.Instance == null) return;
            if (__instance.Data == null) return;

            __instance.GetRole()?.SetRoleTask();
        }
    }
}