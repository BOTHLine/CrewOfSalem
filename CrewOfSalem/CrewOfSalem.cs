using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrewOfSalem
{
    [HarmonyPatch]
    public class CrewOfSalem
    {
        // Fields
        public static Sprite DefaultKillButton;

        public static Sprite InvestigatorButton; // Investigator
        public static Sprite DoctorButton; // Doctor
        public static Sprite VigilanteButton; // Vigilante

        public static AudioClip shieldAttempt;

        public static readonly Dictionary<byte, Role> AssignedSpecialRoles = new Dictionary<byte, Role>();
        public static readonly List<DeadPlayer> DeadPlayers = new List<DeadPlayer>();

        public static PlayerControl LocalPlayer = null;
        public static readonly List<PlayerControl> Crew = new List<PlayerControl>();
        public static readonly System.Random RNG = new System.Random();

        public static bool GameIsRunning = false;

        // Methods
        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole) where T : RoleGeneric<T>, new()
        {
            if (AssignedSpecialRoles.ContainsKey(specialRole.Player.PlayerId))
            {
                AssignedSpecialRoles.Remove(specialRole.Player.PlayerId);
            }
            AssignedSpecialRoles.Add(specialRole.Player.PlayerId, specialRole);
        }

        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole, PlayerControl player) where T : RoleGeneric<T>, new()
        {
            specialRole.InitializeRole(player);
            AddSpecialRole(specialRole);
        }

        public static Role GetSpecialRoleByPlayer(byte playerId) => AssignedSpecialRoles.TryGetValue(playerId, out Role role) ? role : null;

        public static T GetSpecialRole<T>(byte playerId) where T : RoleGeneric<T>, new() => AssignedSpecialRoles.TryGetValue(playerId, out Role role) ? (T)role : null;

        public static T GetSpecialRole<T>() where T : RoleGeneric<T>, new() => AssignedSpecialRoles.Values.OfType<T>().FirstOrDefault();

        public static bool TryGetSpecialRole<T>(byte playerId, out T role) where T : RoleGeneric<T>, new()
        {
            if (AssignedSpecialRoles.TryGetValue(playerId, out Role tempRole) && tempRole is T value)
            {
                role = value;
                return true;
            }
            role = null;
            return false;
        }

        public static bool TryGetSpecialRoleByPlayer(byte playerId, out Role role)
        {
            if (AssignedSpecialRoles.TryGetValue(playerId, out Role tempRole))
            {
                role = tempRole;
                return true;
            }
            role = null;
            return false;
        }

        public static bool IsPlayerSpecialRole<T>(byte playerId) where T : RoleGeneric<T>, new() => TryGetSpecialRoleByPlayer(playerId, out Role role) && role is T;

        public static bool SpecialRoleIsAssigned<T>(out KeyValuePair<byte, T> keyValuePair) where T : RoleGeneric<T>, new()
        {
            foreach ((var key, Role value) in AssignedSpecialRoles.Where(kvp => kvp.Value is T))
            {
                keyValuePair = new KeyValuePair<byte, T>(key, (T)value);
                return true;
            }
            keyValuePair = default;
            return false;
        }

        public static void WriteImmediately(RPC action)
        {
            MessageWriter writer = GetWriter(action);
            CloseWriter(writer);
        }

        public static MessageWriter GetWriter(RPC action)
        {
            return AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)action, SendOption.None, -1);
        }

        public static void CloseWriter(MessageWriter writer)
        {
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void ResetValues()
        {
            AssignedSpecialRoles.Clear();
            DeadPlayers.Clear();
        }

        // Nested Types
        public enum RPC
        {
            PlayAnimation = 0,
            CompleteTask = 1,
            SyncSettings = 2,
            SetInfected = 3,
            Exiled = 4,
            CheckName = 5,
            SetName = 6,
            CheckColor = 7,
            SetColor = 8,
            SetHat = 9,
            SetSkin = 10,
            ReportDeadBody = 11,
            MurderPlayer = 12,
            SendChat = 13,
            StartMeeting = 14,
            SetScanner = 15,
            SendChatNote = 16,
            SetPet = 17,
            SetStartCounter = 18,
            EnterVent = 19,
            ExitVent = 20,
            SnapTo = 21,
            Close = 22,
            VotingComplete = 23,
            CastVote = 24,
            ClearVote = 25,
            AddVote = 26,
            CloseDoorsOfType = 27,
            RepairSystem = 28,
            SetTasks = 29,
            UpdateGameData = 30,
            // --- Custom RPCs --- TODO:
            SetRole = 42,
            SetLocalPlayers = 43,
            ResetVariables = 44,

            VigilanteKill,
            DoctorSetShielded,
            DoctorBreakShield,
            EscortIncreaseCooldown,
            JesterWin
        }

        public static class ModdedPalette
        {
            public static Color shieldedColor = new Color(0F, 1F, 1F, 1F);
        }
    }
}