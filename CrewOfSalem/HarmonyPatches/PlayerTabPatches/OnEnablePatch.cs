using HarmonyLib;
using UnityEngine;

namespace CrewOfSalem.HarmonyPatches.PlayerTabPatches
{
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public static class OnEnablePatch
    {
        public static void Postfix(PlayerTab __instance)
        {
            int columns = 5;

            float xMin = 1.45F;

            float scale = 0.65F;
            float add = 0.45F;

            float x = xMin;
            float y = -0.05F;

            for (int i = 0; i < __instance.ColorChips.Count; i++) {
                if (i % columns == 0) {
                    x = xMin;
                    y -= add;
                } else {
                    x += add;
                }

                ColorChip chip = __instance.ColorChips[i];
                Transform transform = chip.transform;
                transform.localPosition = new Vector3(x, y, -1F);
                transform.localScale *= scale;
            }
        }
    }
}