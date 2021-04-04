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
        public static bool Prefix(PlayerControl __instance, PlayerControl PAIBDFDMIGK, out bool __state)
        {
            __state = __instance.Data.IsImpostor;
            return true;
        }

        public static void Postfix(PlayerControl __instance, PlayerControl PAIBDFDMIGK, bool __state)
        {
            __instance.Data.IsImpostor = __state;
            DeadPlayers.Add(new DeadPlayer(PAIBDFDMIGK, __instance, DateTime.UtcNow));

            if (PAIBDFDMIGK.GetRole().Faction != Faction.Mafia) return;

            Role mafiaRole =
                Main.Roles.FirstOrDefault(role => role.Faction == Faction.Mafia && !role.Owner.Data.IsDead);
            mafiaRole?.AddAbility<Mafioso, AbilityKill>();
        }
    }
}