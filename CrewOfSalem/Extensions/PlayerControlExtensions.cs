using System;
using System.Collections.Generic;
using CrewOfSalem.HarmonyPatches.GeneralPatches;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using UnhollowerBaseLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;
using Object = UnityEngine.Object;

// TODO: Add "GetName" which sets the name if in vision. Depends on Disguiser/Forger etc.

namespace CrewOfSalem.Extensions
{
    public static class PlayerControlExtensions
    {
        public static void RpcKillPlayer(this PlayerControl killer, PlayerControl target,
            PlayerControl killerAnimation = null)
        {
            if (killerAnimation == null) killerAnimation = killer;
            if (AmongUsClient.Instance.AmClient) killer.KillPlayer(target, killerAnimation);
            WriteRPC(RPC.Kill, killer.PlayerId, target.PlayerId, killerAnimation.PlayerId);
        }

        public static void KillPlayer(this PlayerControl killer, PlayerControl target, PlayerControl killerAnimation)
        {
            DeadPlayers.Add(new DeadPlayer(target, killerAnimation, DateTime.UtcNow));
            KillOverlayShowOnePatch.killerAnimation = killerAnimation.Data;
            killer.MurderPlayer(target);

            if (killerAnimation.GetRole().Faction != Faction.Mafia || LocalRole.Faction != Faction.Mafia ||
                killerAnimation == LocalPlayer) return;

            switch (Main.OptionMafiaSharedKillCooldown.GetValue())
            {
                case 0: // None
                    break;
                case 1: // Killer
                    LocalPlayer.GetAbility<AbilityKill>()
                      ?.SetCooldown(killerAnimation.GetAbility<AbilityKill>().Cooldown);
                    break;
                case 2: // Self
                    LocalPlayer.GetAbility<AbilityKill>()?.SetOnCooldown();
                    break;
                case 3: // Custom
                    float cooldown = Main.OptionMafiaCustomSharedKillCooldown.GetValue();
                    LocalPlayer.GetAbility<AbilityKill>()?.SetCooldown(cooldown);
                    break;
            }
        }

        public static void SetVisuals(this PlayerControl player, PlayerControl targetVisual)
        {
            // if (targetVisual.Data.ColorId == -1) return;
            // if (targetVisual.MyPhysics.Skin.skin == null) return;

            Il2CppArrayBase<SkinData> allSkins = HatManager.Instance.AllSkins.ToArray();
            // if (targetVisual.Data.SkinId >= allSkins.Length) return;

            Il2CppArrayBase<PetBehaviour> allPets = HatManager.Instance.AllPets.ToArray();
            // if (targetVisual.Data.PetId >= allPets.Length) return;

            player.nameText.text = targetVisual.Data.PlayerName;
            player.myRend.material.SetColor(ShaderBodyColor, targetVisual.GetPlayerColor());
            player.myRend.material.SetColor(ShaderBackColor, targetVisual.GetShadowColor());
            player.HatRenderer.SetHat(targetVisual.Data.HatId, targetVisual.Data.ColorId);
            player.nameText.transform.localPosition =
                new Vector3(0F, targetVisual.Data.HatId == 0U ? 0.7F : 1.05F, -0.5F);
            if (player.MyPhysics.Skin.skin.ProdId != allSkins[(int) targetVisual.Data.SkinId].ProdId)
            {
                SetSkinWithAnim(player.MyPhysics, targetVisual.Data.SkinId);
            }

            if (player.CurrentPet == null || player.CurrentPet.ProdId != allPets[(int) targetVisual.Data.PetId].ProdId)
            {
                if (player.CurrentPet) Object.Destroy(player.CurrentPet.gameObject);
                player.CurrentPet = Object.Instantiate(allPets[(int) targetVisual.Data.PetId]);
                player.CurrentPet.transform.position = player.transform.position;
                player.CurrentPet.Source = player;
                player.CurrentPet.Visible = player.Visible;
                PlayerControl.SetPlayerMaterialColors(targetVisual.Data.ColorId, player.CurrentPet.rend);
            } else if (player.CurrentPet != null)
            {
                PlayerControl.SetPlayerMaterialColors(targetVisual.Data.ColorId, player.CurrentPet.rend);
            }

            Role role = player.GetRole();
            Color color = Color.white;

            if (LocalRole?.Faction == Faction.Mafia && role?.Faction == Faction.Mafia)
            {
                color = role!.Color;
            } else if (LocalRole is Executioner executioner && executioner.VoteTarget == player)
            {
                color = role!.Color;
            }

            player.nameText.color = color;

            int showPlayerNames = Main.OptionShowPlayerNames.GetValue();
            if (showPlayerNames != 1) return;
            Vector2 fromPosition = LocalPlayer.GetTruePosition();

            if (player == LocalPlayer) return;

            Vector2 distanceVector = player.GetTruePosition() - fromPosition;
            float distance = distanceVector.magnitude;

            if (!LocalPlayer.Data.IsDead && PhysicsHelpers.AnyNonTriggersBetween(fromPosition,
                distanceVector.normalized, distance, Constants.ShipOnlyMask))
            {
                player.nameText.text = "";
            }
        }

