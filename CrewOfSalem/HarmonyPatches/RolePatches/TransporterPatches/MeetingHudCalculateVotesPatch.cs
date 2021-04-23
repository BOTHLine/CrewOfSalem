using CrewOfSalem.Roles;
using HarmonyLib;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

// TODO: Split this Patch
namespace CrewOfSalem.HarmonyPatches.RolePatches.TransporterPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))]
    [HarmonyPriority(Priority.Last)]
    public static class MeetingHudCalculateVotesPatch
    {
        public static void Postfix(ref Il2CppStructArray<byte> __result)
        {
            if (!TryGetSpecialRole(out Transporter transporter)) return;
            

            PlayerVoteArea transported1 = null;
            PlayerVoteArea transported2 = null;

            foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
            {
                if (playerVoteArea.TargetPlayerId == transporter.Transported1?.PlayerId) transported1 = playerVoteArea;
                else if (playerVoteArea.TargetPlayerId == transporter.Transported2?.PlayerId) transported2 = playerVoteArea;
            }

            if (transported1 == null || transported2 == null || transported1.TargetPlayerId + 1 < 0 ||
                transported1.TargetPlayerId + 1 >= __result.Length || transported2.TargetPlayerId + 1 < 0 ||
                transported2.TargetPlayerId + 1 >= __result.Length) return;
            

            byte temp = __result[transported1.TargetPlayerId + 1];
            __result[transported1.TargetPlayerId + 1] = __result[transported2.TargetPlayerId + 1];
            __result[transported2.TargetPlayerId + 1] = temp;
        }
    }
}