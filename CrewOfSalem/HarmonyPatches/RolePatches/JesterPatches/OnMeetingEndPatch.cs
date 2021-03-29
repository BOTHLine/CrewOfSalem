using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.JesterPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class OnMeetingEndPatch
    {
        public static bool Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return true;

            if (TryGetSpecialRole(out Jester jester) &&
                jester.Owner.PlayerId == ExileController.Instance.exiled?.PlayerId)
            {
                jester.Win();
            }

            return true;
        }
    }
}