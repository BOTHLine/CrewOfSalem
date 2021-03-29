using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    // TODO: Only when blackmailed
    // [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.SetText))]
    public static class ChatControllerPatches
    {
        private const string BlackmailedString = "I am Blackmailed.";

        public static bool Prefix(TextBoxTMP __instance, ref string AMLONLDCANH)
        {
            if (__instance != HudManager.Instance.Chat.TextArea) return true;

            var lastChar = ' ';
            foreach (char c in AMLONLDCANH) lastChar = c;

            switch (lastChar)
            {
                case '\r':
                case '\n':
                case '\b':
                    break;
                default:
                    AMLONLDCANH = BlackmailedString.Substring(0, Mathf.Min(AMLONLDCANH.Length, BlackmailedString.Length));
                    break;
            }

            return true;
        }
    }
}