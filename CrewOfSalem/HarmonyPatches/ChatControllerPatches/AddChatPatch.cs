using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController))]
    public class AddChatPatch
    {
        [HarmonyPatch(nameof(ChatController.AddChat))]
        public static bool Prefix(PlayerControl AHKBPEIJEEO, string KLNJLCOMCAI)
        {
            if (PlayerControl.LocalPlayer == null) return true;
            if (PlayerControl.LocalPlayer.PlayerId == AHKBPEIJEEO.PlayerId) return true;
            if (MeetingHud.Instance != null) return true;
            if (LobbyBehaviour.Instance != null) return true;
            if (PlayerControl.LocalPlayer.Data.IsDead) return true;
            Role localRole = GetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId);
            Role sourceRole = GetSpecialRoleByPlayer(AHKBPEIJEEO.PlayerId);
            if (localRole.Faction == Faction.Mafia && sourceRole.Faction == Faction.Mafia) return true;
            if (localRole.Faction == Faction.Coven && sourceRole.Faction == Faction.Coven) return true;

            if (localRole is Spy && (sourceRole.Faction == Faction.Mafia || sourceRole.Faction == Faction.Coven))
            {
                HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer,
                    sourceRole.Faction.Name + ": " + KLNJLCOMCAI);
                return false;
            }

            return false;
        }
    }
}