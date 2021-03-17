using CrewOfSalem.Roles;
using HarmonyLib;
using System;
using System.Linq;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.CmdReportDeadBody))]
    public static class CmdReportDeadBodyPatch
    {
        public static void Postfix(PlayerControl __instance, GameData.PlayerInfo PAIBDFDMIGK)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || DeadPlayers.Count <= 0) return;

            DeadPlayer deadPlayer = DeadPlayers.FirstOrDefault(x => x.Victim?.PlayerId == PAIBDFDMIGK.PlayerId);
            if (deadPlayer == null) return;

            if (!TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Sheriff _)) return;

            Sheriff sheriff = GetSpecialRole<Sheriff>(PlayerControl.LocalPlayer.PlayerId);
            if (sheriff == null) return;

            if (__instance.PlayerId != sheriff.Player.PlayerId) return;

            BodyReport bodyReport = new BodyReport(deadPlayer, deadPlayer.Killer,
                (float) (DateTime.UtcNow - deadPlayer.KillTime).TotalMilliseconds);

            string reportMsg = bodyReport.ParseBodyReport();

            if (string.IsNullOrWhiteSpace(reportMsg)) return;

            if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance) {
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, reportMsg);
            }

            /* TODO:
            if (reportMsg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                DestroyableSingleton<Telemetry>.Instance.SendWho();
            }
            */
        }
    }
}