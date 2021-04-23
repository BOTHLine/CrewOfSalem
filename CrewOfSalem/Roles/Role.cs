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

        private bool hasSetTasks = false;

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

        public void SetRoleTask()
        {
            if (Owner.myTasks.Count <= 1) return;
            if (hasSetTasks) return;
            if (Owner != LocalPlayer) return;

            hasSetTasks = true;

            Owner.myTasks.Remove(Owner.myTasks.ToArray()
               .FirstOrDefault(task => task.GetComponent<ImportantTextTask>() != null));

            var roleDescription = new GameObject("roleTask").AddComponent<ImportantTextTask>();
            roleDescription.transform.SetParent(Owner.transform, false);
            roleDescription.Text = $"<color=\"white\">{ColorizedText(Name, Color)}: {RoleTask}</color>";
            Owner.myTasks.Insert(0, roleDescription);
        }

        public string EjectMessage(string playerName) => $"{playerName} was the {ColorizedText(Name, Color)}";

        public void ClearSettings()
        {
            Owner = null;
            hasSetTasks = false;
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

        public void RemoveAbility(Ability ability)
        {
            abilities.Remove(ability);

            RefreshAbilityOffsets();
        }

        private void RefreshAbilityOffsets()
        {
            for (var i = 0; i < abilities.Count; i++)
            {
                abilities[i].Offset = Vector3.left * (i + 1);
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
            SetRoleTask();

            foreach (Ability ability in abilities)
            {
                ability.Update(deltaTime);
            }
        }

        protected virtual void SetConfigSettingsInternal() { }

        public virtual void SetIntro(IntroCutscene.Nested_0 instance)
        {
            instance.__this.Title.text = Name;
            instance.__this.Title.color = Color;
            instance.__this.Title.renderer?.material.SetColor(ShaderOutlineColor, OutlineColor);
            instance.__this.Title.transform.localScale = TitleScale;
            instance._c_5__2 = Color;        // TODO Which to use?
            instance._impColor_5__4 = Color; // TODO Which to use?
            instance.__this.ImpostorText.text = RoleTask;
            instance.__this.BackgroundBar.material.color = Color;
        }

        protected virtual void InitializeRoleInternal() { }

        protected virtual void ClearSettingsInternal() { }
    }
}