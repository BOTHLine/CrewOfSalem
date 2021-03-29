using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.VentPatches;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            PlayerVentTimeUtility.SetLastVent(PlayerControl.LocalPlayer.PlayerId);

            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerExitedVent);
            }
        }
    }
}