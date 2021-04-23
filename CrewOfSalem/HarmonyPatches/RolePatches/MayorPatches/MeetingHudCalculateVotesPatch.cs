using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MayorPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))]
    public static class MeetingHudCalculateVotesPatch
    {
        public static int extraVote;

        public static void Postfix(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
        {
            if (!TryGetSpecialRole(out Mayor mayor)) return;
            if (!mayor.GetAbility<AbilityReveal>()?.hasRevealed ?? true) return;

            foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
            {
                if (!playerVoteArea.didVote) continue;

                int num = playerVoteArea.votedFor + 1;
                if (num < 0 || num >= __result.Length) continue;

                if (playerVoteArea.TargetPlayerId != mayor.Owner.PlayerId) continue;

                extraVote = num;
                __result[num] += 1;
                return;
            }
        }
    }
}