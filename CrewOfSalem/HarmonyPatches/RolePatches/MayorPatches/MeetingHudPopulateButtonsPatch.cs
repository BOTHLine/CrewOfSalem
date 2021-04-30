using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MayorPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateButtons))]
    public class MeetingHudPopulateButtonsPatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out Mayor _)) return;

            if (MeetingHud.Instance == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (!(playerVoteArea.TargetPlayerId.ToPlayerControl().GetRole() is Mayor mayor)) continue;
                if (!mayor.hasRevealed) return;

                playerVoteArea.NameText.autoSizeTextContainer = false;
                playerVoteArea.NameText.enableAutoSizing = false;

                playerVoteArea.NameText.text = $"{mayor.Owner.Data.PlayerName} {mayor.Name}";
                playerVoteArea.NameText.color = mayor.Color;
            }
        }
    }
}