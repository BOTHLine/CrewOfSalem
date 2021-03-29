using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    public class RpcSetInfectedPatch
    {
        public static bool Prefix(ref Il2CppReferenceArray<GameData.PlayerInfo> FMAOEJEHPAO)
        {
            List<Role> assignedRoles = AssignedSpecialRoles.Values.ToList();

            foreach (Role r in assignedRoles)
            {
                r.ClearSettings();
            }

            ResetValues();

            WriteImmediately(RPC.ResetVariables);

            var infected = new List<GameData.PlayerInfo>();

            List<Role> availableRoles = Main.Roles.ToList();
            List<RoleSlot> roleSlots = Main.RoleSlots.ToList();
            List<PlayerControl> availablePlayers = PlayerControl.AllPlayerControls.ToArray().ToList();
            for (int i = 0; i < roleSlots.Count && availablePlayers.Count > 0; i++)
            {
                Role role = roleSlots[i].GetRole(ref availableRoles);
                PlayerControl player = availablePlayers[Rng.Next(0, availablePlayers.Count)];
                Role.SetRole(role, player);

                availableRoles.Remove(role);
                availablePlayers.Remove(player);
                if (roleSlots[i].IsInfected)
                {
                    infected.Add(player.Data);
                }
            }

            FMAOEJEHPAO = new Il2CppReferenceArray<GameData.PlayerInfo>(infected.ToArray());
            return true;
        }

        public static void Postfix(Il2CppReferenceArray<GameData.PlayerInfo> FMAOEJEHPAO)
        {
            /*
            List<PlayerControl> crewmates =
                PlayerControl.AllPlayerControls.ToArray().Where(p => !p.Data.IsImpostor).ToList();

            List<PlayerControl> impostors =
                PlayerControl.AllPlayerControls.ToArray().Where(p => p.Data.IsImpostor).ToList();

            Role.SetRole<Investigator>(ref crewmates);
            // Lookout
            Role.SetRole<Psychic>(ref crewmates);
            Role.SetRole<Sheriff>(ref crewmates);
            Role.SetRole<Spy>(ref crewmates);
            Role.SetRole<Tracker>(ref crewmates);

            // Jailor
            // Vampire Hunter
            Role.SetRole<Veteran>(ref crewmates);
            Role.SetRole<Vigilante>(ref crewmates);

            // Bodyguard
            Role.SetRole<Doctor>(ref crewmates);
            // Crusader
            // Trapper

            Role.SetRole<Escort>(ref crewmates);
            // Mayor
            // Medium
            // Retributionist
            // Transporter

            Role.SetRole<Disguiser>(ref impostors);
            // Forger
            // Framer
            // Hypnotist
            // Janitor

            // Ambusher
            // Godfather
            Role.SetRole<Mafioso>(ref impostors);

            // Blackmailer
            // Consigliere
            // Consort

            // Amnesiac
            // Guardian Angel
            Role.SetRole<Survivor>(ref crewmates);

            // Vampire

            Role.SetRole<Executioner>(ref crewmates);
            Role.SetRole<Jester>(ref crewmates);
            // Witch

            // Arsonist
            // Serial Killer
            // Werewolf

            Crew.Clear();
            */

            MessageWriter writer = GetWriter(RPC.SetLocalPlayers);
            writer.WriteBytesAndSize(PlayerControl.AllPlayerControls.ToArray().Select(player => player.PlayerId)
               .ToArray());
            CloseWriter(writer);
        }
    }
}