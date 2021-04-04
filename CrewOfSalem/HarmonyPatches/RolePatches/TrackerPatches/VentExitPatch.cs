using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerExitedVent);
            }
        }
    }
}