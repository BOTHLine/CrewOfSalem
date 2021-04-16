using System;
using System.Collections.Generic;
using CrewOfSalem.HarmonyPatches.GeneralPatches;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

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

        public static void KillPlayer(this PlayerControl killer, PlayerControl target,
            PlayerControl killerAnimation = null)
        {
            DeadPlayers.Add(new DeadPlayer(target, killerAnimation, DateTime.UtcNow));
            KillOverlayShowOnePatch.killerAnimation = killerAnimation?.Data;
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

        public static void RpcStartMeetingCustom(this PlayerControl playerControl, GameData.PlayerInfo info)
        {
            if (AmongUsClient.Instance.AmClient) playerControl.StartMeetingCustom(info);
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

        public static void ClearTasks(this PlayerControl playerControl, bool keepSabotageTasks = true)

        {
            for (int i = playerControl.myTasks.Count - 1; i >= 0; i--)
            {
                PlayerTask task = playerControl.myTasks[i];

                if (keepSabotageTasks && (task.TaskType == TaskTypes.FixComms ||
                                          task.TaskType == TaskTypes.ResetReactor ||
                                          task.TaskType == TaskTypes.ResetSeismic ||
                                          task.TaskType == TaskTypes.RestoreOxy)) continue;

                task.OnRemove();
                playerControl.RemoveTask(task);
                UnityEngine.Object.Destroy(task.gameObject);
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
            playerControl.GetAbility<T>()?.Use(target, out bool sendRpc);
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
    }
}