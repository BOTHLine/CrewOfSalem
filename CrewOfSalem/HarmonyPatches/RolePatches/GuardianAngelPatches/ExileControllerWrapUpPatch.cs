using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.GuardianAngelPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ExileControllerWrapUpPatch
    {
        public static void Postfix()
        {
            if (!TryGetSpecialRole(out GuardianAngel guardianAngel) || guardianAngel.Owner != LocalPlayer ||
                guardianAngel.Owner.Data.IsDead) return;

            if (guardianAngel.ProtectTarget.Data.IsDead)
            {
                guardianAngel.TurnIntoSurvivor();
            }
        }
    }
}