        public static void RpcStartMeetingCustom(this PlayerControl player, GameData.PlayerInfo info)
        {
            if (AmongUsClient.Instance.AmClient) player.StartMeetingCustom(info);
            WriteRPC(RPC.StartMeetingCustom, info?.PlayerId ?? byte.MaxValue);
        }

        public static void StartMeetingCustom(this PlayerControl playerControl, GameData.PlayerInfo info)
        {
            foreach (PlayerControl player in AllPlayers)
            {
                IReadOnlyList<Ability> abilities = player.GetAbilities();
                foreach (Ability ability in abilities)
                {
                    switch (ability)
                    {
                        case AbilityDuration {HasDurationLeft: true} abilityDuration:
                            abilityDuration.EffectEnd();
                            break;
                        case AbilityShield abilityShield when abilityShield.ShieldedPlayer != null:
                            abilityShield.BreakShield();
                            break;
                    }
                }
            }

            if (LocalRole is Psychic psychic) psychic.StartMeeting(info);
        }

        public static void ClearTasksCustom(this PlayerControl playerControl, bool keepSabotageTasks = true)
        {
            for (int i = playerControl.myTasks.Count - 1; i >= 0; i--)
            {
                PlayerTask task = playerControl.myTasks.ToArray()[i];

                if (keepSabotageTasks && (task.TaskType == TaskTypes.FixComms ||
                                          task.TaskType == TaskTypes.ResetReactor ||
                                          task.TaskType == TaskTypes.ResetSeismic ||
                                          task.TaskType == TaskTypes.RestoreOxy)) continue;

                task.OnRemove();
                playerControl.RemoveTask(task);
                Object.Destroy(task.gameObject);
            }
        }

        public static Role GetRole(this PlayerControl playerControl)
        {
            return GetSpecialRoleByPlayer(playerControl);
        }

        public static IReadOnlyList<Ability> GetAbilities(this PlayerControl playerControl)
        {
            return playerControl.GetRole()?.GetAllAbilities();
        }

        public static T GetAbility<T>(this PlayerControl playerControl)
            where T : Ability
        {
            return playerControl.GetRole()?.GetAbility<T>();
        }

        public static void UseAbility<T>(this PlayerControl playerControl, PlayerControl target)
            where T : Ability
        {
            playerControl.GetAbility<T>()?.Use(target, out bool _);
        }

        public static void EndAbility<T>(this PlayerControl playerControl)
            where T : AbilityDuration
        {
            playerControl.GetAbility<T>().EffectEnd();
        }

        public static Color GetPlayerColor(this PlayerControl playerControl)
        {
            return Palette.PlayerColors[playerControl.Data.ColorId];
        }

        public static Color GetShadowColor(this PlayerControl playerControl)
        {
            return Palette.ShadowColors[playerControl.Data.ColorId];
        }

        public static void RpcSoloWin(this PlayerControl winner)
        {
            if (AmongUsClient.Instance.AmClient) WinSolo(winner);

            WriteRPC(RPC.SoloWin, winner.Data.PlayerId);
        }

        public static void WinSolo(this PlayerControl winner)
        {
            foreach (PlayerControl player in AllPlayers)
            {
                if (player == winner) continue;
                player.Die(DeathReason.Exile);
                player.Data.IsDead = true;
                player.Data.IsImpostor = false;
            }

            winner.Revive();
            winner.Data.IsDead = false;
            winner.Data.IsImpostor = true;

            TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>(0);
            TempData.winners.Add(new WinningPlayerData(winner.Data));
        }
    }
}