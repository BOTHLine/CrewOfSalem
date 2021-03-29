using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.VentPatches;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_38))]
    public static class VentEnterPatch
    {
        public static void Postfix()
        {
            PlayerVentTimeUtility.SetLastVent(PlayerControl.LocalPlayer.PlayerId);

            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerEnteredVent);
            }
        }
    }
}