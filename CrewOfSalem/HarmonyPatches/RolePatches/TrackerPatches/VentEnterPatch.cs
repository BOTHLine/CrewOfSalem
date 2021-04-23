using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
    public static class VentEnterPatch
    {
        public static void Postfix()
        {
            if (LocalRole is Tracker tracker)
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerEnteredVent);
            }
        }
    }
}