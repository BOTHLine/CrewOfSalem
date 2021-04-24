using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class FixedUpdatePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (ShipStatus.Instance == null) return;
            if (LocalPlayer != __instance) return;

            LocalRole?.UpdateAbilities(Time.deltaTime);
        }
    }
}