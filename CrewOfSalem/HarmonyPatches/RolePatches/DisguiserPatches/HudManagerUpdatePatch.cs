using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.DisguiserPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        [HarmonyPriority(Priority.VeryLow)]
        public static void Postfix()
        {
            AbilityDisguise[] disguiseAbilities = Ability.GetAllAbilities<AbilityDisguise>();
            if (disguiseAbilities.Any(disguiseAbility => disguiseAbility.HasDurationLeft))
            {
                AbilityDisguise.Disguise();
            }
        }
    }
}