using HarmonyLib;
using System;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        private static bool isImpostor;

        public static bool Prefix(PlayerControl __instance, PlayerControl PAIBDFDMIGK)
        {
            isImpostor = __instance.Data.IsImpostor;
            __instance.Data.IsImpostor = true;
            return true;
        }

        public static void Postfix(PlayerControl __instance, PlayerControl PAIBDFDMIGK)
        {
            __instance.Data.IsImpostor = isImpostor;
            DeadPlayers.Add(new DeadPlayer(PAIBDFDMIGK, __instance, DateTime.UtcNow));
            if (__instance.GetRole().Faction != Faction.Mafia) return;

            Role mafiaRole = Main.Roles.FirstOrDefault(role => role.Faction == Faction.Mafia && !role.Owner.Data.IsDead);
            mafiaRole?.AddAbility(new AbilityKill(mafiaRole, 30F));
        }
    }
}