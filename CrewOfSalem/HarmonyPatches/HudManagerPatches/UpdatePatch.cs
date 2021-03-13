using CrewOfSalem.HarmonyPatches.KillButtonManagerPatches;
using CrewOfSalem.Roles;
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
            PlayerControl localPlayer = PlayerControl.LocalPlayer;

            if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started) return;
            if (DefaultKillButton == null) DefaultKillButton = __instance.KillButton.renderer.sprite;
            if (localPlayer.Data.IsImpostor)
            {
                __instance.KillButton.gameObject.SetActive(true);
                __instance.KillButton.renderer.enabled = true;
                __instance.KillButton.renderer.sprite = DefaultKillButton;
            }

            bool lastQ = Input.GetKeyUp(KeyCode.Q);
            if (!localPlayer.Data.IsImpostor && Input.GetKeyDown(KeyCode.Q) && !lastQ && __instance.UseButton.isActiveAndEnabled)
            {
                PerformKillPatch.Prefix(null);
            }

            bool sabotageActive = false;
            foreach(PlayerTask task in localPlayer.myTasks)
            {
                sabotageActive = task.TaskType switch
                {
                    TaskTypes.FixLights => true,
                    TaskTypes.RestoreOxy => true,
                    TaskTypes.ResetReactor => true,
                    TaskTypes.ResetSeismic => true,
                    TaskTypes.FixComms => true,
                    _ => sabotageActive
                };
            }

            foreach(PlayerControl player in PlayerControl.AllPlayerControls)
            {
                player.nameText.Color = Color.white;
            }

            // Jester Tasks have to be reset every frame.. maybe rework later?
            if (SpecialRoleIsAssigned<Jester>(out var jesterKvp))
            {
                jesterKvp.Value.ClearTasks();
            }

            if (SpecialRoleIsAssigned<Doctor>(out var doctorKvp))
            {
                doctorKvp.Value.CheckShowShieldedPlayer();
            }

            bool jesterCanSeeImpostor = false;

            if (TryGetSpecialRoleByPlayer(localPlayer.PlayerId, out Role current))
            {
                current.SetNameColor();
                current.CheckDead(__instance);
                current.CheckSpecialButton(__instance);
                current.UpdateCooldown(Time.deltaTime);
                switch(current)
                {
                    case Investigator investigator:
                      //  investigator.CheckInvestigateButton();
                      //  investigator.SetCooldown(Time.deltaTime);
                        break;
                    // TODO: Add all Roles
                    case Jester jester:
                        jesterCanSeeImpostor = jester.canSeeImpostor;
                        break;
                }
            }

            if (current is Jester && jesterCanSeeImpostor)
            {
                foreach(PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    player.nameText.Color = TryGetSpecialRoleByPlayer(player.PlayerId, out Role role) ? role.Color : player.nameText.Color;

                    if (MeetingHud.Instance == null) continue;

                    foreach(PlayerVoteArea playerVote in MeetingHud.Instance.playerStates)
                    {
                        if (player.PlayerId == playerVote.TargetPlayerId)
                        {
                            playerVote.NameText.Color = player.nameText.Color;
                        }
                    }
                }
            }
            else if (localPlayer.Data.IsImpostor)
            {
                foreach(PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (!player.Data.IsImpostor) continue;

                    player.nameText.Color = TryGetSpecialRoleByPlayer(player.PlayerId, out Role role) ? role.Color : Palette.ImpostorRed;

                    if (MeetingHud.Instance == null) continue;

                    foreach(PlayerVoteArea playerVote in MeetingHud.Instance.playerStates)
                    {
                        if (player.PlayerId == playerVote.TargetPlayerId)
                        {
                            playerVote.NameText.Color = player.nameText.Color;
                        }
                    }
                }
            }
        }
    }
}
