using System.Linq;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.SurvivorPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static void Prefix([HarmonyArgument(0)] GameOverReason gameOverReason)
        {
            if (!TryGetSpecialRole(out Survivor survivor)) return;

            if (TempData.DidHumansWin(gameOverReason))
            {
                survivor.Owner.Data.IsImpostor = survivor.Owner.Data.IsDead;
                if (!survivor.Owner.Data.IsDead) return;

                TempData.winners.Remove(TempData.winners.ToArray()
                   .FirstOrDefault(winner => winner.Name.Equals(survivor.Owner.Data.PlayerName)));
            } else if (gameOverReason != GameOverReason.ImpostorBySabotage) // Impostor won without Sabotage
            {
                survivor.Owner.Data.IsImpostor = !survivor.Owner.Data.IsDead;
                if (survivor.Owner.Data.IsDead) return;

                TempData.winners.Add(new WinningPlayerData(survivor.Owner.Data));
            }
        }
    }
}