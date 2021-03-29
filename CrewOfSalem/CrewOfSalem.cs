using System;
using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Reactor.Extensions;
using UnityEngine;

namespace CrewOfSalem
{
    [HarmonyPatch]
    public class CrewOfSalem
    {
        // Fields
        public static readonly int ShaderBackColor    = Shader.PropertyToID("_BackColor");
        public static readonly int ShaderBodyColor    = Shader.PropertyToID("_BodyColor");
        public static readonly int ShaderOutlineColor = Shader.PropertyToID("_OutlineColor");
        public static readonly int ShaderVisorColor   = Shader.PropertyToID("_VisorColor");
        public static readonly int ShaderOutline      = Shader.PropertyToID("_Outline");
        public static readonly int ShaderDesat        = Shader.PropertyToID("_Desat");
        public static readonly int ShaderPercent      = Shader.PropertyToID("_Percent");


        public static AudioClip shieldAttempt;

        public static readonly Dictionary<byte, Role> AssignedSpecialRoles = new Dictionary<byte, Role>();
        public static readonly List<DeadPlayer>       DeadPlayers          = new List<DeadPlayer>();

        public static readonly System.Random       Rng  = new System.Random((int) DateTime.Now.Ticks);

        public static bool gameIsRunning = false;

        private static Sprite buttonInvestigate;
        private static Sprite buttonAlert;
        private static Sprite buttonShield;
        private static Sprite buttonBlock;
        private static Sprite buttonDisguise;
        private static Sprite buttonKill;
        private static Sprite buttonSteal;
        private static Sprite buttonForge;
        private static Sprite buttonVest;

        // Properties
        public static Sprite ButtonInvestigate =>
            buttonInvestigate ??= LoadSpriteFromResources("ButtonInvestigate.png", 110F);

        public static Sprite ButtonAlert  => buttonAlert ??= LoadSpriteFromResources("ButtonAlert.png",   110F);
        public static Sprite ButtonKill   => buttonKill ??= LoadSpriteFromResources("ButtonKill.png",     110F);
        public static Sprite ButtonShield => buttonShield ??= LoadSpriteFromResources("ButtonShield.png", 110F);
        public static Sprite ButtonBlock  => buttonBlock ??= LoadSpriteFromResources("ButtonBlock.png",   110F);

        public static Sprite ButtonDisguise =>
            buttonDisguise ??= LoadSpriteFromResources("ButtonDisguise.png", 110F);

        public static Sprite ButtonSteal => buttonSteal ??= LoadSpriteFromResources("ButtonSteal.png", 110F);
        public static Sprite ButtonForge => buttonForge ??= LoadSpriteFromResources("ButtonForge.png", 110F);

        public static Sprite ButtonVest => buttonVest ??= LoadSpriteFromResources("ButtonVest.png", 110F);

        // Methods
        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole)
            where T : RoleGeneric<T>, new()
        {
            if (AssignedSpecialRoles.ContainsKey(specialRole.Owner.PlayerId))
            {
                AssignedSpecialRoles.Remove(specialRole.Owner.PlayerId);
            }

            AssignedSpecialRoles.Add(specialRole.Owner.PlayerId, specialRole);
        }

        public static void AddSpecialRole<T>(RoleGeneric<T> specialRole, PlayerControl player)
            where T : RoleGeneric<T>, new()
        {
            specialRole.InitializeRole(player);
            AddSpecialRole(specialRole);
        }

        private static void AddSpecialRole(Role role)
        {
            if (AssignedSpecialRoles.ContainsKey(role.Owner.PlayerId))
            {
                AssignedSpecialRoles.Remove(role.Owner.PlayerId);
            }

            AssignedSpecialRoles.Add(role.Owner.PlayerId, role);
        }

        public static void AddSpecialRole(Role role, PlayerControl player)
        {
            role.InitializeRole(player);
            AddSpecialRole(role);
        }

        public static Role GetSpecialRoleByPlayerID(byte playerId) =>
            AssignedSpecialRoles.TryGetValue(playerId, out Role role) ? role : null;

        public static Role GetSpecialRoleByPlayer(PlayerControl player)
        {
            return player == null ? null : GetSpecialRoleByPlayerID(player.PlayerId);
        }

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

        public static void SetSkinWithAnim(PlayerPhysics playerPhysics, uint skinId)
        {
            SkinData nextSkin = HatManager.Instance.AllSkins[(int) skinId];
            AnimationClip clip;

            AnimationClip currentPhysicsAnim = playerPhysics.Animator.GetCurrentAnimation();
            if (currentPhysicsAnim == playerPhysics.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;

            float progress = playerPhysics.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.Skin.skin = nextSkin;

            playerPhysics.Skin.animator.Play(clip, 1f);
            playerPhysics.Skin.animator.m_animator.Play("a", 0, progress % 1);
            playerPhysics.Skin.animator.m_animator.Update(0f);
        }

        private static Sprite LoadSpriteFromResources(string path, float pixelsPerUnit)
        {
            path = "CrewOfSalem.Resources." + path;
            try
            {
                Texture2D texture2D = GUIExtensions.CreateEmptyTexture();
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                byte[] byteTexture = stream.ReadFully();
                ImageConversion.LoadImage(texture2D, byteTexture, false);
                return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    new Vector2(0.5F, 0.5F),
                    pixelsPerUnit);
            }
            catch
            {
                ConsoleTools.Error("Failed loading sprites from path: " + path);
            }

            return null;
        }
    }
}