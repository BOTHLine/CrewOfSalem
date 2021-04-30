using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    public static class CmdReportDeadBodyPatch
    {
        public static void Postfix([HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            if (TryGetSpecialRole(out Psychic psychic)) psychic.StartMeeting(target);

            if (Main.OptionDisableSkipOnButton && target == null)
            {
                MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(false);
            }
        }
    }
}