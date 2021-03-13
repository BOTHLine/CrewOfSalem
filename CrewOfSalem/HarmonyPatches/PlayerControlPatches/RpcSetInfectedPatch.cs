using CrewOfSalem.Roles;
using HarmonyLib;
using System.Collections.Generic;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;
using System.Linq;
using Hazel;

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

            Role.SetRole<Investigator>(ref crewmates, Main.OptionInvestigatorSpawnChance);
            Role.SetRole<Sheriff>(ref crewmates, Main.OptionSheriffSpawnChance);
            Role.SetRole<Spy>(ref crewmates, Main.OptionSpySpawnChance);
            Role.SetRole<Tracker>(ref crewmates, Main.OptionTrackerSpawnChance);

            Role.SetRole<Vigilante>(ref crewmates, Main.OptionVigilanteSpawnChance);

            Role.SetRole<Doctor>(ref crewmates, Main.OptionDoctorSpawnChance);

            Role.SetRole<Escort>(ref crewmates, Main.OptionEscortSpawnChance);


            Role.SetRole<Jester>(ref crewmates, Main.OptionJesterSpawnChance);

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