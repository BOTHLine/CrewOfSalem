using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.Options;
using CrewOfSalem.Roles;
using System.Collections.Generic;
using System;
using CrewOfSalem.Roles.Abilities;
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
        public static readonly CustomStringOption OptionShowPlayerNames =
            CustomOption.AddString("Show Player Names", new[] {"Always", "Line of Sight", "Never"});

        public static readonly Role[] Roles =
        {
            RoleGeneric<Investigator>.Instance, // 0
            RoleGeneric<Lookout>.Instance,      // 1
            RoleGeneric<Psychic>.Instance,      // 2
            RoleGeneric<Sheriff>.Instance,      // 3
            RoleGeneric<Spy>.Instance,          // 4
            RoleGeneric<Tracker>.Instance,      // 5

            RoleGeneric<Jailor>.Instance,        // 6
            RoleGeneric<VampireHunter>.Instance, // 7
            RoleGeneric<Veteran>.Instance,       // 8
            RoleGeneric<Vigilante>.Instance,     // 9

            RoleGeneric<Bodyguard>.Instance, // 10
            RoleGeneric<Doctor>.Instance,    // 11
            RoleGeneric<Crusader>.Instance,  // 12
            RoleGeneric<Trapper>.Instance,   // 13

            RoleGeneric<Escort>.Instance,         // 14
            RoleGeneric<Mayor>.Instance,          // 15
            RoleGeneric<Medium>.Instance,         // 16
            RoleGeneric<Retributionist>.Instance, // 17
            RoleGeneric<Transporter>.Instance,    // 18

            RoleGeneric<Disguiser>.Instance, // 19
            RoleGeneric<Framer>.Instance,    // 20
            RoleGeneric<Hypnotist>.Instance, // 21
            RoleGeneric<Janitor>.Instance,   // 22

            RoleGeneric<Ambusher>.Instance,  // 23
            RoleGeneric<Forger>.Instance,    // 24
            RoleGeneric<Godfather>.Instance, // 25
            RoleGeneric<Mafioso>.Instance,   // 26

            RoleGeneric<Blackmailer>.Instance, // 27
            RoleGeneric<Consigliere>.Instance, // 28
            RoleGeneric<Consort>.Instance,     // 29

            RoleGeneric<Amnesiac>.Instance,      // 30
            RoleGeneric<GuardianAngel>.Instance, // 31
            RoleGeneric<Survivor>.Instance,      // 32

            RoleGeneric<Vampire>.Instance, // 33

            RoleGeneric<Executioner>.Instance, // 34
            RoleGeneric<Jester>.Instance,      // 35
            RoleGeneric<Witch>.Instance,       // 36

            RoleGeneric<Arsonist>.Instance,     // 37
            RoleGeneric<SerialKiller>.Instance, // 38
            RoleGeneric<Werewolf>.Instance      // 39
        };

        public static readonly RoleSlot[] RoleSlots =
        {
            new RoleSlot(Faction.Crew),
            new RoleSlot(Faction.Crew),
            new RoleSlot(Faction.Crew),
            new RoleSlot(Faction.Mafia,   Alignment.Killing),
            new RoleSlot(Faction.Neutral, Alignment.Evil),
            new RoleSlot(Faction.Crew),
            new RoleSlot(Faction.Neutral, Alignment.Benign),
            new RoleSlot(Faction.Mafia),
            new RoleSlot(Faction.Crew),
            new RoleSlot(Faction.Mafia)
        };

        // AllowSpawn Options
        private static readonly DictionaryKVP<Type, CustomToggleOption> RoleAllowSpawns =
            new DictionaryKVP<Type, CustomToggleOption>();


        private static KeyValuePair<Type, CustomToggleOption> GetRoleAllowSpawnOption(Role role)
        {
            return new KeyValuePair<Type, CustomToggleOption>(role.GetType(), CustomOption.AddToggle(role.Name, true));
        }

        // Cooldown Options
        private static readonly DictionaryKVP<TypePair, CustomNumberOption> RoleCooldowns =
            new DictionaryKVP<TypePair, CustomNumberOption>
            {
                CreateRoleCooldownOption<Investigator, AbilityInvestigate>(),
                // CreateRoleCooldownOption<Spy>(),

                CreateRoleCooldownOption<Veteran, AbilityAlert>(),
                CreateRoleCooldownOption<Vigilante, AbilityKill>(),

                CreateRoleCooldownOption<Doctor, AbilityShield>(),

                CreateRoleCooldownOption<Escort, AbilityBlock>(),

                CreateRoleCooldownOption<Disguiser, AbilityDisguise>(),

                CreateRoleCooldownOption<Ambusher, AbilityKill>(),
                CreateRoleCooldownOption<Forger, AbilityKill>(),
                CreateRoleCooldownOption<Forger, AbilityForge>(),
                CreateRoleCooldownOption<Mafioso, AbilityKill>(),

                CreateRoleCooldownOption<Consigliere, AbilityCheckRole>(),
                CreateRoleCooldownOption<Consort, AbilityBlock>(),

                CreateRoleCooldownOption<Survivor, AbilityVest>()
            };

        // private static readonly DictionaryKVP<Type, string> ActionNames = new DictionaryKVP<Type, string>();

        // Duration Options
        public static readonly DictionaryKVP<TypePair, CustomNumberOption> RoleDurations =
            new DictionaryKVP<TypePair, CustomNumberOption>
            {
                // CreateRoleDurationOption<Spy>("Spy"),

                CreateRoleDurationOption<Veteran, AbilityAlert>(),

                CreateRoleDurationOption<Doctor, AbilityShield>(),

                CreateRoleDurationOption<Escort, AbilityBlock>(),

                CreateRoleDurationOption<Disguiser, AbilityDisguise>(),

                CreateRoleDurationOption<Forger, AbilityForge>(),

                CreateRoleDurationOption<Consort, AbilityBlock>(),

                CreateRoleDurationOption<Survivor, AbilityVest>()
            };

        // Additional Options
        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer =
            CustomOption.AddString(Role.GetName<Doctor>() + ": Show Shielded Owner",
                new[] {"Doctor", "Target", "Doctor & Target", "Everyone"});

        private static readonly List<IEnumerable<CustomOption>>
            AllCustomOptions = new List<IEnumerable<CustomOption>>();

        private static int customPageIndex;

        public static void TurnPage()
        {
            foreach (CustomOption option in AllCustomOptions[customPageIndex])
            {
                option.HudVisible = option.MenuVisible = false;
            }

            customPageIndex = ++customPageIndex % AllCustomOptions.Count;

            foreach (CustomOption option in AllCustomOptions[customPageIndex])
            {
                option.HudVisible = option.MenuVisible = true;
            }
        }

        public override void Load()
        {
            foreach (Role role in Roles)
            {
                // RoleAllowSpawns.Add(GetRoleAllowSpawnOption(role));
                // RoleSpawnChances.Add(CreateRoleSpawnChanceOption(role));
            }

            AllCustomOptions.Add(new[] {OptionShowPlayerNames, OptionDoctorShowShieldedPlayer});
            AllCustomOptions.Add(RoleCooldowns.Values);
            AllCustomOptions.Add(RoleDurations.Values);

            var doSkip = true;
            foreach (IEnumerable<CustomOption> customOptions in AllCustomOptions)
            {
                if (doSkip)
                {
                    doSkip = false;
                    continue;
                }

                foreach (CustomOption option in customOptions)
                {
                    option.HudVisible = option.MenuVisible = false;
                }
            }

            // new OptionPage(new[] {OptionShowPlayerNames, OptionDoctorShowShieldedPlayer});
            // new OptionPage(RoleSpawnChances.Values);
            // new OptionPage(RoleCooldowns.Values);
            // new OptionPage(RoleDurations.Values);

            // TODO: Add Assets?

            // Disable the https://github.com/DorCoMaNdO/Reactor-Essentials watermark.
            // The code said that you were allowed, as long as you provided credit elsewhere. 
            // I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
            // If the owner of this library has any problems with this, just message me on discord and we'll find a solution
            // BothLine#9610
            CustomOption.ShamelessPlug = false;
            Harmony.PatchAll();
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleSpawnChanceOption<T>(float value = 50F,
            float min = 0F, float max = 100F, float increment = 5F)
            where T : RoleGeneric<T>, new()
        {
            Type type = typeof(T);
            string name = Role.GetName<T>();
            string faction = Role.GetFaction<T>().ShortHandle;
            string alignment = Role.GetAlignment<T>().ShortHandle;
            CustomNumberOption customNumberOption =
                CustomOption.AddNumber($"({faction}{alignment}) {name}", value, min, max, increment);
            return new KeyValuePair<Type, CustomNumberOption>(type, customNumberOption);
        }

        private static KeyValuePair<Type, CustomNumberOption> CreateRoleSpawnChanceOption(Role role, float value = 100F,
            float min = 0F, float max = 100F, float increment = 5F)
        {
            CustomNumberOption customNumberOption = CustomOption.AddNumber(
                $"({role.Faction.Name} {role.Alignment.Name}) {role.Name}", value, min, max, increment);
            return new KeyValuePair<Type, CustomNumberOption>(role.GetType(), customNumberOption);
        }

        /*
        public static float GetRoleSpawnChance<T>()
            where T : RoleGeneric<T>, new()
        {
            return RoleSpawnChances.TryGetValue(typeof(T), out CustomNumberOption value) ? value.GetValue() : 0F;
        }
        */

        private static KeyValuePair<TypePair, CustomNumberOption> CreateRoleCooldownOption<TRole, TAbility>(
            float value = 30F, float min = 10F, float max = 60F, float increment = 2.5F)
            where TRole : RoleGeneric<TRole>, new()
            where TAbility : Ability
        {
            // ActionNames.Add(typeof(T), actionName);
            return new KeyValuePair<TypePair, CustomNumberOption>(
                new TypePair(typeof(TRole), typeof(TAbility)),
                CustomOption.AddNumber($"{Role.GetName<TRole>()} {typeof(TAbility).Name} Cooldown", value, min, max,
                    increment));
        }

        public static float GetRoleCooldown<TRole, TAbility>()
            where TRole : RoleGeneric<TRole>, new()
            where TAbility : Ability
        {
            return RoleCooldowns.TryGetValue(new TypePair(typeof(TRole), typeof(TAbility)),
                out CustomNumberOption value)
                ? value.GetValue()
                : 0F;
        }


        // public static string GetActionName<T>()
        //     where T : RoleGeneric<T>, new()
        // {
        //     return ActionNames.TryGetValue(typeof(T), out string actionName) ? actionName : "";
        // }


        private static KeyValuePair<TypePair, CustomNumberOption> CreateRoleDurationOption<TRole, TAbility>(
            float value = 15F, float min = 5F, float max = 30F, float increment = 2.5F)
            where TRole : RoleGeneric<TRole>, new()
            where TAbility : Ability
        {
            return new KeyValuePair<TypePair, CustomNumberOption>(
                new TypePair(typeof(TRole), typeof(TAbility)),
                CustomOption.AddNumber($"{Role.GetName<TRole>()} {typeof(TAbility).Name} Duration", value, min, max,
                    increment));
        }

        public static float GetRoleDuration<TRole, TAbility>()
            where TRole : RoleGeneric<TRole>, new()
            where TAbility : Ability
        {
            return RoleDurations.TryGetValue(new TypePair(typeof(TRole), typeof(TAbility)),
                out CustomNumberOption value)
                ? value.GetValue()
                : 0F;
        }

        public struct TypePair
        {
            public Type type1;
            public Type type2;

            public TypePair(Type type1, Type type2)
            {
                this.type1 = type1;
                this.type2 = type2;
            }
        }
    }
}