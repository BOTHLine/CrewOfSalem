using System.Linq;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.ConsortPatches
{
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    public static class MinigameBeginPatch
    {
        public static void Postfix(Minigame __instance)
        {
            AbilityBlock[] blockAbilities = Ability.GetAllAbilities<AbilityBlock>();
            if (blockAbilities.Any(blockAbility => blockAbility.owner is Consort && blockAbility.BlockedPlayer == LocalPlayer))
            {
                __instance.Close();
            }
        }
    }
}