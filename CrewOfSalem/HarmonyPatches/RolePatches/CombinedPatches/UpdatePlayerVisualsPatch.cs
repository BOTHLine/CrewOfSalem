using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.PlayerControlPatches;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.CombinedPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePlayerVisualsPatch
    {
        public static void Postfix()
        {
            UpdatePlayerVisuals();
            UpdatePlayerNameColors();
        }

        // TODO: Currently working with PhysicsHelpers.AnyNonTriggersBetween, change to something like "Vision" later?
        private static void UpdatePlayerVisuals()
        {
            if (ShipStatus.Instance == null) return;

            AbilityDisguise[] disguiseAbilities = Ability.GetAllAbilities<AbilityDisguise>();
            if (disguiseAbilities.Any(disguise => disguise.HasDurationLeft))
            {
                TurnAllPlayersGrey();
                return;
            }

            AbilityForge[] forgeAbilities = Ability.GetAllAbilities<AbilityForge>();
            AbilityHypnotize[] hypnotizeAbilities = Ability.GetAllAbilities<AbilityHypnotize>();

            foreach (PlayerControl player in AllPlayers)
            {
                if (!player.Visible) continue;

                byte targetVisualId = player.PlayerId;
                foreach (AbilityForge forge in forgeAbilities)
                {
                    if (!forge.HasDurationLeft || forge.owner.Owner != player) continue;

                    targetVisualId = forge.currentSample.PlayerId;
                }

                foreach (AbilityHypnotize hypnotize in hypnotizeAbilities)
                {
                    if (LocalPlayer != hypnotize.HypnotizedPlayer) continue;

                    if (hypnotize.playerMappings.TryGetValue(targetVisualId, out byte newTargetVisualId))
                    {
                        targetVisualId = newTargetVisualId;
                    }
                }

                player.SetVisuals(targetVisualId.ToPlayerControl());
            }
        }

        private static void UpdatePlayerNameColors()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                Role role = player.GetRole();
                if (player == LocalPlayer)
                {
                    if (role != null)
                    {
                        player.nameText.color = role.Color;
                    }
                } else if (role is Mayor {hasRevealed: true} mayor)
                {
                    player.nameText.color = mayor.Color;
                } else if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
                {
                    player.nameText.color = Faction.Mafia.Color;
                } else if (LocalRole?.Faction == Faction.Coven && role?.Faction == Faction.Coven)
                {
                    player.nameText.color = Faction.Coven.Color;
                } else if (AbilityBite.IsVampire(LocalPlayer) && AbilityBite.IsVampire(player))
                {
                    player.nameText.color = Vampire.GetColor();
                } else
                {
                    player.nameText.color = Palette.White;
                }
            }
        }
    }
}