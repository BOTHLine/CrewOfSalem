using InnerNet;

namespace CrowdedMod.Patches
{
    //[HarmonyPatch(typeof(GameListing), nameof(GameListing), MethodType.Constructor)]
    public static class CustomPatches
    {
        public static void Postfix(GameListing __instance)
        {
            __instance.MaxPlayers = CreateGameOptionsPatches.CreateOptionsPickerStart.MAXPlayers;
        }
    }
}