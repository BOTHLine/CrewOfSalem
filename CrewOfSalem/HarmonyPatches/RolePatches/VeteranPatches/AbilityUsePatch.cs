using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;

namespace CrewOfSalem.HarmonyPatches.RolePatches.VeteranPatches
{
    // [HarmonyPatch(typeof(Ability), nameof(Ability.Use))]
    public static class AbilityUsePatch
    {
        public static bool Prefix(Ability __instance, PlayerControl target, ref bool sendRpc)
        {
            IReadOnlyList<AbilityAlert> abilityAlerts = Ability.GetAllAbilities<AbilityAlert>();
            foreach (AbilityAlert abilityAlert in abilityAlerts)
            {
                if (abilityAlert.owner.Owner != target) continue;

                __instance.owner.Owner.RpcKillPlayer(__instance.owner.Owner, abilityAlert.owner.Owner);
                sendRpc = false;
                return false;
            }

            return true;
        }
    }
}