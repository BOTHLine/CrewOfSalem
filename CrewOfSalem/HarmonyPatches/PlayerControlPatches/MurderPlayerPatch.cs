using CrewOfSalem.Roles;
using HarmonyLib;
using System;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
   public static class MurderPlayerPatch
    {
        public static bool Prefix(PlayerControl __instance, PlayerControl CAKODNGLPDF)
        {
            if (TryGetSpecialRoleByPlayer(__instance.PlayerId, out Role role) && role is Vigilante)
                __instance.Data.IsImpostor = true;
            return true;
        }

        public static void Postfix(PlayerControl __instance, PlayerControl CAKODNGLPDF)
        {
            PlayerControl current = __instance;
            PlayerControl target = CAKODNGLPDF;

            DeadPlayer deadPlayer = new DeadPlayer(target, current, DateTime.UtcNow);

            if (TryGetSpecialRole(current.PlayerId, out Vigilante _))
            {
                current.Data.IsImpostor = false;
            }
            DeadPlayers.Add(deadPlayer);

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Tracker tracker) && target != tracker.Player)
                tracker.SendChatMessage(Tracker.MessageType.PlayerDied);
        }
    }
}