using static CrewOfSalem.CrewOfSalem;

/*
using KeyMinigame = AMKEIECODLC;
using PlayerControl = FFGALNAPKCD;
using SecurityLogger = MAOCFFOEGFE;
*/

namespace CrowdedMod.Patches
{
    internal static class MiniGamePatches
    {
        //[HarmonyPatch(typeof(SecurityLogger), nameof(SecurityLogger.Awake))]
        public static class SecurityLoggerPatch
        {
            public static void Postfix(ref SecurityLogger __instance)
            {
                __instance.Timers = new float[CreateGameOptionsPatches.CreateOptionsPickerStart.MAXPlayers]; // Timers
            }
        }

        //[HarmonyPatch(typeof(KeyMinigame), nameof(KeyMinigame.Start))]
        public static class KeyMinigamePatch
        {
            public static bool Prefix(ref KeyMinigame __instance, out byte __state)
            {
                __state = LocalPlayer.PlayerId;
                LocalPlayer.PlayerId %= 10;
                return true;
            }

            public static void Postfix(ref KeyMinigame __instance, byte __state)
            {
                LocalPlayer.PlayerId = __state;
            }
        }
    }
}