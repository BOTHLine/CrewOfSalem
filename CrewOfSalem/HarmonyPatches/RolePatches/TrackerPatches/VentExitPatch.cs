using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_1))]
    public static class VentExitPatch
    {
        public static void Postfix()
        {
            if (LocalRole is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerExitedVent);
            }
        }
    }
}