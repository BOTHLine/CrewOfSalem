using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches.MeetingHudPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateButtons))]
    public static class MeetingHudPopulateButtons
    {
        public static void Postfix()
        {
            SetMeetingHudRoleName();
            SetMeetingHudNameColor();
        }

        public static void SetMeetingHudRoleName()
        {
            if (MeetingHud.Instance == null) return;
            Role localRole = LocalRole;
            if (localRole == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (localRole.Owner.PlayerId != playerVoteArea.TargetPlayerId) continue;

                playerVoteArea.NameText.text = $"{localRole.Owner.Data.PlayerName} ({localRole.Name})";
            }
        }

        public static void SetMeetingHudNameColor()
        {
            if (MeetingHud.Instance == null) return;
            Role localRole = LocalRole;
            if (localRole == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (localRole.Owner.PlayerId == playerVoteArea.TargetPlayerId)
                {
                    playerVoteArea.NameText.color = localRole.Color;
                } else
                {
                    Role role = playerVoteArea.TargetPlayerId.ToPlayerControl().GetRole();

                    if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
                    {
                        playerVoteArea.NameText.color = Faction.Mafia.Color;
                    } else if (LocalRole?.Faction == Faction.Coven && role?.Faction == Faction.Coven)
                    {
                        playerVoteArea.NameText.color = Faction.Coven.Color;
                    }
                }
            }
        }
    }
}