using System.Collections.Generic;
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
        [HarmonyPriority(Priority.First)]
        public static void Postfix()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                IReadOnlyList<Ability> abilities = player.GetAbilities();
                foreach (Ability ability in abilities)
                {
                    ability.MeetingStart();
                }
            }

            SetMeetingHudRoleName();
            SetMeetingHudNameColor();

            if (MeetingRoomManager.Instance.reporter == null) return;
            
            if (TryGetSpecialRole(out Psychic psychic)) psychic.StartMeeting(MeetingRoomManager.Instance.target);

            if (Main.OptionDisableSkipOnButton && MeetingRoomManager.Instance.target == null)
            {
                MeetingHud.Instance.SkipVoteButton.gameObject.SetActive(false);
            }
        }

        private static void SetMeetingHudRoleName()
        {
            if (MeetingHud.Instance == null) return;
            Role localRole = LocalRole;
            if (localRole == null) return;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                playerVoteArea.NameText.autoSizeTextContainer = false;
                playerVoteArea.NameText.enableAutoSizing = false;
                playerVoteArea.NameText.fontSize = 1.5F;

                if (localRole.Owner.PlayerId == playerVoteArea.TargetPlayerId)
                {
                    playerVoteArea.NameText.text = $"{localRole.Owner.Data.PlayerName} {localRole.Name}";
                }
            }
        }

        private static void SetMeetingHudNameColor()
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