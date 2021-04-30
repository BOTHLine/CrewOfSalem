using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles;
using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;
using Object = UnityEngine.Object;

// TODO: Add Losers to End Screen (just) to show their roles?
// TODO: "Lag" probably comes from SceneManager.LoadScene("EndGame") in AmongUsClient.CoEndGame
namespace CrewOfSalem.HarmonyPatches.GeneralPatches.EndGameManagerPatches
{
    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public static class SetEverythingUpPatch
    {
        public static void Postfix(EndGameManager __instance)
        {
            WinningPlayerData[] winners = TempData.winners.ToArray().OrderBy(b => b.IsYou ? -1 : 0).ToArray();
            List<PoolablePlayer> poolablePlayers = Object.FindObjectsOfType<PoolablePlayer>()
               .OrderBy(player => player.transform.localPosition.x).ToList();

            var winningPlayerPositions = new Vector3[winners.Length];
            for (var i = 0; i < winningPlayerPositions.Length; i++)
            {
                int num = (i % 2 == 0) ? -1 : 1;
                int num2 = (i + 1) / 2;
                float num3 = 1f - num2 * 0.075f;
                float num4 = 1f - num2 * 0.035f;
                float num5 = (i == 0) ? -8 : -1;
                winningPlayerPositions[i] = new Vector3(0.8f * num * num2 * num4,
                    __instance.BaseY - 0.25f + num2 * 0.1f, num5 + num2 * 0.01f) * 1.25f;
            }

            foreach (PoolablePlayer poolablePlayer in poolablePlayers)
            {
                for (var i = 0; i < winningPlayerPositions.Length; i++)
                {
                    if (poolablePlayer.transform.localPosition.Equals(winningPlayerPositions[i]))
                    {
                        if (PlayerNames.TryGetValue(winners[i].Name, out Role role))
                        {
                            poolablePlayer.NameText.text = $"{winners[i].Name}\n({role.Name})";
                            poolablePlayer.NameText.color = PlayerNames[winners[i].Name].Color;
                        }

                        poolablePlayer.NameText.fontSize *= 0.8F;
                        poolablePlayer.NameText.gameObject.SetActive(true);
                    }
                }
            }

            foreach (DeadPlayer deadPlayer in DeadPlayers)
            {
                ConsoleTools.Info(deadPlayer.Killer.Data.PlayerName + " killed " + deadPlayer.Victim.Data.PlayerName +
                                  " at " + deadPlayer.KillTime.TimeOfDay);
            }

            ResetValues();
        }
    }
}