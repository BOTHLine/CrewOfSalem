using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.PlayerControlPatches;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.HudManagerPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            foreach (PlayerControl player in AllPlayers)
            {
                Role role = player.GetRole();
                if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
                {
                    player.nameText.Color = Faction.Mafia.Color;
                } else if (LocalRole?.Faction == Faction.Coven && role?.Faction == Faction.Coven)
                {
                    player.nameText.Color = Faction.Coven.Color;
                } else if (AbilityBite.IsVampire(LocalPlayer) && AbilityBite.IsVampire(player))
                {
                    player.nameText.Color = Vampire.GetColor();
                } else
                {
                    player.nameText.Color = Palette.White;
                }
            }

            AbilityShield.CheckShowShieldedPlayers();

            LocalRole?.SetMeetingHudNameColor();
            LocalRole?.SetMeetingHudRoleName();

            // Add Mafia / Coven / Lover Chat
            // if (role?.Faction == Faction.Mafia || role?.Faction == Faction.Coven || role is Investigator || role is Spy)
            if (MeetingHud.Instance == null && ExileController.Instance == null)
            {
                if (!__instance.Chat.isActiveAndEnabled)
                {
                    __instance.Chat.SetVisible(true);
                }
            }

            UpdatePlayerNames();
        }

        // TODO: Currently working with PhysicsHelpers.AnyNonTriggersBetween, change to something like "Vision" later?
        private static void UpdatePlayerNames()
        {
            IReadOnlyList<AbilityDisguise> disguiseAbilities = Ability.GetAllAbilities<AbilityDisguise>();
            if (disguiseAbilities != null)
            {
                if (disguiseAbilities.Any(ability => ability.HasDurationLeft)) return;
            }

            int showPlayerNames = Main.OptionShowPlayerNames.GetValue();
            if (showPlayerNames != 1) return;
            Vector2 fromPosition = LocalPlayer.GetTruePosition();

            foreach (PlayerControl player in AllPlayers)
            {
                if (player == LocalPlayer) continue;

                Vector2 distanceVector = player.GetTruePosition() - fromPosition;
                float distance = distanceVector.magnitude;
                if (PhysicsHelpers.AnyNonTriggersBetween(fromPosition, distanceVector.normalized, distance,
                    Constants.ShipOnlyMask))
                {
                    player.nameText.Text = "";
                } else
                {
                    if (!player.Visible) continue;
                    
                    Role role = player.GetRole();
                    string name = player.Data.PlayerName;
                    
                    if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
                    {
                        name = CrewOfSalem.ColorizedText(player.Data.PlayerName, role!.Color);
                    } else if (LocalRole is Executioner executioner && executioner.VoteTarget == player)
                    {
                        name = CrewOfSalem.ColorizedText(player.Data.PlayerName, Faction.Crew.Color);
                    }

                    player.nameText.Text = name;
                }
            }
        }
    }
}