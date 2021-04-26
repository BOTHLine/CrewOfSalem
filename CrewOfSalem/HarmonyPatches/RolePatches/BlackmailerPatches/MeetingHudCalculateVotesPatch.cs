using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using UnhollowerBaseLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))]
    [HarmonyPriority(Priority.Low)]
    public static class MeetingHudCalculateVotesPatch
    {
        public static void Postfix(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            if (blackmailAbilities.Length == 0) return;

            foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
            {
                if (!playerVoteArea.didVote) continue;

                int num = playerVoteArea.votedFor + 1;
                if (num < 0 || num >= __result.Length) continue;

                if (blackmailAbilities.Any(blackmail => playerVoteArea.TargetPlayerId == blackmail.BlackmailedPlayer?.PlayerId))
                {
                    __result[num] -= (byte) (MayorPatches.MeetingHudCalculateVotesPatch.extraVote == num ? 2 : 1);
                }
            }
        }
    }
}