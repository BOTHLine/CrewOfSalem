using System;
using CrewOfSalem.Roles;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CrewOfSalem.Extensions;
using Reactor.Extensions;
using UnityEngine;

namespace CrewOfSalem
{
    [HarmonyPatch]
    public class CrewOfSalem
    {
        // Fields
        public static readonly int ShaderBackColor     = Shader.PropertyToID("_BackColor");
        public static readonly int ShaderBodyColor     = Shader.PropertyToID("_BodyColor");
        public static readonly int ShaderOutlineColor  = Shader.PropertyToID("_OutlineColor");
        public static readonly int ShaderVisorColor    = Shader.PropertyToID("_VisorColor");
        public static readonly int ShaderOutline       = Shader.PropertyToID("_Outline");
        public static readonly int ShaderDesat         = Shader.PropertyToID("_Desat");
        public static readonly int ShaderMask          = Shader.PropertyToID("_Mask");
        public static readonly int ShaderNormalizedUvs = Shader.PropertyToID("_NormalizedUvs");

        public static readonly Dictionary<string, Role> PlayerNames   = new Dictionary<string, Role>();
        public static readonly Dictionary<byte, Role>   AssignedRoles = new Dictionary<byte, Role>();
        public static readonly List<DeadPlayer>         DeadPlayers   = new List<DeadPlayer>();

        public static readonly System.Random Rng = new System.Random((int) DateTime.Now.Ticks);

        public static bool gameIsRunning = false;

        private static Sprite logo;

        private static Sprite buttonInvestigate;
        private static Sprite buttonWatch;
        private static Sprite buttonMap;
        private static Sprite buttonSurveillance;
        private static Sprite buttonVitals;

        private static Sprite buttonAlert;
        private static Sprite buttonKill;

        private static Sprite buttonGuard;
        private static Sprite buttonShield;

        private static Sprite buttonBlock;
        private static Sprite buttonReveal;
        private static Sprite buttonSeance;
        private static Sprite buttonTransport;

        private static Sprite buttonDisguise;
        private static Sprite buttonHypnotize;

        private static Sprite buttonSteal;
        private static Sprite buttonForge;

        private static Sprite buttonBlackmail;

        private static Sprite buttonProtect;
        private static Sprite buttonVest;
        private static Sprite buttonBite;

        // Properties
        public static IEnumerable<PlayerControl> AllPlayers  => PlayerControl.AllPlayerControls.ToArray();
        public static PlayerControl              LocalPlayer => PlayerControl.LocalPlayer;
        public static GameData.PlayerInfo        LocalData   => LocalPlayer?.Data;
        public static Role                       LocalRole   => LocalPlayer?.GetRole();

        public static Sprite Logo => logo ??= LoadSpriteFromResources("CrewOfSalem.png", 450);

        public static Sprite ButtonInvestigate => buttonInvestigate ??= LoadSpriteFromResources("ButtonInvestigate.png");
        public static Sprite ButtonWatch => buttonWatch ??= LoadSpriteFromResources("ButtonWatch.png");
        public static Sprite ButtonMap => buttonMap ??= LoadSpriteFromResources("ButtonMap.png");
        public static Sprite ButtonSurveillance => buttonSurveillance ??= LoadSpriteFromResources("ButtonSurveillance.png");
        public static Sprite ButtonVitals => buttonVitals ??= LoadSpriteFromResources("ButtonVitals.png");

        public static Sprite ButtonAlert => buttonAlert ??= LoadSpriteFromResources("ButtonAlert.png");
        public static Sprite ButtonKill  => buttonKill ??= LoadSpriteFromResources("ButtonKill.png");

        public static Sprite ButtonGuard  => buttonGuard ??= LoadSpriteFromResources("ButtonGuard.png");
        public static Sprite ButtonShield => buttonShield ??= LoadSpriteFromResources("ButtonShield.png");

        public static Sprite ButtonBlock     => buttonBlock ??= LoadSpriteFromResources("ButtonBlock.png");
        public static Sprite ButtonReveal    => buttonReveal ??= LoadSpriteFromResources("ButtonReveal.png");
        public static Sprite ButtonSeance    => buttonSeance ??= LoadSpriteFromResources("ButtonSeance.png");
        public static Sprite ButtonTransport => buttonTransport ??= LoadSpriteFromResources("ButtonTransport.png");

        public static Sprite ButtonDisguise  => buttonDisguise ??= LoadSpriteFromResources("ButtonDisguise.png");
        public static Sprite ButtonHypnotize => buttonHypnotize ??= LoadSpriteFromResources("ButtonHypnotize.png");

        public static Sprite ButtonSteal => buttonSteal ??= LoadSpriteFromResources("ButtonSteal.png");
        public static Sprite ButtonForge => buttonForge ??= LoadSpriteFromResources("ButtonForge.png");

        public static Sprite ButtonBlackmail => buttonBlackmail ??= LoadSpriteFromResources("ButtonBlackmail.png");

