using System.Linq;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.EscortPatches
{
    // [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        [HarmonyPriority(Priority.HigherThanNormal)]
        public static bool Prefix([HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
        {
            AbilityBlock[] blockAbilities = Ability.GetAllAbilities<AbilityBlock>();
            if (!blockAbilities.Any(blockAbility =>
                blockAbility.owner is Escort && blockAbility.BlockedPlayer == LocalPlayer))
            {
                return true;
            }

            canUse = couldUse = false;
            return false;
        }
    }
}