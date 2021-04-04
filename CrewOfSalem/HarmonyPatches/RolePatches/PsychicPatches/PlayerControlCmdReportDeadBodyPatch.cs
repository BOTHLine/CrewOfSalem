using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.PsychicPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    public static class PlayerControlCmdReportDeadBodyPatch
    {
        public static void Postfix()
        {
            Role role = PlayerControl.LocalPlayer.GetRole();
            if (role is Psychic psychic)
            {
                psychic.StartMeeting();
            }
        }
    }
}