        public static Sprite ButtonProtect => buttonProtect ??= LoadSpriteFromResources("ButtonProtect.png");
        public static Sprite ButtonVest    => buttonVest ??= LoadSpriteFromResources("ButtonVest.png");
        public static Sprite ButtonBite    => buttonBite ??= LoadSpriteFromResources("ButtonBite");

        // Methods
        private static void AddRole(Role role)
        {
            if (PlayerNames.ContainsKey(role.Owner.Data.PlayerName))
            {
                PlayerNames.Remove(role.Owner.Data.PlayerName);
            }

            PlayerNames.Add(role.Owner.Data.PlayerName, role);

            if (AssignedRoles.ContainsKey(role.Owner.PlayerId))
            {
                AssignedRoles[role.Owner.PlayerId].ClearSettings();
                AssignedRoles.Remove(role.Owner.PlayerId);
            }

            AssignedRoles.Add(role.Owner.PlayerId, role);

            // role.SetRoleTask();
        }

        public static void AddRole(Role role, PlayerControl player)
        {
            role.Owner = player;
            AddRole(role);
        }

        public static Role GetSpecialRoleByPlayerID(byte playerId) =>
            AssignedRoles.TryGetValue(playerId, out Role role) ? role : null;

        public static Role GetSpecialRoleByPlayer(PlayerControl player)
        {
            return player == null ? null : GetSpecialRoleByPlayerID(player.PlayerId);
        }

        public static bool TryGetSpecialRole<T>(out T role) where T : RoleGeneric<T>, new()
        {
            foreach (KeyValuePair<byte, Role> kvp in AssignedRoles)
            {
                if (!(kvp.Value is T value)) continue;

                role = value;
                return true;
            }

            role = null;
            return false;
        }

        public static MessageWriter GetWriter(RPC action)
        {
            return AmongUsClient.Instance.StartRpcImmediately(LocalPlayer.NetId, (byte) action, SendOption.None, -1);
        }

        public static void CloseWriter(MessageWriter writer)
        {
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void WriteRPC(RPC action, params byte[] data)
        {
            ConsoleTools.Info("Writing RPC: " + action);
            MessageWriter writer = GetWriter(action);
            foreach (byte b in data)
            {
                ConsoleTools.Info("RPC Data: " + b);
                writer.Write(b);
            }

            CloseWriter(writer);
        }

        public static void ResetValues()
        {
            foreach (Role role in AssignedRoles.Values)
            {
                role.ClearSettings();
            }

            AssignedRoles.Clear();
            DeadPlayers.Clear();
            PlayerNames.Clear();
        }

        public static string MultiColorText(params Tuple<string, Color>[] textColorPairs)
        {
            return textColorPairs.Aggregate("",
                (current, textColorPair) => current + ColorizedText(textColorPair.Item1, textColorPair.Item2));
        }

        public static string ColorizedText(string text, Color color)
        {
            return $"<color=#{ColorToHex(color)}>{text}</color>";
        }

        private static string ColorToHex(Color color)
        {
            return ((int) (color.r * 255)).ToString("X2") + ((int) (color.g * 255)).ToString("X2") +
                   ((int) (color.b * 255)).ToString("X2") + ((int) (color.a * 255)).ToString("X2");
        }

        public static void SetSkinWithAnim(PlayerPhysics playerPhysics, uint skinId)
        {
            SkinData nextSkin = HatManager.Instance.AllSkins.ToArray()[(int) skinId];
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

        private static Sprite LoadSpriteFromResources(string path, float pixelsPerUnit = 100F)
        {
            path = "CrewOfSalem.Resources." + path;
            try
            {
                Texture2D texture2D = GUIExtensions.CreateEmptyTexture();
                var assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                byte[] byteTexture = stream.ReadFully();
                ImageConversion.LoadImage(texture2D, byteTexture, false);
                return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    new Vector2(0.5F, 0.5F), pixelsPerUnit).DontDestroy();
            }
            catch
            {
                ConsoleTools.Error("Failed loading sprites from path: " + path);
            }

            return null;
        }

        public static void TurnAllPlayersGrey()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                player.nameText.text = "";
                player.myRend.material.SetColor(ShaderBackColor, Color.grey);
                player.myRend.material.SetColor(ShaderBodyColor, Color.grey);
                player.HatRenderer.SetHat(0, 0);
                SetSkinWithAnim(player.MyPhysics, 0);
                if (player.CurrentPet) UnityEngine.Object.Destroy(player.CurrentPet.gameObject);
            }
        }

        public static void ResetPlayerColors()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                player.SetName(player.Data.PlayerName);
                player.SetHat(player.Data.HatId, player.Data.ColorId);
                SetSkinWithAnim(player.MyPhysics, player.Data.SkinId);
                player.SetPet(player.Data.PetId);
                player.CurrentPet.Visible = player.Visible;
                player.SetColor(player.Data.ColorId);
            }
        }
    }
}