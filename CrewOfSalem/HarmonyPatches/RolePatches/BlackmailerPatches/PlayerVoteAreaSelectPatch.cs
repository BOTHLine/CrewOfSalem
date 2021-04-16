using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
    public static class PlayerVoteAreaSelectPatch
    {
        public static bool Prefix()
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            return blackmailAbilities.All(blackmailAbility => blackmailAbility.BlackmailedPlayer != LocalPlayer);
        }
    }
}