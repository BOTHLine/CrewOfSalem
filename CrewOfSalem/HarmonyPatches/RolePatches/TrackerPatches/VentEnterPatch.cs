using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_38))]
    public static class VentEnterPatch
    {
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerEnteredVent);
            }
        }
    }
}