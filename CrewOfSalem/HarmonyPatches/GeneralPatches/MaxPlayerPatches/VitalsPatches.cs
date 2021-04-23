using UnityEngine;
using System.Linq;
using static CrewOfSalem.CrewOfSalem;


namespace CrowdedMod.Patches
{
    internal static class VitalsPatches
    {
        private static int currentPage = 0;
        private const  int MAXPerPage  = 10;
        private static int MAXPages => Mathf.CeilToInt((float) AllPlayers.Count() / MAXPerPage);

        //[HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
        public static class VitalsGuiPatchBegin
        {
            public static void Postfix(VitalsMinigame __instance)
            {
                //Fix the name of each player (better multi color handling)
                VitalsPanel[] vitalsPanels = __instance.vitals;
                foreach (StringNames color in Palette.ShortColorNames) //Palette.ShortColorNames
                {
                    VitalsPanel[] colorFiltered =
                        vitalsPanels.Where(panel => panel.Text.text.Equals(color.ToString())).ToArray();
                    if (colorFiltered.Length <= 1)
                        continue;
                    int i = 1;
                    foreach (VitalsPanel panel in colorFiltered)
                    {
                        panel.Text.text += i;
                        i++;
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
        public static class VitalsGuiPatchUpdate
        {
            public static void Postfix(VitalsMinigame __instance)
            {
                if (PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(LocalPlayer))
                    return;
                //Allow to switch pages
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.mouseScrollDelta.y > 0f)
                    currentPage = Mathf.Clamp(currentPage - 1, 0, MAXPages - 1);
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.mouseScrollDelta.y < 0f)
                    currentPage = Mathf.Clamp(currentPage + 1, 0, MAXPages - 1);

                //Place dead players at the beginning, disconnected at the end
                VitalsPanel[] vitalsPanels =
                    __instance.vitals.OrderBy(x => (x.IsDead ? 0 : 1) + (x.IsDiscon ? 2 : 0))
                       .ToArray(); //VitalsPanel[] //Sorted by: Dead -> Alive -> dead&disc -> alive&disc
                int i = 0;

                //Show/hide/move each panel
                foreach (VitalsPanel panel in vitalsPanels)
                {
                    if (i >= currentPage * MAXPerPage && i < (currentPage + 1) * MAXPerPage)
                    {
                        panel.gameObject.SetActive(true);
                        int relativeIndex = i % MAXPerPage;
                        // /!\ -2.7f hardcoded, can we get it the same way as MeetingHud.VoteOrigin ?
                        Transform transform = panel.transform;
                        Vector3 localPosition = transform.localPosition;
                        localPosition = new Vector3(-2.7f + 0.6f * relativeIndex, localPosition.y, localPosition.z);
                        transform.localPosition = localPosition;
                    } else
                        panel.gameObject.SetActive(false);

                    i++;
                }
            }
        }
    }
}