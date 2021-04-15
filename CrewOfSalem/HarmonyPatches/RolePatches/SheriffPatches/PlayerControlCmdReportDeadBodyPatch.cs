using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreScripts;
using CrewOfSalem.Extensions;
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
            if (__instance == null || PlayerControl.LocalPlayer == null || DeadPlayers.Count <= 0) return;

            if (!(PlayerControl.LocalPlayer.GetRole() is Sheriff sheriff)) return;

            DeadPlayer deadPlayer =
                DeadPlayers.FirstOrDefault(x => target != null && x.Victim?.PlayerId == target.PlayerId);
            if (deadPlayer == null) return;

            if (__instance.PlayerId != sheriff.Owner.PlayerId) return;

            List<Func<DeadPlayer, string>> possibleHints = DeadPlayer.Hints.ToList();
            var hintAmount = (int) (Main.OptionSheriffMaxHintAmount.GetValue() -
                                    (int) (deadPlayer.KillAge / 1000F /
                                           Main.OptionSheriffHintDecreaseInterval.GetValue()));

            if (hintAmount < Main.OptionSheriffMinHintAmount.GetValue())
                hintAmount = (int) Main.OptionSheriffMinHintAmount.GetValue();

            for (var i = 0; i < hintAmount; i++)
            {
                Func<DeadPlayer, string> hint = possibleHints[Rng.Next(possibleHints.Count)];
                possibleHints.Remove(hint);
                string reportMsg = hint.Invoke(deadPlayer);

                if (string.IsNullOrWhiteSpace(reportMsg)) return;

                if (AmongUsClient.Instance.AmClient && HudManager.Instance)
                {
                    HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer,
                        $"{deadPlayer.Victim.Data.PlayerName}: {reportMsg}");
                }

                if (reportMsg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Telemetry.Instance.SendWho();
                }
            }
        }
    }
}