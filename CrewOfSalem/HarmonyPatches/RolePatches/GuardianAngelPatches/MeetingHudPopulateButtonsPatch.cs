using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.GuardianAngelPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateButtons))]
    public static class MeetingHudPopulateButtonsPatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out GuardianAngel guardianAngel) || guardianAngel.Owner != LocalPlayer) return;

            if (MeetingHud.Instance == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (guardianAngel.ProtectTarget.PlayerId != playerVoteArea.TargetPlayerId) continue;

                playerVoteArea.NameText.autoSizeTextContainer = false;
                playerVoteArea.NameText.enableAutoSizing = false;

                playerVoteArea.NameText.text = $"{guardianAngel.ProtectTarget.Data.PlayerName}\nTarget";
            }
        }
    }
}