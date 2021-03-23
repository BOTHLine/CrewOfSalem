using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ExileControllerPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class DestroyPatch
    {
        public static void Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;

            TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role);
            role?.SpecialButton?.OnMeetingEnds();

            if (TryGetSpecialRole(out Executioner executioner) &&
                executioner.VoteTarget.PlayerId == ExileController.Instance.exiled?.PlayerId)
            {
                WriteImmediately(RPC.ExecutionerWin);
                executioner.Win();
            } else if (role is Jester jester && jester.Player.PlayerId == ExileController.Instance.exiled?.PlayerId)
            {
                WriteImmediately(RPC.JesterWin);
                jester.Win();
            }
        }
    }
}