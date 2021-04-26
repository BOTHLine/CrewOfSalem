using System;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public static class GameStartManagerUpdatePatch
    {
        private static DateTime? lobbyCreationTime;

        public static byte[] TimeBytes
        {
            get => lobbyCreationTime != null ? BitConverter.GetBytes(lobbyCreationTime.Value.Ticks) : new byte[0];
            set => lobbyCreationTime = new DateTime(BitConverter.ToInt64(value));
        }

        public static void Postfix(GameStartManager __instance)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                lobbyCreationTime ??= DateTime.UtcNow;
            } else if (lobbyCreationTime == null && AmongUsClient.Instance != null && LocalPlayer != null)
            {
                WriteRPC(RPC.RequestSyncLobbyTime);
            }

            if (lobbyCreationTime == null) return;

            TimeSpan time = new TimeSpan(0, 10, 0) - (DateTime.UtcNow - lobbyCreationTime.Value);

            var playerCount = $"{__instance.LastPlayerCount}/{PlayerControl.GameOptions.MaxPlayers}";
            var lobbyTime = $"{time.Minutes:00}:{time.Seconds:00}";

            __instance.PlayerCounter.text = $"{playerCount}\n{lobbyTime}";
        }
    }
}