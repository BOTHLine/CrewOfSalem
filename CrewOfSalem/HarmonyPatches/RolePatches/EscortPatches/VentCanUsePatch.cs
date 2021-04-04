using System.Linq;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.EscortPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        [HarmonyPriority(Priority.HigherThanNormal)]
        public static bool Prefix(ref bool OFPIPGCNGAK, ref bool KHBPLGBBIEC)
        {
            AbilityBlock[] blockAbilities = Ability.GetAllAbilities<AbilityBlock>();
            if (!blockAbilities.Any(blockAbility =>
                blockAbility.owner is Escort && blockAbility.BlockedPlayer == PlayerControl.LocalPlayer))
            {
                return true;
            }

            OFPIPGCNGAK = KHBPLGBBIEC = false;
            return false;
        }
    }
}