using System;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BodyguardPatches
{
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Close), new Type[0])]
    public static class MinigameClosePatch
    {
        public static bool Prefix()
        {
            PlayerControl.LocalPlayer.GetAbility<AbilityGuard>()?.RpcToggleInTask(false);
            return true;
        }
    }
}