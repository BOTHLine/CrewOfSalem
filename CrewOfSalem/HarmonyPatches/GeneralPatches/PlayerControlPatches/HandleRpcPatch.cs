using System.Collections.Generic;
using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using CrewOfSalem.Extensions;
using CrewOfSalem.HarmonyPatches.GeneralPatches;
using CrewOfSalem.Roles.Abilities;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    // TODO: Start- und Endscreen laden zu lange.
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    public static class HandleRpcPatch
    {
        private static PlayerControl ReadPlayerControl(this MessageReader reader)
        {
            return PlayerTools.GetPlayerById(reader.ReadByte());
        }

        public static void Prefix([HarmonyArgument(0)] byte data)
        {
            ConsoleTools.Info("Reading RPC: " + (RPC) data);
        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte data,
            [HarmonyArgument(                                                 1)] MessageReader reader)
        {
            PlayerControl target;
            byte roleId;

            switch (data /*Packet ID*/)
            {
                case (byte) RPC.RequestSyncLobbyTime:
                    if (!AmongUsClient.Instance.AmHost) return;
                    MessageWriter writer = GetWriter(RPC.SyncLobbyTime);
                    writer.WriteBytesAndSize(GameStartManagerUpdatePatch.TimeBytes);
                    CloseWriter(writer);
                    break;

                case (byte) RPC.SyncLobbyTime:
                    if (AmongUsClient.Instance.AmHost) return;
                    byte[] bytes = reader.ReadBytesAndSize();
                    GameStartManagerUpdatePatch.TimeBytes = bytes;
                    break;

                case (byte) RPC.SetInfected:
                    foreach (Role role in AssignedRoles.Values)
                    {
                        role.InitializeRole();
                    }

                    break;

                case (byte) RPC.ForceEnd:
                    DebugPatches.EndGamePatch.ForceEnd();
                    break;

                case (byte) RPC.SetRole:
                    roleId = reader.ReadByte();
                    target = reader.ReadPlayerControl();

                    foreach (Role role in Main.Roles)
                    {
                        if (role.RoleID == roleId)
                        {
                            AddRole(role, target);
                        }
                    }

                    break;

                case (byte) RPC.AddKillAbility:
                    reader.ReadPlayerControl().GetRole().AddAbility<Mafioso, AbilityKill>();
                    break;

                // ---------- Special Role Conditions ----------
                case (byte) RPC.Kill:
                    PlayerControl killer = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    PlayerControl animation = reader.ReadPlayerControl();
                    killer.KillPlayer(target, animation);
                    break;

                case (byte) RPC.Watch:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityWatch>(target);
                    break;

                case (byte) RPC.WatchVisitor:
                    target = reader.ReadPlayerControl();
                    roleId = reader.ReadByte();

                    foreach (Role role in Main.Roles)
                    {
                        if (role.RoleID == roleId)
                        {
                            target.GetAbility<AbilityWatch>().AddVisitor(role);
                        }
                    }

                    break;

                case (byte) RPC.AlertStart:
                    __instance.UseAbility<AbilityAlert>(null);
                    break;

                case (byte) RPC.AlertEnd:
                    __instance.EndAbility<AbilityAlert>();
                    break;

                case (byte) RPC.GuardStart:
                    __instance.UseAbility<AbilityGuard>(null);
                    break;

                case (byte) RPC.GuardEnd:
                    __instance.EndAbility<AbilityGuard>();
                    break;

                case (byte) RPC.ShieldStart:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityShield>(target);
                    break;

                case (byte) RPC.ShieldEnd:
                    target = reader.ReadPlayerControl();
                    target.GetAbility<AbilityShield>().BreakShield();
                    break;

                case (byte) RPC.BlockAoeStart:
                    int blockedAmount = reader.ReadByte();
                    var targets = new List<PlayerControl>();
                    for (var i = 0; i < blockedAmount; i++)
                    {
                        targets.Add(reader.ReadPlayerControl());
                    }

                    __instance.GetAbility<AbilityBlockAOE>().BlockPlayers(targets);
                    break;

                case (byte) RPC.BlockAoeEnd:
                    break;

                case (byte) RPC.BlockStart:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityBlock>(target);
                    break;

                case (byte) RPC.BlockEnd:
                    __instance.EndAbility<AbilityBlock>();
                    break;

                case (byte) RPC.Reveal:
                    __instance.UseAbility<AbilityReveal>(null);
                    break;

                case (byte) RPC.DisguiseStart:
                    __instance.UseAbility<AbilityDisguise>(null);
                    break;

                case (byte) RPC.DisguiseEnd:
                    __instance.EndAbility<AbilityDisguise>();
                    break;

                case (byte) RPC.HypnotizeStart:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityHypnotize>(target);
                    break;

                case (byte) RPC.HypnotizeEnd:
                    __instance.EndAbility<AbilityHypnotize>();
                    break;

                case (byte) RPC.ForgeStart:
                    target = reader.ReadPlayerControl();
                    __instance.GetAbility<AbilityForge>().ForgeStart(target);
                    break;

                case (byte) RPC.ForgeEnd:
                    __instance.EndAbility<AbilityForge>();
                    break;

                case (byte) RPC.Blackmail:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityBlackmail>(target);
                    break;

                case (byte) RPC.ProtectStart:
                    target = reader.ReadPlayerControl();
                    var protect = __instance.GetAbility<AbilityProtect>();
                    protect.ProtectTarget = target;
                    protect.Use(null, out bool _);
                    break;

                case (byte) RPC.ProtectEnd:
                    __instance.EndAbility<AbilityProtect>();
                    break;

                case (byte) RPC.VestStart:
                    __instance.UseAbility<AbilityVest>(null);
                    break;

                case (byte) RPC.VestEnd:
                    __instance.EndAbility<AbilityVest>();
                    break;

                case (byte) RPC.VampireConvert:
                    target = reader.ReadPlayerControl();
                    AbilityBite.ConvertVampire(target);
                    break;

                case (byte) RPC.GuardianAngelTarget:
                    target = reader.ReadPlayerControl();
                    if (TryGetSpecialRole(out GuardianAngel guardianAngel)) guardianAngel.ProtectTarget = target;
                    break;

                case (byte) RPC.ExecutionerTarget:
                    target = reader.ReadPlayerControl();
                    if (TryGetSpecialRole(out Executioner executioner)) executioner.VoteTarget = target;
                    break;
            }
        }
    }
}