using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
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

        public static bool Prefix([HarmonyArgument(0)] byte data)
        {
            // ConsoleTools.Info("Reading RPC: " + (RPC) data);
            return true;
        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] byte data,
            [HarmonyArgument(                                                 1)] MessageReader reader)
        {
            PlayerControl target;

            switch (data /*Packet ID*/)
            {
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
                    byte roleId = reader.ReadByte();
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

                case (byte) RPC.StartMeetingCustom:
                    __instance.StartMeetingCustom(GameData.Instance.GetPlayerById(reader.ReadByte()));
                    break;

                // ---------- Special Role Conditions ----------
                case (byte) RPC.Kill:
                    PlayerControl killer = reader.ReadPlayerControl();
                    target = reader.ReadPlayerControl();
                    PlayerControl animation = reader.ReadPlayerControl();
                    killer.KillPlayer(target, animation);
                    break;

                case (byte) RPC.AlertStart:
                    __instance.UseAbility<AbilityAlert>(null);
                    break;

                case (byte) RPC.AlertEnd:
                    __instance.EndAbility<AbilityAlert>();
                    break;

                case (byte) RPC.ToggleGuard:
                    __instance.UseAbility<AbilityGuard>(null);
                    break;

                case (byte) RPC.ToggleInTask:
                    __instance.GetAbility<AbilityGuard>().ToggleInTask();
                    break;

                case (byte) RPC.Shield:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityShield>(target);
                    break;

                case (byte) RPC.ShieldBreak:
                    target = reader.ReadPlayerControl();
                    target.GetAbility<AbilityShield>().BreakShield();
                    break;

                case (byte) RPC.Block:
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

                case (byte) RPC.Hypnotize:
                    target = reader.ReadPlayerControl();
                    __instance.UseAbility<AbilityHypnotize>(target);
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