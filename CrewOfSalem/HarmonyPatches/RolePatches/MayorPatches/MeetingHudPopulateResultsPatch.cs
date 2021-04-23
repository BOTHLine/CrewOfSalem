using HarmonyLib;
using UnhollowerBaseLib;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MayorPatches
{
    // [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))] // TODO: Split Combined Patch?
    public static class MeetingHudPopulateResultsPatch
    {
        public static void Postfix([HarmonyArgument(0)] Il2CppStructArray<byte> states)
        {
            
        }
    }
}