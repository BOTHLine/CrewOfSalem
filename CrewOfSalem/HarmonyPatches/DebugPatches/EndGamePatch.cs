using CrewOfSalem.Extensions;
using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.DebugPatches
{
    // TODO: Remove for real Games
    // [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class EndGamePatch
    {
        public static void Postfix()
        {
            if (!Input.GetKeyDown(KeyCode.L) || LobbyBehaviour.Instance != null) return;

            RpcForceEnd();
        }

        private static void RpcForceEnd()
        {
            WriteRPC(RPC.ForceEnd);
            ForceEnd();
        }

        public static void ForceEnd()
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (!player.Data.IsImpostor)
                {
                    player.RemoveInfected();
                    player.KillPlayer(player, player);
                    player.Data.IsImpostor = true;
                }
            }
        }
    }
}