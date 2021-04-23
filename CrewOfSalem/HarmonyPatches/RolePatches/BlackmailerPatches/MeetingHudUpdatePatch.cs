using System.Linq;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using TMPro;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.BlackmailerPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    public static class MeetingHudUpdatePatch
    {
        private static TextMeshPro blackmailedTextRenderer;

        public static void Postfix(MeetingHud __instance)
        {
            if (blackmailedTextRenderer != null || __instance?.TitleText == null) return;

            AbilityBlackmail[] blackmailAbilities = Ability.GetAllAbilities<AbilityBlackmail>();
            if (blackmailAbilities.All(blackmailAbility => blackmailAbility.BlackmailedPlayer != LocalPlayer)) return;

            blackmailedTextRenderer = Object.Instantiate(__instance.TitleText, __instance.TitleText.transform.parent);
            blackmailedTextRenderer.text = ColorizedText("You are blackmailed.", Faction.Mafia.Color);
            blackmailedTextRenderer.fontSize = 12F;
            blackmailedTextRenderer.transform.position =
                __instance.TitleText.transform.position + new Vector3(0F, -1.6F, -50F);
        }
    }
}