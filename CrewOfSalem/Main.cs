using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.CustomOptions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using CrewOfSalem.Roles.Alignments;
using System.Collections.Generic;
using System;

namespace CrewOfSalem
{
    [BepInPlugin(Id, name, version)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Main : BasePlugin
    {
        public const string Id = "gg.reactor.crewofsalem";
        public const string name = "Crew Of Salem";
        public const string version = "0.1";
        public Harmony Harmony { get; } = new Harmony(Id);

        //This section uses the https://github.com/DorCoMaNdO/Reactor-Essentials framework

        // General Game Options
        public static CustomStringOption OptionShowPlayerNames = CustomOption.AddString("Show Player Names", new[] { "Always", "Line of Sight", "Never" });

        // SpawnChance Options
        public static readonly DictionaryKVP<Type, CustomNumberOption> RoleSpawnChances = new DictionaryKVP<Type, CustomNumberOption>
        {
            CreateRoleSpawnChanceOption<Investigator>(),
            CreateRoleSpawnChanceOption<Sheriff>(),
            CreateRoleSpawnChanceOption<Spy>(),
            CreateRoleSpawnChanceOption<Tracker>(),

            CreateRoleSpawnChanceOption<Veteran>(),
            CreateRoleSpawnChanceOption<Vigilante>(),

            CreateRoleSpawnChanceOption<Doctor>(),

            CreateRoleSpawnChanceOption<Escort>(),

            CreateRoleSpawnChanceOption<Jester>()
        };

        // Cooldown Options
        public static readonly DictionaryKVP<Type, CustomNumberOption> RoleCooldowns = new DictionaryKVP<Type, CustomNumberOption>
        {
            CreateRoleCooldownOption<Investigator>(),

            CreateRoleCooldownOption<Veteran>(),
            CreateRoleCooldownOption<Vigilante>(),

            CreateRoleCooldownOption<Doctor>(),
            CreateRoleCooldownOption<Escort>()
        };

        //
        public static readonly DictionaryKVP<Type, CustomNumberOption> RoleDurations = new DictionaryKVP<Type, CustomNumberOption>
        {
            CreateRoleDurationOption<Veteran>("Alert"),
            CreateRoleDurationOption<Doctor>("Shield"),
            CreateRoleDurationOption<Escort>("Block")
        };

        // Additional Options
        // Crew
        // Crew Killing

        // Crew Protective
        // Doctor
        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer = CustomOption.AddString(Role.GetName<Doctor>() + ": Show Shielded Player", new[] { "Doctor", "Target", "Doctor & Target", "Everyone" });

        // Crew Support


        // Neutral
        // Neutral Evil

        public override void Load()
        {
            // TODO:Add Assets?

            //Disable the https://github.com/DorCoMaNdO/Reactor-Essentials watermark.
            //The code said that you were allowed, as long as you provided credit elsewhere. 
            //I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
            //If the owner of this library has any problems with this, just message me on discord and we'll find a solution
            //BothLine#9610

            CustomOption.ShamelessPlug = false;

            Harmony.PatchAll();
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleSpawnChanceOption<T>(float value = 50F, float min = 0F, float max = 100F, float increment = 5F)
            where T : RoleGeneric<T>, new()
        {
            Type type = typeof(T);

            string name = Role.GetName<T>();
            Faction faction = Role.GetFaction<T>();
            Alignment alignment = Role.GetAlignment<T>();
            CustomNumberOption customNumberOption = CustomOption.AddNumber("(" + faction.ShortHandle + alignment.ShortHandle + ") " + name, value, min, max, increment);

            return new KeyValuePair<Type, CustomNumberOption>(type, customNumberOption);
        }

        public static float GetRoleSpawnChance<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleSpawnChances.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleCooldownOption<T>(float value = 30F, float min = 10F, float max = 60F, float increment = 2.5F)
            where T : RoleGeneric<T>, new()
        {
            return new KeyValuePair<Type, CustomNumberOption>(typeof(T), CustomOption.AddNumber(Role.GetName<T>() + ": Cooldown", value, min, max, increment));
        }

        public static float GetRoleCooldown<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleCooldowns.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleDurationOption<T>(string actionName, float value = 15F, float min = 5F, float max = -1F, float increment = 2.5F)
            where T : RoleGeneric<T>, new()
        {
            if (max < 0F) max = GetRoleCooldown<T>();
            return new KeyValuePair<Type, CustomNumberOption>(typeof(T), CustomOption.AddNumber(Role.GetName<T>() + " " + actionName + ": Duration", value, min, max, increment));
        }

        public static float GetRoleDuration<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleDurations.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }
    }
}