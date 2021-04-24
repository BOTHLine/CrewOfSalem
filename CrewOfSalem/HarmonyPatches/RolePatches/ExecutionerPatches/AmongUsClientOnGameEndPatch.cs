using System.Linq;
using CrewOfSalem.Roles;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static void Prefix([HarmonyArgument(0)] GameOverReason gameOverReason)
        {
            if (!TryGetSpecialRole(out Executioner executioner)) return;
            
            WinningPlayerData executionerWinner;
            if (executioner.Owner.Data.IsImpostor)
            {
                executionerWinner = new WinningPlayerData(executioner.Owner.Data);
                TempData.winners = new List<WinningPlayerData>(0);
                TempData.winners.Add(executionerWinner);
            } else if (TempData.DidHumansWin(gameOverReason))
            {
                executioner.Owner.Data.IsImpostor = true;
                executionerWinner = TempData.winners.ToArray()
                   .FirstOrDefault(winner => winner.Name.Equals(executioner.Owner.Data.PlayerName));
                TempData.winners.Remove(executionerWinner);
            }
        }
    }
}