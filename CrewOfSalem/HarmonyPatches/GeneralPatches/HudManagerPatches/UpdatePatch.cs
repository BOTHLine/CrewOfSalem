using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.PlayerControlPatches;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.HudManagerPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            PlayerControl localPlayer = PlayerControl.LocalPlayer;

            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            Role localRole = localPlayer.GetRole();

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                Role otherRole = player.GetRole();
                if (localRole?.Faction == Faction.Mafia && otherRole?.Faction == Faction.Mafia)
                {
                    player.nameText.Color = Faction.Mafia.Color;
                } else if (localRole?.Faction == Faction.Coven && otherRole?.Faction == Faction.Coven)
                {
                    player.nameText.Color = Faction.Coven.Color;
                } else if (AbilityBite.IsVampire(localPlayer) && AbilityBite.IsVampire(player))
                {
                    player.nameText.Color = Vampire.GetColor();
                } else
                {
                    player.nameText.Color = Color.white;
                }
            }

            AbilityShield.CheckShowShieldedPlayers();

            localRole?.SetNameColor();
            if (!localPlayer.Data.IsDead) localRole?.UpdateAbilities(Time.deltaTime);

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
            Vector2 fromPosition = PlayerControl.LocalPlayer.GetTruePosition();

            PlayerControl[] allPlayers = PlayerControl.AllPlayerControls.ToArray();
            foreach (PlayerControl player in allPlayers)
            {
                Vector2 distanceVector = player.GetTruePosition() - fromPosition;
                float distance = distanceVector.magnitude;
                if (PhysicsHelpers.AnyNonTriggersBetween(fromPosition, distanceVector.normalized, distance,
                    Constants.ShipOnlyMask))
                {
                    player.nameText.enabled = false;
                } else
                {
                    player.nameText.enabled = player.Visible;
                }
            }
        }
    }
}