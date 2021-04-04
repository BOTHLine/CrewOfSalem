using System;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Abilities;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    // [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
    public static class MeetingHudCheckForEndVotingPatch
    {
        public static bool Prefix(MeetingHud __instance)
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            if (blackmailAbilities.All(blackmailAbility => blackmailAbility.BlackmailedPlayer == null)) return true;

            if (__instance.playerStates.All(ps => ps.isDead || ps.didVote || blackmailAbilities.Any(blackmailAbility =>
                blackmailAbility.BlackmailedPlayer == PlayerTools.GetPlayerById((byte) ps.TargetPlayerId))))
            {
                byte[] self = __instance.CalculateVotes();
                int maxIdx = self.IndexOfMax(p => (int) p, out bool tie) - 1;
                GameData.PlayerInfo exiled = GameData.Instance.AllPlayers.ToArray()
                   .FirstOrDefault(v => (int) v.PlayerId == maxIdx);
                var array = new byte[10];
                foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
                {
                    array[playerVoteArea.TargetPlayerId] = playerVoteArea.GetState();
                }

                __instance.Method_0(array, exiled, tie);
            }

            return false;
        }

        private static int IndexOfMax<T>(this IReadOnlyList<T> self, Func<T, int> comparer, out bool tie)
        {
            tie = false;
            var num = int.MinValue;
            int result = -1;
            for (var i = 0; i < self.Count; i++)
            {
                int num2 = comparer(self[i]);
                if (num2 > num)
                {
                    result = i;
                    num = num2;
                    tie = false;
                } else if (num2 == num)
                {
                    tie = true;
                    result = -1;
                }
            }

            return result;
        }
    }
}