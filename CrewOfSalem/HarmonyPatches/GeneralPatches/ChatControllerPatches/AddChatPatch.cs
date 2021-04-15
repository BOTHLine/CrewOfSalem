using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    public class AddChatPatch
    {
        public static bool Prefix([HarmonyArgument(0)] PlayerControl source,[HarmonyArgument(1)] string message)
        {
            PlayerControl local = PlayerControl.LocalPlayer;
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
                HudManager.Instance.Chat.AddChat(local, sourceRole.Faction.Name + ": " + message);
                return false;
            }

            return false;
        }
    }
}