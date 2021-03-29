using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static void Postfix(PlayerControl PAIBDFDMIGK)
        {
            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker && PAIBDFDMIGK != tracker.Owner)
                tracker.SendChatMessage(Tracker.MessageType.PlayerDied);
        }
    }
}