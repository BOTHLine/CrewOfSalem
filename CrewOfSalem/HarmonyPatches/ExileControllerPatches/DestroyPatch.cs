using CrewOfSalem.Roles;
using HarmonyLib;
using System;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.MeetingPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new Type[] { typeof(UnityEngine.Object) })]
    public static class DestroyPatch
    {
        public static void Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return;
            if (!SpecialRoleIsAssigned<Jester>(out var jesterKvp)) return;
            if (ExileController.Instance.exiled?.PlayerId != jesterKvp.Key) return;

            WriteImmediately(RPC.JesterWin);

            jesterKvp.Value.Win();
        }
    }
}