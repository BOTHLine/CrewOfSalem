using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController))]
    public class AddChatPatch
    {
        [HarmonyPatch(nameof(ChatController.AddChat))]
        public static bool Prefix(PlayerControl AHKBPEIJEEO, string KLNJLCOMCAI)
        {
            PlayerControl local = PlayerControl.LocalPlayer;
            PlayerControl source = AHKBPEIJEEO;
            if (local == null) return true;
            if (LobbyBehaviour.Instance) return true;
            if (MeetingHud.Instance) return true;
            if (local.PlayerId == source.PlayerId) return true;
            if (local.Data.IsDead && source.Data.IsDead) return true;
            Role localRole = local.GetRole();
            Role sourceRole = source.GetRole();
            if (localRole == null || sourceRole == null) return false;
            if (localRole.Faction == Faction.Mafia && sourceRole.Faction == Faction.Mafia &&
                local.Data.IsDead) return true;
            if (localRole.Faction == Faction.Mafia && sourceRole.Faction == Faction.Mafia &&
                local.Data.IsDead == source.Data.IsDead) return true;
            if (localRole.Faction == Faction.Coven && sourceRole.Faction == Faction.Coven &&
                local.Data.IsDead) return true;
            if (localRole.Faction == Faction.Coven && sourceRole.Faction == Faction.Coven &&
                local.Data.IsDead == source.Data.IsDead) return true;

            if (localRole is Spy && !sourceRole.Owner.Data.IsDead &&
                (sourceRole.Faction == Faction.Mafia || sourceRole.Faction == Faction.Coven))
            {
                HudManager.Instance.Chat.AddChat(local, sourceRole.Faction.Name + ": " + KLNJLCOMCAI);
                return false;
            }

            return false;
        }
    }
}