using System;
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
        public static void Postfix(PlayerControl __instance, GameData.PlayerInfo PAIBDFDMIGK)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || DeadPlayers.Count <= 0) return;

            if (!(PlayerControl.LocalPlayer.GetRole() is Sheriff sheriff)) return;

            DeadPlayer deadPlayer =
                DeadPlayers.FirstOrDefault(x => PAIBDFDMIGK != null && x.Victim?.PlayerId == PAIBDFDMIGK.PlayerId);
            if (deadPlayer == null) return;

            if (__instance.PlayerId != sheriff.Owner.PlayerId) return;

            string reportMsg = deadPlayer.ParseBodyReport();

            if (string.IsNullOrWhiteSpace(reportMsg)) return;

            if (AmongUsClient.Instance.AmClient && HudManager.Instance)
            {
                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, reportMsg);
            }

            if (reportMsg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Telemetry.Instance.SendWho();
            }
        }
    }
}