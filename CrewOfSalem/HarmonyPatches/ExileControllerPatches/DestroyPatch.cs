using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.MeetingPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class DestroyPatch
    {
        public static void Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (!TryGetSpecialRole(out Jester jester)) return;
            if (ExileController.Instance.exiled?.PlayerId != jester.Player.PlayerId) return;

            WriteImmediately(RPC.JesterWin);

            jester.Win();
        }
    }
}