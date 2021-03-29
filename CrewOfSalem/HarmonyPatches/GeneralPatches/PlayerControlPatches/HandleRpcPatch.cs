using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public static class HandleRpcPatch
    {
        private static PlayerControl ReadPlayerControl(this MessageReader reader)
        {
            return PlayerTools.GetPlayerById(reader.ReadByte());
        }

        public static bool Prefix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {
            ConsoleTools.Info("RPC: " + (RPC) ACCJCEHMKLN);
            return true;
        }

        public static void Postfix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {
            MessageReader reader = HFPCBBHJIPJ;

            PlayerControl source;
            PlayerControl target;

            switch (ACCJCEHMKLN /*Packet ID*/)
            {
                case (byte) RPC.ReportDeadBody:
                    if (TryGetSpecialRole(out Psychic psychic))
                    {
                        psychic.StartMeeting();
                    }

                    break;

                case (byte) RPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    {
                        if (player.PlayerId == playerId)
                        {
                            switch (roleId)
                            {
                                case var value when value == Investigator.GetRoleID():
                                    AddSpecialRole(new Investigator(), player);
                                    break;

                                // Lookout

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

                                // Jailor

                                // Vampire Hunter

                                case var value when value == Veteran.GetRoleID():
                                    AddSpecialRole(new Veteran(), player);
                                    break;

                                case var value when value == Vigilante.GetRoleID():
                                    AddSpecialRole(new Vigilante(), player);
                                    break;

                                case var value when value == Bodyguard.GetRoleID():
                                    AddSpecialRole(new Bodyguard(), player);
                                    break;

                                case var value when value == Doctor.GetRoleID():
                                    AddSpecialRole(new Doctor(), player);
                                    break;

                                // Crusader

                                // Trapper

                                case var value when value == Escort.GetRoleID():
                                    AddSpecialRole(new Escort(), player);
                                    break;

                                // Mayor

                                // Medium

                                // Retributionist

                                // Transport

                                case var value when value == Disguiser.GetRoleID():
                                    AddSpecialRole(new Disguiser(), player);
                                    break;

                                // Framer

                                // Hypnotist

                                // Janitor

                                case var value when value == Ambusher.GetRoleID():
                                    AddSpecialRole(new Ambusher(), player);
                                    break;

                                // Godfather

                                case var value when value == Forger.GetRoleID():
                                    AddSpecialRole(new Forger(), player);
                                    break;

                                case var value when value == Mafioso.GetRoleID():
                                    AddSpecialRole(new Mafioso(), player);
                                    break;

                                // Blackmailer

                                case var value when value == Consigliere.GetRoleID():
                                    AddSpecialRole(new Consigliere(), player);
                                    break;

                                case var value when value == Consort.GetRoleID():
                                    AddSpecialRole(new Consort(), player);
                                    break;

                                // Amnesiac

                                // Guardian Angel

                                case var value when value == Survivor.GetRoleID():
                                    AddSpecialRole(new Survivor(), player);
                                    break;

                                // Vampire

                                case var value when value == Executioner.GetRoleID():
                                    AddSpecialRole(new Executioner(), player);
                                    break;

                                case var value when value == Jester.GetRoleID():
                                    AddSpecialRole(new Jester(), player);
                                    break;

                                // Witch

                                // Arsonist

                                // Serial Killer

                                // Werewolf
                            }
                        }
                    }

                    break;
                case (byte) RPC.ResetVariables:
                    List<Role> assignedRoles = AssignedSpecialRoles.Values.ToList();
                    foreach (Role role in assignedRoles)
                    {
                        role.ClearSettings();
                    }

                    ResetValues();
                    break;

                // ---------- Special Role Conditions ----------
                case (byte) RPC.Kill:
                    source = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    source.MurderPlayer(target);
                    break;

                case (byte) RPC.AlertStart:
                    source = reader.ReadPlayerControl();
                    source.UseAbility<AbilityAlert>(null);
                    break;

                case (byte) RPC.AlertEnd:
                    source = reader.ReadPlayerControl();
                    source.EndAbility<AbilityAlert>();
                    break;

                case (byte) RPC.ToggleGuard:
                    source = reader.ReadPlayerControl();
                    source.UseAbility<AbilityGuard>(null);
                    break;

                case (byte) RPC.ToggleInTask:
                    source = reader.ReadPlayerControl();
                    source.GetAbility<AbilityGuard>().ToggleInTask();
                    break;

                case (byte) RPC.ShieldStart:
                    source = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    source.UseAbility<AbilityShield>(target);
                    break;

                case (byte) RPC.ShieldEnd:
                    source = reader.ReadPlayerControl();
                    source.EndAbility<AbilityShield>();
                    break;

                case (byte) RPC.Block:
                    source = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    source.UseAbility<AbilityBlock>(target);
                    break;

                case (byte) RPC.DisguiseStart:
                    source = reader.ReadPlayerControl();
                    source.UseAbility<AbilityDisguise>(null);
                    break;

                case (byte) RPC.DisguiseEnd:
                    source = reader.ReadPlayerControl();
                    source.EndAbility<AbilityDisguise>();
                    break;

                case (byte) RPC.ForgeStart:
                    source = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    source.UseAbility<AbilityForge>(target);
                    break;

                case (byte) RPC.ForgeEnd:
                    source = reader.ReadPlayerControl();
                    source.EndAbility<AbilityForge>();
                    break;

                case (byte) RPC.VestStart:
                    source = reader.ReadPlayerControl();
                    source.UseAbility<AbilityVest>(null);
                    break;

                case (byte) RPC.VestEnd:
                    source = reader.ReadPlayerControl();
                    source.EndAbility<AbilityVest>();
                    break;
            }
        }
    }
}