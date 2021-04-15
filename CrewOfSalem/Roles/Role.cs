using System;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Abilities;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public abstract class Role
    {
        // Fields
        private readonly List<Ability> abilities = new List<Ability>();

        // Properties
        public abstract byte   RoleID { get; }
        public abstract string Name   { get; }

        public abstract Faction   Faction   { get; }
        public abstract Alignment Alignment { get; }

        public virtual    Color Color        => Faction.Color;
        protected virtual Color OutlineColor { get; } = new Color(0, 0, 0, 1);

        public virtual    string  RoleTask   => Alignment.GetColorizedTask(Faction);
        protected virtual Vector3 TitleScale { get; } = new Vector3(1, 1, 1);

        public abstract string Description { get; }

        public PlayerControl Owner { get; set; }

        // Methods
        private static byte GetRoleID<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetRoleID();
        public static string GetName<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetName();
        public static Color GetColor<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetColor();

        public static PlayerControl GetPlayer<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetPlayer();

        public static Faction GetFaction<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetFaction();
        public static Alignment GetAlignment<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetAlignment();

        public static string GetRoleTask<T>() where T : RoleGeneric<T>, new() => RoleGeneric<T>.GetRoleTask();

        // Constructors
        protected Role() { }

        protected Role(PlayerControl owner)
        {
            Owner = owner;
        }

        // Methods
        public static void RpcSetRole(Role role, PlayerControl player)
        {
            if (AmongUsClient.Instance.AmClient) SetRole(role, player);
            WriteRPC(RPC.SetRole, role.RoleID, player.PlayerId);
        }

        public static void SetRole(Role role, PlayerControl player)
        {
            AddRole(role, player);
        }

        public void InitializeRole()
        {
            InitializeRoleInternal();
            SetConfigSettingsInternal();
            InitializeAbilities();
        }

        public void SetMeetingHudNameColor()
        {
            if (MeetingHud.Instance != null)
            {
                foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
                {
                    if (Owner.PlayerId == playerVoteArea.TargetPlayerId)
                    {
                        playerVoteArea.NameText.Color = Color;
                    }
                }
            } else
            {
                Owner.nameText.Color = Color;
            }
        }

        public void SetMeetingHudRoleName()
        {
            if (MeetingHud.Instance != null)
            {
                foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
                {
                    if (Owner.PlayerId == playerVoteArea.TargetPlayerId)
                    {
                        playerVoteArea.NameText.Text = $"{Owner.Data.PlayerName}\n({Name})";
                        playerVoteArea.NameText.scale = 0.8F;
                    }
                }
            }
        }

        public void SetRoleTask()
        {
            if (Owner != PlayerControl.LocalPlayer) return;

            if (Owner.Data.IsImpostor && Owner.myTasks.Count > 0)
            {
                Owner.myTasks.RemoveAt(0);
            }

            var roleDescription = new GameObject("roleTask").AddComponent<ImportantTextTask>();
            roleDescription.transform.SetParent(Owner.transform, false);
            roleDescription.Text = $"{ColorizedText(Name, Color)}: {RoleTask}";
            Owner.myTasks.Insert(0, roleDescription);
        }

        public string EjectMessage(string playerName) => $"{playerName} was the {ColorizedText(Name, Color)}";

        public void ClearSettings()
        {
            Owner = null;
            ClearAbilities();
            ClearSettingsInternal();
        }

        public TAbility AddAbility<TRole, TAbility>(bool insertAtFront = false)
            where TRole : RoleGeneric<TRole>, new()
            where TAbility : Ability
        {
            TAbility ability;
            if (typeof(AbilityDuration).IsAssignableFrom(typeof(TAbility)))
            {
                ability = Activator.CreateInstance(typeof(TAbility), this, Main.GetRoleCooldown<TRole, TAbility>(),
                    Main.GetRoleDuration<TRole, TAbility>()) as TAbility;
            } else
            {
                ability =
                    Activator.CreateInstance(typeof(TAbility), this, Main.GetRoleCooldown<TRole, TAbility>()) as
                        TAbility;
            }

            AddAbility(ability, insertAtFront);
            return ability;
        }

        private void AddAbility(Ability ability, bool insertAtFront)
        {
            if (insertAtFront) abilities.Insert(0, ability);
            else abilities.Add(ability);

            RefreshAbilityOffsets();
        }

        private void RefreshAbilityOffsets()
        {
            for (var i = 0; i < abilities.Count; i++)
            {
                abilities[i].Offset = Vector3.left * i;
            }
        }

        public T GetAbility<T>()
            where T : Ability
        {
            return abilities.FirstOrDefault(ability => ability is T) as T;
        }

        public IReadOnlyList<Ability> GetAllAbilities()
        {
            return abilities;
        }

        public void ClearAbilities()
        {
            foreach (Ability ability in abilities)
            {
                ability.Destroy();
            }

            abilities.Clear();
        }

        // Virtual Methods
        protected abstract void InitializeAbilities();

        public void UpdateAbilities(float deltaTime)
        {
            foreach (Ability ability in abilities)
            {
                ability.Update(deltaTime);
            }
        }

        protected virtual void SetConfigSettingsInternal() { }

        public virtual void SetIntro(IntroCutscene.CoBegin__d instance)
        {
            instance.__this.Title.Text = Name;
            instance.__this.Title.Color = Color;
            instance.__this.Title.render?.material.SetColor(ShaderOutlineColor, OutlineColor);
            instance.__this.Title.transform.localScale = TitleScale;
            instance.c = Color;
            instance.__this.ImpostorText.Text = RoleTask;
            instance.__this.BackgroundBar.material.color = Color;
        }

        protected virtual void InitializeRoleInternal() { }

        protected virtual void ClearSettingsInternal() { }
    }
}