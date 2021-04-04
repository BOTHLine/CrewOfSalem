using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MediumPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdatePatch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix()
        {
            if (PlayerControl.LocalPlayer.GetRole() is Medium medium)
            {
                Medium.TurnAllGray();
                Medium.MakeDeadVisible();
            }
        }
    }
}