using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingHudClosePatch
    {
        public static void Prefix()
        {
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            foreach (AbilityBlackmail blackmailAbility in blackmailAbilities)
            {
                blackmailAbility.BlackmailedPlayer = null;
            }
        }
    }
}