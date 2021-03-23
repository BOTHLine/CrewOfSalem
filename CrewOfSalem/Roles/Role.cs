using System;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;
using static CrewOfSalem.Main;

namespace CrewOfSalem.Roles
{
    public abstract class Role
    {
        // Properties
        protected abstract byte   RoleID { get; }
        public abstract    string Name   { get; }

        public abstract Faction   Faction   { get; }
        public abstract Alignment Alignment { get; }

        protected virtual Color Color        => Faction.Color;
        protected virtual Color OutlineColor { get; } = new Color(0, 0, 0, 1);

        protected virtual string  StartText  => Alignment.GetColorizedTask(Faction);
        protected virtual Vector3 TitleScale { get; } = new Vector3(1, 1, 1);

        protected abstract bool HasSpecialButton { get; }

        public             RoleButton SpecialButton       { get; private set; }
        protected abstract Sprite     SpecialButtonSprite { get; }

        public  float Cooldown        { get; set; }
        public  float Duration        { get; set; }
        private float CurrentDuration { get; set; }
        public  bool  HasDurationLeft => CurrentDuration > 0F;

        protected virtual bool NeedsTarget => true;

        // protected PlayerControl CurrentTarget { get; set; }
        protected virtual Func<bool> CanUse        => () => true;
        protected virtual Action     OnMeetingEnds => null;
        protected virtual Vector3    ButtonOffset  => Vector3.zero;

        public PlayerControl Player { get; protected set; }

        // Methods
        private static byte GetRoleID<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetRoleID();
        public static string GetName<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetName();
        public static Color GetColor<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetColor();

        public static PlayerControl GetPlayer<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetPlayer();

        public static Faction GetFaction<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetFaction();
        public static Alignment GetAlignment<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetAlignment();

        // Constructors
        protected Role() { }

        protected Role(PlayerControl player)
        {
            Player = player;
        }

        // Methods
        public static void SetRole<T>(ref List<PlayerControl> players)
            where T : RoleGeneric<T>, new()
        {
            float spawnChance = GetRoleSpawnChance<T>();
            if (spawnChance < 1 || players.Count <= 0) return;
            bool spawnChanceAchieved = Rng.Next(1, 101) <= spawnChance;
            if (!spawnChanceAchieved) return;

            int random = Rng.Next(0, players.Count);
            T role = new T();
            AddSpecialRole(role, players[random]);
            players.RemoveAt(random);

            MessageWriter writer = GetWriter(RPC.SetRole);
            writer.Write(GetRoleID<T>());
            writer.Write(role.Player.PlayerId);
            CloseWriter(writer);
        }

        public void InitializeRole(PlayerControl player)
        {
            Player = player;
            SetConfigSettings();
            if (HasSpecialButton)
            {
                SpecialButton = new RoleButton(Cooldown, PerformAction, CanUse, OnMeetingEnds, SpecialButtonSprite,
                    ButtonOffset);
            }

            InitializeRoleInternal();
            SetRoleDescription();
        }

        public void SetNameColor()
        {
            if (MeetingHud.Instance != null)
            {
                foreach (PlayerVoteArea playerVote in MeetingHud.Instance.playerStates)
                {
                    if (Player.PlayerId == playerVote.TargetPlayerId)
                    {
                        playerVote.NameText.Color = Color;
                    }
                }
            } else
            {
                Player.nameText.Color = Color;
            }
        }

        public void SetRoleDescription()
        {
            if (Player != PlayerControl.LocalPlayer) return;

            if (Player.Data.IsImpostor)
            {
                Player.myTasks.RemoveAt(0);
            }

            var roleDescription = new GameObject("roleDescription").AddComponent<ImportantTextTask>();
            roleDescription.transform.SetParent(Player.transform, false);
            roleDescription.Text = $"{ColorizedText(Name, Color)}: {StartText}";
            Player.myTasks.Insert(0, roleDescription);
        }

        public void UpdateButton()
        {
            if (!HasSpecialButton || SpecialButton == null) return;
            if (NeedsTarget)
            {
                // CurrentTarget = PlayerTools.FindClosestTarget(Player);
                SpecialButton.Target = PlayerTools.FindClosestTarget(Player);
            }

            SpecialButton.HudUpdate();
        }

        public bool PerformAction()
        {
            ConsoleTools.Info("Perform Action Internal");
            if (NeedsTarget && SpecialButton.Target == null) return false;
            if (!PerformActionInternal()) return false;

            CurrentDuration = Duration;
            return true;
        }

        public string EjectMessage(string playerName) => $"{playerName} was the {ColorizedText(Name, Color)}";

        public void ClearSettings()
        {
            Player = null;
            CurrentDuration = 0F;
            // SpecialButton.Destroy();
            SpecialButton = null;
            ClearSettingsInternal();
        }

        public void StartDuration()
        {
            CurrentDuration = Duration;
        }

        public void EndDuration()
        {
            CurrentDuration = 0F;
        }

        // Virtual Methods
        protected virtual void SetConfigSettings() { }

        public virtual void SetIntro(IntroCutscene.CoBegin__d instance)
        {
            instance.__this.Title.Text = Name;
            instance.__this.Title.render?.material.SetColor(ShaderOutlineColor, OutlineColor);
            instance.__this.Title.transform.localScale = TitleScale;
            instance.c = Color;
            instance.__this.ImpostorText.Text = StartText;
            instance.__this.BackgroundBar.material.color = Color;
        }

        protected virtual void InitializeRoleInternal() { }

        public virtual void UpdateDuration(float deltaTime)
        {
            CurrentDuration = Mathf.Max(0F, CurrentDuration - deltaTime);
        }

        protected virtual bool PerformActionInternal()
        {
            return false;
        }

        protected virtual void ClearSettingsInternal() { }
    }
}