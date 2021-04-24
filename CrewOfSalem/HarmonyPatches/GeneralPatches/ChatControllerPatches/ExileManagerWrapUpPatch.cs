using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class ExileManagerWrapUpPatch
    {
        public static void Postfix()
        {
            HudManager.Instance?.Chat.SetVisible(true);
        }
    }
}