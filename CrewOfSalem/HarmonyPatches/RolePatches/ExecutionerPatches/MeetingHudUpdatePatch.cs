using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingHudUpdatePatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out Executioner executioner) ||
                executioner.Owner != PlayerControl.LocalPlayer) return;

            if (MeetingHud.Instance == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (executioner.VoteTarget.PlayerId != playerVoteArea.TargetPlayerId) continue;

                playerVoteArea.NameText.Text = $"{executioner.VoteTarget.Data.PlayerName}\n(Target)";
                playerVoteArea.NameText.Color = executioner.VoteTarget.GetRole().Color;
                playerVoteArea.NameText.scale = 0.8F;
            }
        }
    }
}