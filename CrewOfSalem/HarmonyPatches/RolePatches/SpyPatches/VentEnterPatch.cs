using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SpyPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
    public static class VentEnterPatch
    {
        public static void Postfix()
        {
            PlayerVentTimeUtility.SetLastVent(LocalPlayer.PlayerId);
        }
    }
}