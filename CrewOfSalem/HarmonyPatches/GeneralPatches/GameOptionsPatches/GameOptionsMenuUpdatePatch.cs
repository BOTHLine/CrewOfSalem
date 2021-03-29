using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.GameSettingPatches
{
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    public static class GameOptionsMenuUpdatePatch
    {
        public static void Postfix(ref GameOptionsMenu __instance)
        {
            __instance.GetComponentInParent<Scroller>().YBounds.max = 16F;
        }
    }
}