using System.Linq;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.GuardianAngelPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static bool Prefix([HarmonyArgument(0)] GameOverReason gameOverReason)
        {
            if (!TryGetSpecialRole(out GuardianAngel guardianAngel)) return true;

            if (TempData.DidHumansWin(gameOverReason))
            {
                guardianAngel.Owner.Data.IsImpostor = guardianAngel.ProtectTarget.Data.IsDead;
                if (!guardianAngel.ProtectTarget.Data.IsDead) return true;

                TempData.winners.Remove(TempData.winners.ToArray()
                   .FirstOrDefault(winner => winner.Name.Equals(guardianAngel.Owner.Data.PlayerName)));
            } else if (gameOverReason != GameOverReason.ImpostorBySabotage) // Impostor won without Sabotage
            {
                guardianAngel.Owner.Data.IsImpostor = !guardianAngel.ProtectTarget.Data.IsDead;
                if (guardianAngel.ProtectTarget.Data.IsDead) return true;

                TempData.winners.Add(new WinningPlayerData(guardianAngel.Owner.Data));
            }

            return true;
        }
    }
}