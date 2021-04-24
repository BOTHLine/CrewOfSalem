using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SpyPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            PlayerVentTimeUtility.SetLastVent(LocalPlayer.PlayerId);
        }
    }
}