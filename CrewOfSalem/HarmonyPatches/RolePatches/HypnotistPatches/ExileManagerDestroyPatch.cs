using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.HypnotistPatches
{
    [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] {typeof(UnityEngine.Object)})]
    public static class ExileManagerDestroyPatch
    {
        public static bool Prefix(UnityEngine.Object obj)
        {
            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject) return true;

            AbilityHypnotize[] hypnotizeAbilities = Ability.GetAllAbilities<AbilityHypnotize>();

            foreach (AbilityHypnotize hypnotize in hypnotizeAbilities)
            {
                hypnotize.Hypnotize();
            }
            return true;
        }
    }
}