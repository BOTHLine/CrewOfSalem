using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.ForgerPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        [HarmonyPriority(Priority.Low)]
        public static void Postfix()
        {
            AbilityForge[] forgeAbilities = Ability.GetAllAbilities<AbilityForge>();
            foreach (AbilityForge forgeAbility in forgeAbilities)
            {
                if (!forgeAbility.HasDurationLeft) continue;

                forgeAbility.Forge();
                return;
            }
        }
    }
}