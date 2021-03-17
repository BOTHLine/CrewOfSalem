using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.VentPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            PlayerVentTimeUtility.SetLastVent(PlayerControl.LocalPlayer.PlayerId);
            
            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Tracker tracker)) {
                tracker.SendChatMessage(Tracker.MessageType.PlayerExitedVent);
            }
        }
    }
}