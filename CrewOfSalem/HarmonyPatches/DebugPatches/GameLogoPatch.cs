using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.DebugPatches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class GameLogoPatch
    {
        public static void Postfix()
        {
            Il2CppArrayBase<GameObject> allGameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in allGameObjects)
            {
                if (!gameObject.name.Equals("AmongUsLogo")) continue;

                if (Camera.main is null) continue;

                gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5F, 0.5F, -9F));

                Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0F, 0F, -9F));
                Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1F,   1F, -9F));
                Vector3 distance = topRight - bottomLeft;

                var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer == null) return;
                spriteRenderer.sprite = LogoNew;
                Bounds bounds = spriteRenderer.bounds;
                Transform transform = spriteRenderer.transform;
                Vector3 localScale = transform.localScale;
                float xScale = (localScale.x / (bounds.extents.x * 2)) * distance.x;
                float yScale = (localScale.y / (bounds.extents.y * 2)) * distance.y;
                localScale = new Vector3(xScale, yScale, localScale.z);
                transform.localScale = localScale;
            }
        }
    }
}