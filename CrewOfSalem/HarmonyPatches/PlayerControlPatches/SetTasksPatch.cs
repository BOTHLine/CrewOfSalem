using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
    public static class SetTasksPatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (TryGetSpecialRoleByPlayer(__instance.PlayerId, out Role role))
            {
                role.SetRoleDescription();
            }
        }
    }
}