using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingHudCoIntroPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            __instance.TitleText.Text = ColorizedText("You are blackmailed.", Faction.Mafia.Color);
        }
    }
}