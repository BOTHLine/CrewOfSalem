using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    public static class CmdReportDeadBodyPatch
    {
        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            WriteRPC(RPC.StartMeetingCustom, target?.PlayerId ?? byte.MaxValue);
        }
    }
}