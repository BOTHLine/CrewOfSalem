using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.TrackerPatches
{
    // TODO: KillPlayer?
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class PlayerControlMurderPlayerPatch
    {
        public static void Postfix([HarmonyArgument(0)] PlayerControl target)
        {
            if (LocalRole is Tracker tracker && target != tracker.Owner)
                tracker.SendChatMessage(Tracker.MessageType.PlayerDied);
        }
    }
}