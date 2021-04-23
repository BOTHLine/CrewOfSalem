using CrewOfSalem.Roles.Abilities;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.HudManagerPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePatch
    {
        [HarmonyPriority(Priority.HigherThanNormal)]
        public static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            AbilityShield.CheckShowShieldedPlayers();

            // Add Mafia / Coven / Lover Chat
            // if (role?.Faction == Faction.Mafia || role?.Faction == Faction.Coven || role is Investigator || role is Spy)
            if (MeetingHud.Instance == null && ExileController.Instance == null)
            {
                if (!__instance.Chat.isActiveAndEnabled)
                {
                    __instance.Chat.SetVisible(true);
                }
            }
        }
    }
}