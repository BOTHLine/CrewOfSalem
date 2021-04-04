using System.Linq;
using CrewOfSalem.Roles.Abilities;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    // [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.DidVote))]
    public static class MeetingHudDidVotePatch
    {
        public static bool Prefix(MeetingHud __instance, out bool __result, byte FEFHEFFFBBI)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            __result = __instance.playerStates.First((p) => p.TargetPlayerId == (sbyte) FEFHEFFFBBI).didVote ||
                       blackmailAbilities.Any(blackmailAbility =>
                           blackmailAbility.BlackmailedPlayer == PlayerTools.GetPlayerById(FEFHEFFFBBI));
            return false;
        }
    }
}