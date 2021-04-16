using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.GuardianAngelPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class OnExileEndPatch
    {
        public static bool Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return true;
            if (TryGetSpecialRole(out GuardianAngel guardianAngel) && guardianAngel.Owner == LocalPlayer &&
                !guardianAngel.Owner.Data.IsDead)
            {
                if (guardianAngel.ProtectTarget.Data.IsDead)
                {
                    guardianAngel.TurnIntoSurvivor();
                }
            }

            return true;
        }
    }
}