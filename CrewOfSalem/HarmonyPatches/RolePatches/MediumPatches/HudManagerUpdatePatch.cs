using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MediumPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix()
        {
            if (!(LocalRole is Medium medium)) return;

            // Medium.TurnAllPlayersGrey(); // TODO: Testing
            Medium.MakeDeadVisible();
        }
    }
}