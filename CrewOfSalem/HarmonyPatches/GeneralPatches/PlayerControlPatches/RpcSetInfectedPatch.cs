using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Factions;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    public static class RpcSetInfectedPatch
    {
        public static bool Prefix([HarmonyArgument(0)] ref Il2CppReferenceArray<GameData.PlayerInfo> playerInfos)
        {
            List<Role> assignedRoles = AssignedRoles.Values.ToList();

            foreach (Role r in assignedRoles)
            {
                r.ClearSettings();
            }

            var infected = new List<GameData.PlayerInfo>();
            var roles = new List<Role>();

            List<Role> availableRoles = Main.Roles.ToList();

            List<RoleSlot> roleSlots = Main.GetRoleSlots().ToList();
            List<PlayerControl> availablePlayers = AllPlayers.ToList();
            for (var i = 0; i < roleSlots.Count && availablePlayers.Count > 0; i++)
            {
                RoleSlot roleSlot = roleSlots[i];
                var spawnChance = 0;
                IEnumerable<Role> possibleRoles = roleSlot.GetFittingRoles(availableRoles);
                var roleSpawnChances = new List<RoleSpawnChancePair>();
                foreach (Role possibleRole in possibleRoles)
                {
                    roleSpawnChances.Add(new RoleSpawnChancePair(possibleRole,
                        spawnChance += (int) Main.GetRoleSpawnChance(possibleRole.GetType())));
                }

                // IEnumerable<RoleSpawnChancePair> roleSpawnChances = roleSlot.GetFittingRoles(availableRoles).Select(role => new RoleSpawnChancePair(role, spawnChance += (int) Main.GetRoleSpawnChance(role.GetType())));
                int spawnValue = Rng.Next(spawnChance);
                foreach (RoleSpawnChancePair roleSpawnChance in roleSpawnChances)
                {
                    if (roleSpawnChance.SpawnChance > spawnValue)
                    {
                        roles.Add(roleSpawnChance.Role);
                        PlayerControl player = availablePlayers[Rng.Next(availablePlayers.Count)];
                        Role.RpcSetRole(roleSpawnChance.Role, player);

                        availableRoles.Remove(roleSpawnChance.Role);
                        availablePlayers.Remove(player);
                        if (roleSlots[i].IsInfected)
                        {
                            infected.Add(player.Data);
                        }

                        break;
                    }
                }

                /*
                Role role = roleSlots[i].GetRole(ref availableRoles);
                PlayerControl player = availablePlayers[Rng.Next(availablePlayers.Count)];
                Role.SetRole(role, player);

                availableRoles.Remove(role);
                availablePlayers.Remove(player);
                if (roleSlots[i].IsInfected)
                {
                    infected.Add(player.Data);
                }
                */
            }

            foreach (Role role in roles)
            {
                role.InitializeRole();
            }

            int killAbilitiesToAdd = (int) Main.OptionMafiaKillStart.GetValue() - roles.Count(role =>
                role.Faction == Faction.Mafia && role.GetAbility<AbilityKill>() != null);

            // TODO: Randomize order of adding killAbilities, so you don't have an advantage if you log into lobby earlier than another one
            for (var i = 0; i < roles.Count && killAbilitiesToAdd > 0; i++)
            {
                Role role = roles[i];
                if (role.Faction != Faction.Mafia || role.GetAbility<AbilityKill>() != null) continue;

                role.AddAbility<Mafioso, AbilityKill>(true);
                WriteRPC(RPC.AddKillAbility, role.Owner.PlayerId);
                killAbilitiesToAdd--;
            }

            playerInfos = new Il2CppReferenceArray<GameData.PlayerInfo>(infected.ToArray());

            return true;
        }

        public static void Postfix([HarmonyArgument(0)] Il2CppReferenceArray<GameData.PlayerInfo> playerInfos)
        {
            MessageWriter writer = GetWriter(RPC.SetLocalPlayers);
            writer.WriteBytesAndSize(AllPlayers.Select(player => player.PlayerId).ToArray());
            CloseWriter(writer);
        }

        private struct RoleSpawnChancePair
        {
            public Role Role;
            public int  SpawnChance;

            public RoleSpawnChancePair(Role role, int spawnChance)
            {
                Role = role;
                SpawnChance = spawnChance;
            }
        }
    }
}