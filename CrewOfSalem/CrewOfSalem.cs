﻿using System;
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
        public static readonly int ShaderVisorColor   = Shader.PropertyToID("_VisorColor");
        public static readonly int ShaderOutline      = Shader.PropertyToID("_Outline");
        public static readonly int ShaderOutlineColor = Shader.PropertyToID("_OutlineColor");
        public static readonly int ShaderDesat        = Shader.PropertyToID("_Desat");

        public static AudioClip shieldAttempt;

        public static readonly Dictionary<byte, Role> AssignedSpecialRoles = new Dictionary<byte, Role>();
        public static readonly List<DeadPlayer>       DeadPlayers          = new List<DeadPlayer>();

        public static readonly List<PlayerControl> Crew = new List<PlayerControl>();
        public static readonly System.Random       Rng  = new System.Random((int) DateTime.Now.Ticks);

        public static bool GameIsRunning = false;

        public static Sprite defaultKillButton;

        // Properties
        public static Sprite InvestigatorButton => defaultKillButton;
        public static Sprite DoctorButton       => defaultKillButton;
        public static Sprite VeteranButton      => defaultKillButton;
        public static Sprite VigilanteButton    => defaultKillButton;
        public static Sprite EscortButton       => defaultKillButton;

        public static Sprite MafiosoButton => defaultKillButton;

        public static Sprite SurvivorButton => defaultKillButton;

        // Methods
        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole)
            where T : RoleGeneric<T>, new()
        {
            if (AssignedSpecialRoles.ContainsKey(specialRole.Player.PlayerId))
            {
                AssignedSpecialRoles.Remove(specialRole.Player.PlayerId);
            }

            AssignedSpecialRoles.Add(specialRole.Player.PlayerId, specialRole);
        }

        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole, PlayerControl player)
            where T : RoleGeneric<T>, new()
        {
            specialRole.InitializeRole(player);
            AddSpecialRole(specialRole);
        }

        public static Role GetSpecialRoleByPlayer(byte playerId) =>
            AssignedSpecialRoles.TryGetValue(playerId, out Role role) ? role : null;

        public static T GetSpecialRole<T>(byte playerId) where T : RoleGeneric<T>, new() =>
            AssignedSpecialRoles.TryGetValue(playerId, out Role role) ? (T) role : null;

        public static T GetSpecialRole<T>() where T : RoleGeneric<T>, new() =>
            AssignedSpecialRoles.Values.OfType<T>().FirstOrDefault();

        public static bool TryGetSpecialRole<T>(out T role) where T : RoleGeneric<T>, new()
        {
            foreach (KeyValuePair<byte, Role> kvp in AssignedSpecialRoles)
            {
                if (kvp.Value is T value)
                {
                    role = value;
                    return true;
                }
            }

            role = null;
            return false;
        }

        public static bool TryGetSpecialRoleByPlayer<T>(byte playerId, out T role) where T : RoleGeneric<T>, new()
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

        public static bool IsPlayerSpecialRole<T>(byte playerId) where T : RoleGeneric<T>, new() =>
            TryGetSpecialRoleByPlayer(playerId, out Role role) && role is T;

        public static void WriteImmediately(RPC action)
        {
            MessageWriter writer = GetWriter(action);
            CloseWriter(writer);
        }

        public static MessageWriter GetWriter(RPC action)
        {
            return AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte) action,
                SendOption.None, -1);
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

        public static string ColorizedText(string text, Color color)
        {
            return $"[{ColorToHex(color)}]{text}[]";
        }

        private static string ColorToHex(Color color)
        {
            return ((int) (color.r * 255)).ToString("X2") + ((int) (color.g * 255)).ToString("X2") +
                   ((int) (color.b * 255)).ToString("X2") + ((int) (color.a * 255)).ToString("X2");
        }
    }
}