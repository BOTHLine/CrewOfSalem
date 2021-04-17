using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.GuardianAngelPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingHudUpdatePatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out GuardianAngel guardianAngel) || guardianAngel.Owner != LocalPlayer) return;

            if (MeetingHud.Instance == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (guardianAngel.ProtectTarget.PlayerId != playerVoteArea.TargetPlayerId) continue;

                playerVoteArea.NameText.Text = $"{guardianAngel.ProtectTarget.Data.PlayerName}\n(Target)";
                playerVoteArea.NameText.scale = 0.8F;
            }
        }
    }
}