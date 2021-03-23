using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    public static class SendChatPatch
    {
        public static bool Prefix(ChatController __instance)
        {
            PlayerControl.LocalPlayer.RpcSendChat(__instance.TextArea.text);
            __instance.TextArea.Clear();
            return false;
        }
    }
}