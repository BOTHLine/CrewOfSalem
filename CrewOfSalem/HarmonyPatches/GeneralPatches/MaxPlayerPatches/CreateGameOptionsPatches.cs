using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/*
using CreateOptionsPicker = PEOBBDIGAEP;
using TextRenderer = AELDHKGBIFD;
using PassiveButton = HHMBANDDIOA;
using SaveManager = IANFCOGHJMJ;
using GameOptionsData = KMOGFLPJLLK;
using SettingsMode = JDJOCPGLLDO;
*/

namespace CrowdedMod.Patches
{
    public static class CreateGameOptionsPatches
    {
        // [HarmonyPatch(typeof(CreateOptionsPicker), nameof(CreateOptionsPicker.Start))]
        public static class CreateOptionsPickerStart // Credits to XtraCube (mostly)
        {
            public const byte MAXPlayers = 15;

            private static void Postfix(CreateOptionsPicker __instance)
            {
                if (__instance.mode != SettingsMode.Host) return;
                float offset = __instance.MaxPlayerButtons[1].transform.position.x -
                               __instance.MaxPlayerButtons[0].transform.position.x;

                #region MaxPlayers stuff

                List<SpriteRenderer> playerButtons = __instance.MaxPlayerButtons.ToList();

                SpriteRenderer plusButton =
                    Object.Instantiate(playerButtons.Last(), playerButtons.Last().transform.parent);
                plusButton.GetComponentInChildren<TextRenderer>().Text = "+";
                plusButton.name = "255";
                plusButton.transform.position = playerButtons.Last().transform.position + new Vector3(offset * 2, 0, 0);
                var passiveButton = plusButton.GetComponent<PassiveButton>();
                passiveButton.OnClick.m_PersistentCalls.m_Calls.Clear();
                passiveButton.OnClick.AddListener((UnityAction) PlusListener);

                void PlusListener()
                {
                    byte curHighest = byte.Parse(playerButtons[__instance.MaxPlayerButtons.Length - 2].name);
                    int delta = Mathf.Clamp(curHighest + 7, curHighest, MAXPlayers) - curHighest;
                    if (delta == 0) return; // fast skip
                    for (byte i = 1; i < 8; i++)
                    {
                        SpriteRenderer button = __instance.MaxPlayerButtons[i];
                        button.name =
                            button.GetComponentInChildren<TextRenderer>().Text =
                                (byte.Parse(button.name) + delta).ToString();
                    }

                    __instance.SetMaxPlayersButtons(__instance.GetTargetOptions().MaxPlayers); // MaxPlayers
                }

                SpriteRenderer minusButton =
                    Object.Instantiate(playerButtons.Last(), playerButtons.Last().transform.parent);
                minusButton.GetComponentInChildren<TextRenderer>().Text = "-";
                minusButton.name = "255";
                minusButton.transform.position = playerButtons.First().transform.position;
                var minusPassiveButton = minusButton.GetComponent<PassiveButton>();
                minusPassiveButton.OnClick.m_PersistentCalls.m_Calls.Clear();
                minusPassiveButton.OnClick.AddListener((UnityAction) MinusListener);

                void MinusListener()
                {
                    byte curLowest = byte.Parse(playerButtons[1].name);
                    int delta = curLowest - Mathf.Clamp(curLowest - 7, 4, curLowest);
                    if (delta == 0) return; // fast skip
                    for (byte i = 1; i < 8; i++)
                    {
                        SpriteRenderer button = __instance.MaxPlayerButtons[i];
                        button.name =
                            button.GetComponentInChildren<TextRenderer>().Text =
                                (byte.Parse(button.name) - delta).ToString();
                    }

                    __instance.SetMaxPlayersButtons(__instance.GetTargetOptions().MaxPlayers); // MaxPlayers
                }

                playerButtons.ForEach(b =>
                {
                    var button = b.GetComponent<PassiveButton>();
                    button.OnClick.m_PersistentCalls.m_Calls.Clear();

                    void DefaultListener()
                    {
                        byte value = byte.Parse(button.name);
                        GameOptionsData targetOptions = __instance.GetTargetOptions();
                        if (value <= targetOptions.NumImpostors) // NumImpostors
                        {
                            targetOptions.NumImpostors = value - 1;
                            __instance.SetImpostorButtons(targetOptions.NumImpostors); // UpdateImpostorButtons
                        }

                        __instance.SetMaxPlayersButtons(value);
                    }

                    button.OnClick.AddListener((UnityAction) DefaultListener);
                    button.transform.position += new Vector3(offset, 0, 0);
                });

                playerButtons.Insert(0, minusButton);
                playerButtons.Add(plusButton);
                __instance.MaxPlayerButtons = playerButtons.ToArray();

                #endregion

                #region Impostor stuff

                List<SpriteRenderer> impostorButtons = __instance.ImpostorButtons.ToList();

                for (byte i = 4; i < 11; i++)
                {
                    SpriteRenderer button =
                        Object.Instantiate(impostorButtons.Last(), impostorButtons.Last().transform.parent);
                    button.GetComponent<PassiveButton>().name =
                        button.GetComponentInChildren<TextRenderer>().Text = i.ToString();
                    button.transform.position += new Vector3(offset, 0, 0);
                    impostorButtons.Add(button);
                }

                impostorButtons.ForEach(b =>
                {
                    var button = b.GetComponent<PassiveButton>();
                    button.OnClick.m_PersistentCalls.m_Calls.Clear();

                    void DefaultListener()
                    {
                        byte value = byte.Parse(button.name);
                        if (value >= __instance.GetTargetOptions().MaxPlayers) // MaxPlayers
                        {
                            return;
                        }

                        __instance.SetImpostorButtons(byte.Parse(button.name));
                    }

                    button.OnClick.AddListener((UnityAction) DefaultListener);
                });

                __instance.ImpostorButtons = impostorButtons.ToArray();
                __instance.SetImpostorButtons(__instance.GetTargetOptions().NumImpostors); // NumImpostors

                #endregion
            }
        }

        //[HarmonyPatch(typeof(SaveManager), nameof(SaveManager.GameHostOptions),MethodType.Getter)] // GameHostOptions
        private static class SaveManagerGetGameHostOptions
        {
            private static bool Prefix(out GameOptionsData __result)
            {
                SaveManager.GameHostOptions ??= SaveManager.Method_59("gameHostOptions");

                // patched because of impostor clamping
                SaveManager.GameHostOptions.NumImpostors = Mathf.Clamp(SaveManager.GameHostOptions.NumImpostors, 1,
                    SaveManager.GameHostOptions.MaxPlayers - 1); // NumImpostors = Clamp(1, MaxPlayers-1)
                SaveManager.GameHostOptions.KillDistance =
                    Mathf.Clamp(SaveManager.GameHostOptions.KillDistance, 0, 2); // KillDistance

                __result = SaveManager.GameHostOptions;
                return false;
            }
        }
    }
}