using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ExileControllerWrapUpPatch
    {
        public static void Postfix()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                IReadOnlyList<Ability> abilities = player.GetAbilities();
                foreach (Ability ability in abilities)
                {
                    ability.SetOnCooldown();
                }
            }
        }
    }
}