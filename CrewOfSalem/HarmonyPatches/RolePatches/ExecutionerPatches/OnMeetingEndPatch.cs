using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExecutionerPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class OnMeetingEndPatch
    {
        public static bool Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return true;

            if (TryGetSpecialRole(out Executioner executioner))
            {
                if (executioner.VoteTarget.Data.IsDead)
                {
                    executioner.TurnIntoJester();
                } else if (executioner.VoteTarget.PlayerId == ExileController.Instance.exiled.PlayerId)
                {
                    executioner.Win();
                    return true;
                }
            }

            return true;
        }
    }
}