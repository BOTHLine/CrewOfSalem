using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MayorPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Awake))]
    public static class MeetingHudAwakePatch
    {
        public static void Prefix()
        {
            if (TryGetSpecialRole(out Mayor mayor) && !mayor.hasRevealed)
            {
                mayor.hasRevealed = mayor.GetAbility<AbilityReveal>().hasRevealed;
            }
        }
    }
}