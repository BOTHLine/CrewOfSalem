using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public class HandleRpcPatch
    {
        private static readonly int Outline = Shader.PropertyToID("_Outline");
        private static readonly int VisorColor = Shader.PropertyToID("_VisorColor");

        public static void Postfix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {
            MessageReader reader = HFPCBBHJIPJ;

            switch (ACCJCEHMKLN /*Packet ID*/) {
                case (byte) RPC.ReportDeadBody:
                    if (TryGetSpecialRole(out Psychic psychic)) {
                        psychic.StartMeeting();
                    }

                    break;

                case (byte) RPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (player.PlayerId == playerId) {
                            switch (roleId) {
                                case var value when value == Investigator.GetRoleID():
                                    AddSpecialRole(new Investigator(), player);
                                    break;

                                case var value when value == Psychic.GetRoleID():
                                    AddSpecialRole(new Psychic(), player);
                                    break;

                                case var value when value == Sheriff.GetRoleID():
                                    AddSpecialRole(new Sheriff(), player);
                                    break;

                                case var value when value == Spy.GetRoleID():
                                    AddSpecialRole(new Spy(), player);
                                    break;

                                case var value when value == Tracker.GetRoleID():
                                    AddSpecialRole(new Tracker(), player);
                                    break;

                                case var value when value == Veteran.GetRoleID():
                                    AddSpecialRole(new Veteran(), player);
                                    break;

                                case var value when value == Vigilante.GetRoleID():
                                    AddSpecialRole(new Vigilante(), player);
                                    break;

                                case var value when value == Doctor.GetRoleID():
                                    AddSpecialRole(new Doctor(), player);
                                    break;

                                case var value when value == Escort.GetRoleID():
                                    AddSpecialRole(new Escort(), player);
                                    break;

                                case var value when value == Survivor.GetRoleID():
                                    AddSpecialRole(new Survivor(), player);
                                    break;

                                case var value when value == Jester.GetRoleID():
                                    AddSpecialRole(new Jester(), player);
                                    break;
                            }
                        }
                    }

                    break;
                // ---------- Special Role Conditions ----------

                case (byte) RPC.VeteranAlert:
                    var veteran = GetSpecialRole<Veteran>();
                    veteran.CurrentDuration = veteran.AlertDuration;
                    break;

                case (byte) RPC.VeteranKill:
                    byte killerID = reader.ReadByte();
                    byte victimID = reader.ReadByte();
                    PlayerControl killer = PlayerTools.GetPlayerById(killerID);
                    PlayerControl victim = PlayerTools.GetPlayerById(victimID);
                    killer.MurderPlayer(victim);
                    break;

                case (byte) RPC.VigilanteKill:
                    killerID = reader.ReadByte();
                    victimID = reader.ReadByte();
                    killer = PlayerTools.GetPlayerById(killerID);
                    victim = PlayerTools.GetPlayerById(victimID);
                    killer.MurderPlayer(victim);
                    break;

                case (byte) RPC.DoctorSetShielded:
                    byte shieldedId = reader.ReadByte();
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (player.PlayerId == shieldedId) {
                            GetSpecialRole<Doctor>().ShieldedPlayer = player;
                        }
                    }

                    break;

                case (byte) RPC.DoctorBreakShield:
                    Doctor doctor = GetSpecialRole<Doctor>();
                    PlayerControl shieldedPlayer = doctor.ShieldedPlayer;
                    if (shieldedPlayer != null) {
                        shieldedPlayer.myRend.material.SetColor(VisorColor, Palette.VisorColor);
                        shieldedPlayer.myRend.material.SetFloat(Outline, 0F);
                    }

                    doctor.ShieldedPlayer = null;
                    break;

                case (byte) RPC.EscortIncreaseCooldown:
                    byte targetID = reader.ReadByte();
                    PlayerControl target = PlayerTools.GetPlayerById(targetID);
                    target.SetKillTimer(target.killTimer + GetSpecialRole<Escort>().Duration);
                    break;

                case (byte) RPC.ResetVariables:
                    List<Role> assignedRoles = AssignedSpecialRoles.Values.ToList();
                    foreach (Role role in assignedRoles) {
                        role.ClearSettingsInternal();
                    }

                    ResetValues();
                    break;

                case (byte) RPC.JesterWin:
                    GetSpecialRole<Jester>().Win();
                    break;
            }
        }
    }
}