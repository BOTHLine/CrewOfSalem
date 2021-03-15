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

        // Additional Options
        // Crew

        // Crew Killing
        // Veteran
        public static readonly CustomNumberOption OptionVeteranAlertDuration = CustomOption.AddNumber(nameof(Veteran) + ": Alert Duration", 15F, 5F, GetRoleCooldown<Veteran>(), 2.5F);

        // Crew Protective
        // Doctor
        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer = CustomOption.AddString(nameof(Doctor) + ": Show Shielded Player", new[] { "Doctor", "Target", "Doctor & Target", "Everyone" });
        public static readonly CustomNumberOption OptionDoctorShieldDuration = CustomOption.AddNumber(nameof(Doctor) + ": Shield Duration", 15F, 5F, GetRoleCooldown<Doctor>(), 2.5F);

        // Crew Support
        // Escort
        public static readonly CustomNumberOption OptionEscortCooldownIncrease = CustomOption.AddNumber(nameof(Escort) + ": Cooldown Increase", 30F, 10F, 60F, 2.5F);


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

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleSpawnChanceOption<T>()
            where T : RoleGeneric<T>, new()
        {
            Type type = typeof(T);

            string name = Role.GetName<T>();
            Faction faction = Role.GetFaction<T>();
            Alignment alignment = Role.GetAlignment<T>();
            CustomNumberOption customNumberOption = CustomOption.AddNumber("(" + faction.shortHandle + alignment.shortHandle + ") " + name, 50F, 0F, 100F, 5F);

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
            return new KeyValuePair<Type, CustomNumberOption>(typeof(T), CustomOption.AddNumber(typeof(T).Name + ": Cooldown", value, min, max, increment));
        }

        public static float GetRoleCooldown<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleCooldowns.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }
    }
}