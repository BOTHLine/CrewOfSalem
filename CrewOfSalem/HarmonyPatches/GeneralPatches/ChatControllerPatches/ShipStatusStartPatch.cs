using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    public static class ShipStatusStartPatch
    {
        public static void Postfix()
        {
            HudManager.Instance?.Chat.SetVisible(true);
        }
    }
}