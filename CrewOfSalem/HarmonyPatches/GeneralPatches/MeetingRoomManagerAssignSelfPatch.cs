using CrewOfSalem.Extensions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    // TODO: Check if MeetingRoomManager.Reporter and .Target are already assigned when MeetingHud.Awake etc. gets called.
    [HarmonyPatch(typeof(MeetingRoomManager), nameof(MeetingRoomManager.AssignSelf))]
    public static class MeetingRoomManagerAssignSelfPatch
    {
        public static void Postfix([HarmonyArgument(0)] PlayerControl reporter, [HarmonyArgument(1)] GameData.PlayerInfo target)
        {
            reporter.StartMeeting(target);
        }
    }
}