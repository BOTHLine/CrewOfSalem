using CrewOfSalem.Roles;
using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.KeyboardJoystickPatches
{
    [HarmonyPatch(typeof(KeyboardJoystick))]
    public static class UpdatePatch
    {
        [HarmonyPatch(nameof(KeyboardJoystick.Update))]
        public static void Postfix(KeyboardJoystick __instance)
        {
            if (Input.GetKeyDown(KeyCode.Q) &&
                TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role))
            {
                role.SpecialButton.Use();
            }
        }
    }
}