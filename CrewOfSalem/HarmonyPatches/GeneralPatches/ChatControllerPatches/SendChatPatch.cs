using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    public static class SendChatPatch
    {
        public static bool Prefix(ChatController __instance)
        {
            if (!__instance.TextArea.text.Equals("-help")) return true;

            __instance.AddChat(LocalPlayer, LocalRole.Description);
            __instance.TextArea.Clear();
            return false;
        }
    }
}