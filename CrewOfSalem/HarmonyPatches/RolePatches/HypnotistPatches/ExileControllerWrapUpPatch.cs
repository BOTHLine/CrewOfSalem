using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.HypnotistPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ExileControllerWrapUpPatch
    {
        public static void Postfix()
        {
            AbilityHypnotize[] hypnotizeAbilities = Ability.GetAllAbilities<AbilityHypnotize>();

            foreach (AbilityHypnotize hypnotize in hypnotizeAbilities)
            {
                hypnotize.Hypnotize();
            }
        }
    }
}