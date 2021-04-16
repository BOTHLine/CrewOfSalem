using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BodyguardPatches
{
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    public static class MinigameBeginPatch
    {
        public static bool Prefix()
        {
            LocalPlayer.GetAbility<AbilityGuard>()?.RpcToggleInTask(true);
            return true;
        }
    }
}