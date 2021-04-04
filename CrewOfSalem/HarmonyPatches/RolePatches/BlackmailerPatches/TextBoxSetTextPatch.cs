using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.SetText))]
    public static class TextBoxSetTextPatch
    {
        private const string BlackmailedString = "I am Blackmailed.";

        public static bool Prefix(TextBoxTMP __instance, ref string AMLONLDCANH)
        {
            if (__instance != HudManager.Instance.Chat.TextArea) return true;
            
            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            if (blackmailAbilities.All(blackmailAbility => blackmailAbility.BlackmailedPlayer != PlayerControl.LocalPlayer)) return true;

            var lastChar = ' ';
            foreach (char c in AMLONLDCANH) lastChar = c;

            switch (lastChar)
            {
                case '\r':
                case '\n':
                    AMLONLDCANH = BlackmailedString + '\r';
                    break;
                case '\b':
                    break;
                default: AMLONLDCANH = BlackmailedString.Substring(0, Mathf.Min(AMLONLDCANH.Length, BlackmailedString.Length));
                    break;
            }

            return true;
        }
    }
}