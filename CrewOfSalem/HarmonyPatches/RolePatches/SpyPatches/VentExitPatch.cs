using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SpyPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            if (!(PlayerControl.LocalPlayer.GetRole() is Spy spy)) return;

            PlayerVentTimeUtility.SetLastVent(PlayerControl.LocalPlayer.PlayerId);
        }
    }
}