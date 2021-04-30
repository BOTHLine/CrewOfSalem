using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(MedScannerBehaviour), nameof(MedScannerBehaviour.Position), MethodType.Getter)]
    public static class MedScannerBehaviourPositionPatch
    {
        public static bool Prefix(MedScannerBehaviour __instance, ref Vector3 __result)
        {
            if (!Main.OptionRemoveMedbayProof) return true;

            const float maxOffset = 0.2F;

            float xOffset = (LocalPlayer.PlayerId - 5) * (maxOffset / 5F);
            float yOffset = -maxOffset + LocalPlayer.PlayerId * (maxOffset / 10F);
            __result = __instance.transform.position + __instance.Offset + new Vector3(xOffset, yOffset, 0F);

            return false;
        }
    }
}