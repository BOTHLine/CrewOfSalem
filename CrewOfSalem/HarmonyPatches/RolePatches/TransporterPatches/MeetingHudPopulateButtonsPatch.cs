using CrewOfSalem.Roles;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.TransporterPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateButtons))]
    public static class MeetingHudPopulateButtonsPatch
    {
        public static bool[]           selected;
        public static SpriteRenderer[] renderers;

        public static void Postfix(MeetingHud __instance, [HarmonyArgument(0)] byte reporter)
        {
            if (!(LocalRole is Transporter transporter)) return;
            if (LocalPlayer != transporter.Owner) return;

            selected = new bool[__instance.playerStates.Length];
            renderers = new SpriteRenderer[__instance.playerStates.Length];

            for (var i = 0; i < __instance.playerStates.Count; i++)
            {
                PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                GameObject transporterButtonObject = Object.Instantiate(template, playerVoteArea.transform);
                transporterButtonObject.transform.position = template.transform.position;
                transporterButtonObject.transform.localPosition =
                    new Vector3(0F, 0.03F, template.transform.localPosition.z);
                var renderer = transporterButtonObject.GetComponent<SpriteRenderer>();
                renderer.sprite = ButtonTransport;
                renderer.color = Color.red;

                var button = transporterButtonObject.GetComponent<PassiveButton>();
                button.OnClick = new Button.ButtonClickedEvent();
                int index = i;
                button.OnClick.AddListener((UnityAction) Listener);

                selected[i] = false;
                renderers[i] = renderer;

                void Listener()
                {
                    OnClick(index, __instance);
                }
            }
        }

        // TODO
        private static void OnClick(int index, MeetingHud instance) { }
    }
}