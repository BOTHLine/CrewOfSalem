using System.Collections.Generic;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.VeteranPatches
{
    [HarmonyPatch(typeof(Ability), nameof(Ability.Use))]
    public static class AbilityUsePatch
    {
        public static bool Prefix(Ability __instance, PlayerControl target, ref bool sendRpc)
        {
            IReadOnlyList<AbilityAlert> abilityAlerts = Ability.GetAllAbilities<AbilityAlert>();
            foreach (AbilityAlert abilityAlert in abilityAlerts)
            {
                if (abilityAlert.owner.Owner != target) continue;

                __instance.owner.Owner.RpcMurderPlayer(__instance.owner.Owner);
                sendRpc = false;
                return false;
            }

            return true;
        }
    }
}