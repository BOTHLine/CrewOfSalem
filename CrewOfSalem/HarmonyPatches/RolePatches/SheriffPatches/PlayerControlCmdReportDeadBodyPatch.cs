using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreScripts;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.SheriffPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    public static class PlayerControlCmdReportDeadBodyPatch
    {
        // TODO: Move Functionality to Sheriff-Class and only call from here
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo target)
        {
            if (__instance == null || LocalPlayer == null || DeadPlayers.Count <= 0) return;

            if (!(LocalRole is Sheriff sheriff)) return;

            DeadPlayer deadPlayer =
                DeadPlayers.FirstOrDefault(x => target != null && x.Victim?.PlayerId == target.PlayerId);
            if (deadPlayer == null) return;

            if (__instance.PlayerId != sheriff.Owner.PlayerId) return;

            List<string> hints = deadPlayer.hintMessages.ToList();

            var hintAmount = (int) (Main.OptionSheriffMaxHintAmount.GetValue() -
                                    (int) (deadPlayer.KillAge / 1000F /
                                           Main.OptionSheriffHintDecreaseInterval.GetValue()));

            if (hintAmount < Main.OptionSheriffMinHintAmount.GetValue())
                hintAmount = (int) Main.OptionSheriffMinHintAmount.GetValue();

            for (var i = 0; i < hintAmount; i++)
            {
                string hint = hints[Rng.Next(hints.Count)];

                if (string.IsNullOrWhiteSpace(hint)) return;

                if (AmongUsClient.Instance.AmClient && HudManager.Instance)
                {
                    HudManager.Instance.Chat.AddChat(LocalPlayer, $"{deadPlayer.Victim.Data.PlayerName}: {hint}");
                }

                if (hint.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Telemetry.Instance.SendWho();
                }
            }
        }
    }
}