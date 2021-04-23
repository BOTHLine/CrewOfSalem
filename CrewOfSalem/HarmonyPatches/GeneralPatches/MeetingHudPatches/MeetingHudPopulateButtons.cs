using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
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
                if (localRole.Owner.PlayerId == playerVoteArea.TargetPlayerId)
                {
                    playerVoteArea.NameText.text = $"{localRole.Owner.Data.PlayerName} ({localRole.Name})";
                } else if (playerVoteArea.TargetPlayerId.ToPlayerControl().GetRole() is Mayor {hasRevealed: true} mayor)
                {
                    playerVoteArea.NameText.text = $"{mayor.Owner.Data.PlayerName} ({mayor.Name})";
                }
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
                    if (role is Mayor {hasRevealed: true} mayor)
                    {
                        playerVoteArea.NameText.color = mayor.Color;
                    } else if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
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