using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.DidVote))]
    public static class MeetingHudDidVotePatch
    {
        public static bool Prefix(MeetingHud __instance, out bool __result, [HarmonyArgument(0)] byte playerId)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            __result = __instance.playerStates.First((p) => p.TargetPlayerId == (sbyte) playerId).didVote ||
                       blackmailAbilities.Any(blackmailAbility =>
                           blackmailAbility.BlackmailedPlayer == PlayerTools.GetPlayerById(playerId));
            return false;
        }
    }
}