using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    // TODO: KillPlayer?
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class PlayerControlMurderPlayerPatch
    {
        public static void Postfix([HarmonyArgument(0)] PlayerControl target)
        {
            if (PlayerControl.LocalPlayer.GetRole() is Tracker tracker && target != tracker.Owner)
                tracker.SendChatMessage(Tracker.MessageType.PlayerDied);
        }
    }
}