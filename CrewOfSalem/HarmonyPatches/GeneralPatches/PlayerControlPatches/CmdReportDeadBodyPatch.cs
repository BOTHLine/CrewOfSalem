using CrewOfSalem.Extensions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    public static class CmdReportDeadBodyPatch
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            __instance.RpcStartMeetingCustom(target);
        }
    }
}