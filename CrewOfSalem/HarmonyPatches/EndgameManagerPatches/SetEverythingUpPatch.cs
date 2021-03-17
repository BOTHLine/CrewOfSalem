using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.EndgameManagerPatches
{
    // [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static class SetEverythingUpPatch
    {
        public static bool Prefix(EndGameManager __instance)
        {
            GameIsRunning = false;

            if (TempData.winners.Count > 1 && TempData.DidHumansWin(TempData.EndReason))
            {
                TempData.winners.Clear();
                var orderLocalPlayers = new List<PlayerControl>();
                foreach (PlayerControl player in Crew)
                {
                    if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        orderLocalPlayers.Add(player);
                    }
                }

                foreach (PlayerControl player in Crew)
                {
                    if (player.PlayerId != PlayerControl.LocalPlayer.PlayerId)
                    {
                        orderLocalPlayers.Add(player);
                    }
                }

                foreach (PlayerControl winner in orderLocalPlayers)
                {
                    TempData.winners.Add(new WinningPlayerData(winner.Data));
                }
            }

            return true;
        }

        public static void Postfix(EndGameManager __instance)
        {
            if (!TempData.DidHumansWin(TempData.EndReason)) return;

            foreach (PlayerControl player in Crew)
            {
                if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    return;
                }
            }

            __instance.WinText.Text = "Defeat";
            __instance.WinText.Color = Palette.ImpostorRed;
            __instance.BackgroundBar.material.color = new Color(1F, 0F, 0F);
        }
    }
}