using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class MeetingHudStartPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();

            foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
            {
                if (blackmailAbilities.Any(blackmailAbility =>
                    blackmailAbility.BlackmailedPlayer.PlayerId == playerVoteArea.TargetPlayerId))
                {
                    playerVoteArea.didVote = true;
                    playerVoteArea.votedFor = -2;
                }
            }
        }
    }
}