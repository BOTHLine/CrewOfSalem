using HarmonyLib;
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
        public static bool Prefix(PlayerControl __instance, out bool __state, [HarmonyArgument(0)] PlayerControl target)
        {
            __state = __instance.Data.IsImpostor;
            __instance.Data.IsImpostor = true;
            return true;
        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl victim, bool __state)
        {
            __instance.Data.IsImpostor = __state;

            if (!AmongUsClient.Instance.AmHost) return;
            if (victim.GetRole().Faction != Faction.Mafia) return;

            if (AssignedRoles.Values.Count(role =>
                    role.Faction == Faction.Mafia && !role.Owner.Data.IsDead &&
                    role.GetAbility<AbilityKill>() != null) >=
                Main.OptionMafiaKillAlways.GetValue()) return;

            Role[] mafiaWithoutKill = AssignedRoles.Values.Where(role =>
                    role.Faction == Faction.Mafia && !role.Owner.Data.IsDead && role.GetAbility<AbilityKill>() == null)
               .ToArray();
            if (mafiaWithoutKill.Length == 0) return;

            Role newKiller = mafiaWithoutKill[Rng.Next(mafiaWithoutKill.Length)];
            newKiller.AddAbility<Mafioso, AbilityKill>(true);
            WriteRPC(RPC.AddKillAbility, newKiller.Owner.PlayerId);
        }
    }
}