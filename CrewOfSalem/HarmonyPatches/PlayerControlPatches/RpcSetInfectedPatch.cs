using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    public class RpcSetInfectedPatch
    {
        public static void Postfix(Il2CppReferenceArray<GameData.PlayerInfo> JPGEIBIBJPJ)
        {
            List<Role> assignedRoles = AssignedSpecialRoles.Values.ToList();

            foreach (Role r in assignedRoles)
            {
                r.ClearSettings();
            }
            ResetValues();

            WriteImmediately(RPC.ResetVariables);

            List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToList();
            crewmates.RemoveAll(x => x.Data.IsImpostor);

            Role.SetRole<Investigator>(ref crewmates);
            Role.SetRole<Sheriff>(ref crewmates);
            Role.SetRole<Spy>(ref crewmates);
            Role.SetRole<Tracker>(ref crewmates);

            Role.SetRole<Veteran>(ref crewmates);
            Role.SetRole<Vigilante>(ref crewmates);

            Role.SetRole<Doctor>(ref crewmates);

            Role.SetRole<Escort>(ref crewmates);


            Role.SetRole<Jester>(ref crewmates);
            
            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList();
            impostors.RemoveAll(x => !x.Data.IsImpostor);

            Crew.Clear();
            LocalPlayer = PlayerControl.LocalPlayer;

            bool jesterExists = SpecialRoleIsAssigned<Jester>(out var jesterKvp);
            Jester jester = jesterKvp.Value;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.Data.IsImpostor) continue;
                if (jesterExists && jester.Player.PlayerId == player.PlayerId) continue;

                Crew.Add(player);
            }

            MessageWriter writer = GetWriter(RPC.SetLocalPlayers);
            writer.WriteBytesAndSize(Crew.Select(player => player.PlayerId).ToArray());
            CloseWriter(writer);
        }
    }
}