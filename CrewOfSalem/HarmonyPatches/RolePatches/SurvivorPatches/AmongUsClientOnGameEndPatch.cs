using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.SurvivorPatches
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public static class AmongUsClientOnGameEndPatch
    {
        public static bool Prefix(GameOverReason OFLKLGMHBEL)
        {
            if (TryGetSpecialRole(out Survivor survivor))
            {
                if (TempData.DidHumansWin(OFLKLGMHBEL))
                {
                    survivor.Owner.Data.IsImpostor = survivor.Owner.Data.IsDead;
                } else if (OFLKLGMHBEL != GameOverReason.ImpostorBySabotage)
                {
                    survivor.Owner.Data.IsImpostor = !survivor.Owner.Data.IsDead;
                } else
                {
                    survivor.Owner.Data.IsImpostor = false;
                }
            }

            return true;
        }
    }
}