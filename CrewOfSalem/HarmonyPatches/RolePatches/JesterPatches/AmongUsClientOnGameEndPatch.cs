using CrewOfSalem.Roles;
using HarmonyLib;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using static CrewOfSalem.CrewOfSalem;


namespace CrewOfSalem.HarmonyPatches.JesterPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static void Prefix([HarmonyArgument(0)] GameOverReason gameOverReason)
        {
            if (!TryGetSpecialRole(out Jester jester)) return;

            WinningPlayerData jesterWinner;
            if (jester.Owner.Data.IsImpostor)
            {
                jesterWinner = new WinningPlayerData(jester.Owner.Data);
                TempData.winners = new List<WinningPlayerData>(0);
                TempData.winners.Add(jesterWinner);
            } else if (TempData.DidHumansWin(gameOverReason))
            {
                jester.Owner.Data.IsImpostor = true;
                jesterWinner = TempData.winners.ToArray()
                   .FirstOrDefault(winner => winner.Name.Equals(jester.Owner.Data.PlayerName));
                TempData.winners.Remove(jesterWinner);
            }
        }
    }
}