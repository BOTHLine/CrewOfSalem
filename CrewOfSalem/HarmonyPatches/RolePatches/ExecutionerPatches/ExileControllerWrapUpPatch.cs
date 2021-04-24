using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ExileControllerWrapUpPatch
    {
        public static void Postfix()
        {
            if (TryGetSpecialRole(out Executioner executioner))
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

                if (executioner.VoteTarget.PlayerId == ExileController.Instance.exiled?.PlayerId)
                {
                    executioner.Owner.WinSolo();
                } else if (executioner.VoteTarget.Data.IsDead && !executioner.Owner.Data.IsDead)
                {
                    executioner.TurnIntoJester();
                }
            }
        }
    }
}