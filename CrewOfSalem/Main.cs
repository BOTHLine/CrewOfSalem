using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.Options;
using CrewOfSalem.Roles;
using System.Collections.Generic;
using System;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Main : BasePlugin
    {
        private const string  Id      = "gg.reactor.crewofsalem";
        public const  string  Name    = "Crew Of Salem";
        public const  string  Version = "0.1";
        private       Harmony Harmony { get; } = new Harmony(Id);

        //This section uses the https://github.com/DorCoMaNdO/Reactor-Essentials framework

        // General Game Options
        public static CustomStringOption OptionShowPlayerNames =
            CustomOption.AddString("Show Owner Names", new[] {"Always", "Line of Sight", "Never"});

        public static readonly Role[] Roles =
        {
            new Investigator(),
            new Psychic(),
            new Sheriff(),
            new Spy(),

            new Veteran(),
            new Vigilante(),

            new Bodyguard(),
            new Doctor(),

            new Escort(),

            new Disguiser(),

            // new Ambusher(),
            new Forger(),
            new Mafioso(),

            new Consigliere(),
            new Consort(),

            new Survivor(),

            new Executioner(),
            new Jester()
        };

        public static readonly RoleSlot[] RoleSlots =
        {
            new RoleSlot(Faction.Crew,    Alignment.Investigative),
            new RoleSlot(Faction.Crew,    Alignment.Investigative),
            new RoleSlot(Faction.Crew,    Alignment.Support),
            new RoleSlot(Faction.Mafia,   Alignment.Killing),
            new RoleSlot(Faction.Neutral, Alignment.Evil),
            new RoleSlot(Faction.Crew,    Alignment.Investigative),
            new RoleSlot(Faction.Neutral, Alignment.Benign),
            new RoleSlot(Faction.Mafia,   Alignment.Killing),
            new RoleSlot(Faction.Crew,    Alignment.Killing),
            new RoleSlot(Faction.Crew)
        };

        // SpawnChance Options
        private static readonly DictionaryKVP<Type, CustomNumberOption> RoleSpawnChances =
            new DictionaryKVP<Type, CustomNumberOption>
            {
                /*
                CreateRoleSpawnChanceOption<Investigator>(),
                // Lookout
                CreateRoleSpawnChanceOption<Psychic>(),
                CreateRoleSpawnChanceOption<Sheriff>(),
                CreateRoleSpawnChanceOption<Spy>(),
                CreateRoleSpawnChanceOption<Tracker>(),

                // Jailor
                // Vampire Hunter
                CreateRoleSpawnChanceOption<Veteran>(),
                CreateRoleSpawnChanceOption<Vigilante>(),

                // Bodyguard
                CreateRoleSpawnChanceOption<Doctor>(),
                // Crusader
                // Trapper

                CreateRoleSpawnChanceOption<Escort>(),
                // Mayor
                // Medium
                // Retributionist
                // Transporter

                CreateRoleSpawnChanceOption<Disguiser>(),
                // Forger
                // Framer
                // Hypnotist
                // Janitor

                // Ambusher
                // Godfather
                CreateRoleSpawnChanceOption<Mafioso>(),

                // Blackmailer
                CreateRoleSpawnChanceOption<Consigliere>(),
                CreateRoleSpawnChanceOption<Consort>(),

                // Amnesiac
                // GuardianAngel
                CreateRoleSpawnChanceOption<Survivor>(),

                CreateRoleSpawnChanceOption<Executioner>(),
                CreateRoleSpawnChanceOption<Jester>()
                // Witch

                // Arsonist
                // SerialKiller
                // Werewolf
                */
            };

        // Cooldown Options
        private static readonly DictionaryKVP<Type, CustomNumberOption> RoleCooldowns =
            new DictionaryKVP<Type, CustomNumberOption>
            {
                CreateRoleCooldownOption<Investigator>("Investigate"),
                CreateRoleCooldownOption<Spy>("Spy"),

                CreateRoleCooldownOption<Veteran>("Alert"),
                CreateRoleCooldownOption<Vigilante>("Kill"),

                CreateRoleCooldownOption<Doctor>("Shield"),

                CreateRoleCooldownOption<Escort>("Block"),

                CreateRoleCooldownOption<Disguiser>("Disguise"),

                CreateRoleCooldownOption<Ambusher>("Kill"),
                CreateRoleCooldownOption<Forger>("Forge"),
                CreateRoleCooldownOption<Mafioso>("Kill"),

                CreateRoleCooldownOption<Consigliere>("Investigate"),
                CreateRoleCooldownOption<Consort>("Block"),

                CreateRoleCooldownOption<Survivor>("Vest")
            };

        // private static readonly DictionaryKVP<Type, string> ActionNames = new DictionaryKVP<Type, string>();

        // Duration Options
        public static readonly DictionaryKVP<Type, CustomNumberOption> RoleDurations =
            new DictionaryKVP<Type, CustomNumberOption>
            {
                CreateRoleDurationOption<Spy>("Spy"),

                CreateRoleDurationOption<Veteran>("Alert"),

                CreateRoleDurationOption<Doctor>("Shield"),

                CreateRoleDurationOption<Escort>("Block"),

                CreateRoleDurationOption<Disguiser>("Disguise"),

                CreateRoleDurationOption<Forger>("Forge"),

                CreateRoleDurationOption<Consort>("Block"),

                CreateRoleDurationOption<Survivor>("Vest")
            };

        // Additional Options
        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer =
            CustomOption.AddString(Role.GetName<Doctor>() + ": Show Shielded Owner",
                new[] {"Doctor", "Target", "Doctor & Target", "Everyone"});

        public static readonly CustomNumberOption OptionForgeCooldown =
            CustomOption.AddNumber("Forger Cooldown", 30F, 10F, 60F, 2.5F);

        public static readonly CustomNumberOption OptionForgeDuration =
            CustomOption.AddNumber("Forger Duration", 30F, 10F, 60F, 2.5F);

        public override void Load()
        {
            // TODO: Add Assets?

            // Disable the https://github.com/DorCoMaNdO/Reactor-Essentials watermark.
            // The code said that you were allowed, as long as you provided credit elsewhere. 
            // I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
            // If the owner of this library has any problems with this, just message me on discord and we'll find a solution
            // BothLine#9610
            CustomOption.ShamelessPlug = true;
            Harmony.PatchAll();
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleSpawnChanceOption<T>(float value = 50F,
            float min = 0F, float max = 100F, float increment = 5F)
            where T : RoleGeneric<T>, new()
        {
            Type type = typeof(T);
            string name = /*ColorizedText(*/Role.GetName<T>() /*, Role.GetColor<T>())*/;
            string faction = Role.GetFaction<T>().ShortHandle;
            string alignment = Role.GetAlignment<T>().ShortHandle;
            CustomNumberOption customNumberOption =
                CustomOption.AddNumber($"({faction}{alignment}) {name}", value, min, max, increment);
            return new KeyValuePair<Type, CustomNumberOption>(type, customNumberOption);
        }

        public static float GetRoleSpawnChance<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleSpawnChances.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleCooldownOption<T>(
            string actionName, float value = 30F, float min = 10F, float max = 60F, float increment = 2.5F)
            where T : RoleGeneric<T>, new()
        {
            // ActionNames.Add(typeof(T), actionName);
            return new KeyValuePair<Type, CustomNumberOption>(typeof(T),
                CustomOption.AddNumber(
                    $"{ /*ColorizedText(*/Role.GetName<T>() /*,Role.GetColor<T>())*/} {actionName} Cooldown", value,
                    min, max, increment));
        }

        public static float GetRoleCooldown<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleCooldowns.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }

        /*
        public static string GetActionName<T>()
            where T : RoleGeneric<T>, new()
        {
            return ActionNames.TryGetValue(typeof(T), out string actionName) ? actionName : "";
        }
        */

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleDurationOption<T>(string actionName,
            float value = 15F, float min = 5F, float max = 30F, float increment = 2.5F)
            where T : RoleGeneric<T>, new()
        {
            return new KeyValuePair<Type, CustomNumberOption>(typeof(T),
                CustomOption.AddNumber(
                    $"{ /*ColorizedText(*/Role.GetName<T>() /*,Role.GetColor<T>())*/} {actionName} Duration", value,
                    min, max, increment));
        }

        public static float GetRoleDuration<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleDurations.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }
    }
}