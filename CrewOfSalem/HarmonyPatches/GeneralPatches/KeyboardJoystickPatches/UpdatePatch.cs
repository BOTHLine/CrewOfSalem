using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.KeyboardJoystickPatches
{
    [HarmonyPatch(typeof(KeyboardJoystick))]
    public static class UpdatePatch
    {
        [HarmonyPatch(nameof(KeyboardJoystick.Update))]
        public static void Postfix(KeyboardJoystick __instance)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IReadOnlyList<Ability> abilities = PlayerControl.LocalPlayer.GetAbilities();
                if (abilities?.Count > 0)
                {
                    abilities[0]?.TryUse();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab) &&
                LobbyBehaviour.Instance != null /*&& Object.FindObjectOfType<GameOptionsMenu>() == null*/)
            {
                Main.TurnPage();
                // OptionPage.TurnPage();
            }
        }
    }
}