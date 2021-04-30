using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateButtons))]
    public static class MeetingHudPopulateButtonsPatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out Executioner executioner) || executioner.Owner != LocalPlayer) return;

            if (MeetingHud.Instance == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (executioner.VoteTarget.PlayerId != playerVoteArea.TargetPlayerId) continue;

                playerVoteArea.NameText.autoSizeTextContainer = false;
                playerVoteArea.NameText.enableAutoSizing = false;
                
                playerVoteArea.NameText.text = $"{executioner.VoteTarget.Data.PlayerName}\nTarget";
                playerVoteArea.NameText.color = executioner.VoteTarget.GetRole().Color;
            }
        }
    }
}