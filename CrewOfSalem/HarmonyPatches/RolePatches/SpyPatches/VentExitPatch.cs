using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SpyPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            if (!(LocalRole is Spy spy)) return;

            PlayerVentTimeUtility.SetLastVent(LocalPlayer.PlayerId);
        }
    }
}