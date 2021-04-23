using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.ChatControllerPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    public static class AddChatPatch
    {
        public static bool Prefix([HarmonyArgument(0)] PlayerControl source,[HarmonyArgument(1)] string message)
        {
            if (LocalPlayer == null) return true;
            if (LobbyBehaviour.Instance) return true;
            if (MeetingHud.Instance) return true;
            if (LocalPlayer == source) return true;
            if (LocalData.IsDead && source.Data.IsDead) return true;
            Role role = source.GetRole();
            if (LocalRole == null || role == null) return false;
            if (LocalRole.Faction == Faction.Mafia && role.Faction == Faction.Mafia && LocalData.IsDead) return true;
            if (LocalRole.Faction == Faction.Mafia && role.Faction == Faction.Mafia && LocalData.IsDead == source.Data.IsDead) return true;
            if (LocalRole.Faction == Faction.Coven && role.Faction == Faction.Coven && LocalData.IsDead) return true;
            if (LocalRole.Faction == Faction.Coven && role.Faction == Faction.Coven && LocalData.IsDead == source.Data.IsDead) return true;

            if (LocalRole is Spy && !role.Owner.Data.IsDead &&
                (role.Faction == Faction.Mafia || role.Faction == Faction.Coven))
            {
                HudManager.Instance.Chat.AddChat(LocalPlayer, role.Faction.Name + ": " + message);
                return false;
            }

            return false;
        }
    }
}