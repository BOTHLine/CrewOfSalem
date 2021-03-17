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
        public static void Postfix(Il2CppReferenceArray<GameData.PlayerInfo> FMAOEJEHPAO)
        {
            List<Role> assignedRoles = AssignedSpecialRoles.Values.ToList();

            foreach (Role r in assignedRoles) {
                r.ClearSettingsInternal();
            }

            ResetValues();

            WriteImmediately(RPC.ResetVariables);

            List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToList();
            crewmates.RemoveAll(x => x.Data.IsImpostor);

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

            Role.SetRole<Survivor>(ref crewmates);

            Role.SetRole<Jester>(ref crewmates);

            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList();
            impostors.RemoveAll(x => !x.Data.IsImpostor);

            Crew.Clear();


            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (Main.OptionShowPlayerNames.GetValue() == 2) player.nameText.Text = "";
                if (player.Data.IsImpostor) continue;
                if (GetSpecialRoleByPlayer(player.PlayerId) is Jester) continue;

                Crew.Add(player);
            }

            MessageWriter writer = GetWriter(RPC.SetLocalPlayers);
            writer.WriteBytesAndSize(Crew.Select(player => player.PlayerId).ToArray());
            CloseWriter(writer);
        }
    }
}