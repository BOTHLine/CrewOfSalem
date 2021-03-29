using System;
using CrewOfSalem.Roles;
using HarmonyLib;
using System.Linq;
using Assets.CoreScripts;
using CrewOfSalem.Extensions;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.CmdReportDeadBody))]
    public static class CmdReportDeadBodyPatch
    {
        public static void Postfix(PlayerControl __instance, GameData.PlayerInfo PAIBDFDMIGK)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || DeadPlayers.Count <= 0) return;

            Role localRole = PlayerControl.LocalPlayer.GetRole();
            if (localRole is Psychic psychic)
            {
                psychic.StartMeeting();
            }

            DeadPlayer deadPlayer =
                DeadPlayers.FirstOrDefault(x => PAIBDFDMIGK != null && x.Victim?.PlayerId == PAIBDFDMIGK.PlayerId);
            if (deadPlayer == null) return;

            if (!(localRole is Sheriff sheriff)) return;

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