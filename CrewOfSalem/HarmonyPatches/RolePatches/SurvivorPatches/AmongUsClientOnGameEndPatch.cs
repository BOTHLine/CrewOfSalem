using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.SurvivorPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static bool Prefix([HarmonyArgument(0)] GameOverReason gameOverReason)
        {
            if (!TryGetSpecialRole(out Survivor survivor)) return true;

            if (TempData.DidHumansWin(gameOverReason))
            {
                survivor.Owner.Data.IsImpostor = survivor.Owner.Data.IsDead;
            } else if (gameOverReason != GameOverReason.ImpostorBySabotage)
            {
                survivor.Owner.Data.IsImpostor = !survivor.Owner.Data.IsDead;
            } else
            {
                survivor.Owner.Data.IsImpostor = false;
            }

            return true;
        }
    }
}