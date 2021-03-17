using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Alignments;
using HarmonyLib;
using System;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static bool Prefix(PlayerControl __instance, PlayerControl PAIBDFDMIGK)
        {
            if (TryGetSpecialRoleByPlayer(__instance.PlayerId, out Role role) && role.Alignment is Killing)
                __instance.Data.IsImpostor = true;
            return true;
        }

        public static void Postfix(PlayerControl __instance, PlayerControl PAIBDFDMIGK)
        {
            PlayerControl current = __instance;
            PlayerControl target = PAIBDFDMIGK;

            DeadPlayer deadPlayer = new DeadPlayer(target, current, DateTime.UtcNow);

            if (TryGetSpecialRoleByPlayer(current.PlayerId, out Role role) && role.Alignment is Killing) {
                current.Data.IsImpostor = false;
            }

            DeadPlayers.Add(deadPlayer);

            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Tracker tracker) &&
                target != tracker.Player)
                tracker.SendChatMessage(Tracker.MessageType.PlayerDied);
        }
    }
}