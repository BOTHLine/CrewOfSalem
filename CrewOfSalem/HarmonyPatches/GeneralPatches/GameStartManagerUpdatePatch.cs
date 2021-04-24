using System;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
    public static class GameStartManagerUpdatePatch
    {
        private static DateTime? lobbyCreationTime;

        public static void Postfix(GameStartManager __instance)
        {
            lobbyCreationTime ??= DateTime.UtcNow;

            TimeSpan time = new TimeSpan(0, 10, 0) - (DateTime.UtcNow - lobbyCreationTime.Value);

            var playerCount = $"{__instance.LastPlayerCount}/{PlayerControl.GameOptions.MaxPlayers}";
            var lobbyTime = $"{time.Minutes:00}:{time.Seconds:00}";

            __instance.PlayerCounter.text = $"{playerCount}\n{lobbyTime}";
        }
    }
}