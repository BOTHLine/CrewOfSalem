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
        public abstract byte RoleID { get; }
        public abstract string Name { get; }

        public abstract Faction Faction { get; }
        public abstract Alignment Alignment { get; }

        public virtual Color Color => Faction.Color;
        public virtual Color OutlineColor { get; } = new Color(0, 0, 0, 1);

        protected virtual string StartText => Alignment.GetColorizedTask(Faction);
        protected virtual Vector3 TitleScale { get; } = new Vector3(1, 1, 1);

        public abstract bool HasSpecialButton { get; }
        public abstract Sprite SpecialButton { get; }

        public float Cooldown { get; protected set; }
        public float Duration { get; protected set; }
        public float CurrentDuration { get; set; }

        public PlayerControl Player { get; protected set; }

        // Methods
        public static byte GetRoleID<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetRoleID();
        public static string GetName<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetName();

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
        public static void SetRole<T>(ref List<PlayerControl> crew)
            where T : RoleGeneric<T>, new()
        {
            float spawnChance = GetRoleSpawnChance<T>();
            if (spawnChance < 1 || crew.Count <= 0) return;
            bool spawnChanceAchieved = Rng.Next(1, 101) <= spawnChance;
            if (!spawnChanceAchieved) return;

            int random = Rng.Next(0, crew.Count);
            T role = new T();
            role.InitializeRoleInternal(crew[random]);
            AddSpecialRole(role);
            crew.RemoveAt(random);

            MessageWriter writer = GetWriter(RPC.SetRole);
            writer.Write(GetRoleID<T>());
            writer.Write(role.Player.PlayerId);
            CloseWriter(writer);
        }

        public void InitializeRoleInternal(PlayerControl player)
        {
            Player = player;
            SetConfigSettings();
            Player.SetKillTimer(Cooldown);
            SetRoleDescription();
            InitializeRole();
        }

        public void SetNameColor()
        {
            if (MeetingHud.Instance != null) {
                foreach (PlayerVoteArea playerVote in MeetingHud.Instance.playerStates) {
                    if (Player.PlayerId == playerVote.TargetPlayerId) {
                        playerVote.NameText.Color = Color;
                    }
                }
            } else {
                Player.nameText.Color = Color;
            }
        }

        public void SetRoleDescription()
        {
            ImportantTextTask roleDescription = new GameObject("roleDescription").AddComponent<ImportantTextTask>();
            roleDescription.transform.SetParent(Player.transform, false);
            roleDescription.Text = $"{ColorizedText(Name, Color)}: {StartText}";
            Player.myTasks.Insert(0, roleDescription);
        }

        public string EjectMessage(string playerName) => $"{playerName} was the {ColorizedText(Name, Color)}";

        public void ClearSettingsInternal()
        {
            Player = null;
            CurrentDuration = 0F;
            ClearSettings();
        }

        // Virtual Methods
        protected virtual void SetConfigSettings() { }

        public virtual void SetIntro(IntroCutscene.CoBegin__d instance)
        {
            instance.__this.Title.Text = Name;
            instance.__this.Title.render?.material?.SetColor("_OutlineColor", OutlineColor);
            instance.__this.Title.transform.localScale = TitleScale;
            instance.c = Color;
            instance.__this.ImpostorText.Text = StartText;
            instance.__this.BackgroundBar.material.color = Color;
        }

        protected virtual void InitializeRole()
        {
            Player.SetKillTimer(Cooldown);
        }

        public virtual void CheckDead(HudManager instance)
        {
            if (!Player.Data.IsDead) return;

            KillButtonManager killButton = instance.KillButton;
            killButton.gameObject.SetActive(false);
            killButton.renderer.enabled = false;
        }

        public virtual void CheckSpecialButton(HudManager instance)
        {
            if (!HasSpecialButton) return;

            if (instance.UseButton == null || !instance.UseButton.isActiveAndEnabled || Player.Data.IsDead) return;

            KillButtonManager killButton = instance.KillButton;
            killButton.gameObject.SetActive(true);
            killButton.renderer.enabled = true;
            killButton.isActive = true;
            killButton.renderer.sprite = SpecialButton;
            killButton.SetTarget(PlayerTools.FindClosestTarget(Player));
        }

        public virtual void UpdateCooldown(float deltaTime)
        {
            Player.SetKillTimer(Mathf.Max(0F, Player.killTimer - deltaTime));
            CurrentDuration = Mathf.Max(0F, CurrentDuration - deltaTime);
        }

        public virtual void PerformAction(KillButtonManager instance) { }

        protected virtual void ClearSettings() { }
    }
}