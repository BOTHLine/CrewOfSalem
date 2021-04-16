using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class OnExileEndPatch
    {
        public static bool Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return true;
            if (TryGetSpecialRole(out Executioner executioner) && executioner.Owner == LocalPlayer)
            {
                /*
                if (!executioner.IsJester)
                {
                    if (executioner.VoteTarget.PlayerId == ExileController.Instance.exiled.PlayerId)
                    {
                        executioner.RpcWin();
                    } else if (executioner.VoteTarget.Data.IsDead && executioner.Owner == LocalPlayer)
                    {
                        executioner.RpcTurnIntoJester();
                    }
                } else
                {
                    if (executioner.Owner.PlayerId == ExileController.Instance.exiled.PlayerId)
                    {
                        executioner.RpcWin();
                    }
                }
                */

                if (executioner.VoteTarget.Data.IsDead && !executioner.Owner.Data.IsDead)
                {
                    executioner.TurnIntoJester();
                } else if (executioner.VoteTarget.PlayerId == ExileController.Instance.exiled?.PlayerId)
                {
                    executioner.Win();
                    return true;
                }
            }

            return true;
        }
    }
}