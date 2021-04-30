using System.Collections.Generic;
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
    // TODO: Performance?
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePlayerVisualsPatch
    {
        private static readonly List<byte> GreyPlayerIds = new List<byte>();

        public static void Postfix()
        {
            UpdatePlayerVisuals();
            UpdatePlayerNameColors(); // TODO: Get out of Update? Use something like Awake/Start instead?
        }

        // TODO: Currently working with PhysicsHelpers.AnyNonTriggersBetween, change to something like "Vision" later?
        private static void UpdatePlayerVisuals()
        {
            if (ShipStatus.Instance == null) return;

            GreyPlayerIds.Clear();

            AbilitySeance[] seanceAbilities = Ability.GetAllAbilities<AbilitySeance>();
            if (seanceAbilities.Any(seance => seance.owner.Owner == LocalPlayer && seance.HasDurationLeft))
            {
                TurnAllPlayersGrey();
                return;
            }

            AbilityDisguise[] disguiseAbilities = Ability.GetAllAbilities<AbilityDisguise>();
            foreach (AbilityDisguise disguise in disguiseAbilities)
            {
                if (!disguise.HasDurationLeft) continue;
                
                foreach (PlayerControl player in AllPlayers)
                {
                    if (GreyPlayerIds.Contains(player.PlayerId)) continue;
                    if (!disguise.IsPlayerInRange(player)) continue;

                    GreyPlayerIds.Add(player.PlayerId);
                    player.TurnGrey();
                }
            }


            AbilityForge[] forgeAbilities = Ability.GetAllAbilities<AbilityForge>();
            AbilityHypnotize[] hypnotizeAbilities = Ability.GetAllAbilities<AbilityHypnotize>();

            foreach (PlayerControl player in AllPlayers)
            {
                if (GreyPlayerIds.Contains(player.PlayerId)) continue;
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

                // TODO: Check if visuals to set match the current visuals, then skip
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