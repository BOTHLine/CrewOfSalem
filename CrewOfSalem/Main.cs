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
        public const  string  Version = "1.0";
        private       Harmony Harmony { get; } = new Harmony(Id);

        // General Game Options
        public static readonly CustomStringOption OptionShowPlayerNames =
            CustomOption.AddString("Show Player Names", new[] {"Always", "Line of Sight", "Never"});

        public static readonly CustomNumberOption OptionMafiaKillStart =
            CustomOption.AddNumber("Mafia Kill Abilities Start", 2F, 1F, 4F, 1F);

        public static readonly CustomNumberOption OptionMafiaKillAlways =
            CustomOption.AddNumber("Mafia Kill Abilities Always", 1F, 1F, 3F, 1F);

        public static readonly CustomStringOption OptionMafiaSharedKillCooldown =
            CustomOption.AddString("Mafia Shared Kill Cooldown", new[] {"None", "Killer", "Self", "Custom"});

        public static readonly CustomNumberOption OptionMafiaCustomSharedKillCooldown =
            CustomOption.AddNumber("Mafia Shared Custom Kill Cooldown", 10F, 0F, 30F, 2.5F);

        // public static readonly CustomToggleOption OptionEndScreenShowAllPlayers =
        //    CustomOption.AddToggle("End Screen Show All Players", false);

        public static readonly Role[] Roles =
        {
            // RoleGeneric<Investigator>.Instance, // Done
            RoleGeneric<Lookout>.Instance, // Done
            RoleGeneric<Psychic>.Instance, // Done
            RoleGeneric<Sheriff>.Instance, // Done
            RoleGeneric<Spy>.Instance,     // Done
            // RoleGeneric<Tracker>.Instance,      // TODO

            // RoleGeneric<Jailor>.Instance,        // TODO
            // RoleGeneric<VampireHunter>.Instance, // TODO
            RoleGeneric<Veteran>.Instance,   // Done
            RoleGeneric<Vigilante>.Instance, // Done

            RoleGeneric<Bodyguard>.Instance, // Done
            RoleGeneric<Doctor>.Instance,    // Done
            // RoleGeneric<Crusader>.Instance,  // TODO
            // RoleGeneric<Trapper>.Instance,   // TODO

            RoleGeneric<Escort>.Instance, // TODO: Think about when to add cooldown, block tasks and vents
            RoleGeneric<Mayor>.Instance,  // TODO
            RoleGeneric<Medium>.Instance, // TODO: Do show corpse colors?
            // RoleGeneric<Retributionist>.Instance, // TODO
            // RoleGeneric<Transporter>.Instance,    // TODO

            RoleGeneric<Disguiser>.Instance, // TODO: Do show corpse colors?
            // RoleGeneric<Framer>.Instance,    // TODO
            RoleGeneric<Hypnotist>.Instance, // TODO
            // RoleGeneric<Janitor>.Instance,   // TODO

            RoleGeneric<Ambusher>.Instance, // Done
            RoleGeneric<Forger>.Instance,   // Done
            // RoleGeneric<Godfather>.Instance, // TODO: Godfather undying?
            // RoleGeneric<Mafioso>.Instance, // TODO: Mafioso can see Godfather on map and will promote to Godfather if he dies?

            RoleGeneric<Blackmailer>.Instance, // Done
            RoleGeneric<Consigliere>.Instance, // Done
            RoleGeneric<Consort>.Instance,     // TODO: See Escort

            // RoleGeneric<Amnesiac>.Instance,      // TODO
            RoleGeneric<GuardianAngel>.Instance, // TODO
            RoleGeneric<Survivor>.Instance,      // Done

            // RoleGeneric<Vampire>.Instance, //TODO

            RoleGeneric<Executioner>.Instance, // Done
            RoleGeneric<Jester>.Instance,      // Done
            // RoleGeneric<Witch>.Instance,       // TODO

            // RoleGeneric<Arsonist>.Instance,     // TODO
            // RoleGeneric<SerialKiller>.Instance, // TODO
            // RoleGeneric<Werewolf>.Instance      // TODO
        };

        private static readonly CustomStringOption[] RoleSlots = new CustomStringOption[10];

        /*
        public static readonly RoleSlot[] RoleSlots =
        {
            new RoleSlot(GuardianAngel.Instance),
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
        */

        // AllowSpawn Options
        private static readonly DictionaryKVP<Type, CustomNumberOption> RoleSpawnChances =
            new DictionaryKVP<Type, CustomNumberOption>();

        // Cooldown Options
        private static readonly DictionaryKVP<TypePair, CustomNumberOption> RoleCooldowns =
            new DictionaryKVP<TypePair, CustomNumberOption>
            {
                CreateRoleCooldownOption<Investigator, AbilityInvestigate>(),

                CreateRoleCooldownOption<Lookout, AbilityMap>(35F),
                CreateRoleCooldownOption<Lookout, AbilitySurveillance>(35F),
                CreateRoleCooldownOption<Lookout, AbilityVitals>(35F),

                CreateRoleCooldownOption<Veteran, AbilityAlert>(27.5F),
                CreateRoleCooldownOption<Vigilante, AbilityKill>(32.5F),

                CreateRoleCooldownOption<Doctor, AbilityShield>(27.5F),

                CreateRoleCooldownOption<Escort, AbilityBlock>(27.5F),
                CreateRoleCooldownOption<Medium, AbilitySeance>(),

                CreateRoleCooldownOption<Disguiser, AbilityDisguise>(32.5F),

                CreateRoleCooldownOption<Ambusher, AbilityKill>(),
                CreateRoleCooldownOption<Forger, AbilityKill>(),
                CreateRoleCooldownOption<Forger, AbilityForge>(32.5F),
                CreateRoleCooldownOption<Mafioso, AbilityKill>(),

                CreateRoleCooldownOption<Consigliere, AbilityCheckRole>(40F),
                CreateRoleCooldownOption<Consort, AbilityBlock>(27.5F),

                CreateRoleCooldownOption<GuardianAngel, AbilityProtect>(27.5F),
                CreateRoleCooldownOption<Survivor, AbilityVest>(27.5F)
            };

        // private static readonly DictionaryKVP<Type, string> ActionNames = new DictionaryKVP<Type, string>();

        // Duration Options
        private static readonly DictionaryKVP<TypePair, CustomNumberOption> RoleDurations =
            new DictionaryKVP<TypePair, CustomNumberOption>
            {
                CreateRoleDurationOption<Veteran, AbilityAlert>(7.5F),

                CreateRoleDurationOption<Escort, AbilityBlock>(),
                CreateRoleDurationOption<Medium, AbilitySeance>(10F),

                CreateRoleDurationOption<Disguiser, AbilityDisguise>(10F),

                CreateRoleDurationOption<Forger, AbilityForge>(10F),

                CreateRoleDurationOption<Consort, AbilityBlock>(),

                CreateRoleDurationOption<GuardianAngel, AbilityProtect>(7.5F),
                CreateRoleDurationOption<Survivor, AbilityVest>(7.5F)
            };

        // Additional Options
        public static readonly CustomNumberOption OptionSheriffMaxHintAmount =
            CustomOption.AddNumber(Role.GetName<Sheriff>() + ": Max Hint Amount", 3F, 1F, DeadPlayer.Hints.Length, 1F);

        public static readonly CustomNumberOption OptionSheriffHintDecreaseInterval =
            CustomOption.AddNumber(Role.GetName<Sheriff>() + ": Hint Decrease Interval", 8F, 0F, 15F, 1F);

        public static readonly CustomNumberOption OptionSheriffMinHintAmount =
            CustomOption.AddNumber(Role.GetName<Sheriff>() + ": Min Hint Amount", 0F, 0F, DeadPlayer.Hints.Length, 1F);

        public static readonly CustomNumberOption OptionBodyguardGuardRange =
            CustomOption.AddNumber(Role.GetName<Bodyguard>() + ": Guard Range", 1F, 0.25F, 2F, 0.25F);

        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer =
            CustomOption.AddString(Role.GetName<Doctor>() + ": Show Shielded Owner",
                new[] {"Doctor", "Target", "Doctor & Target", "Everyone"});

        public static readonly CustomToggleOption OptionLookoutSharesCooldown =
            CustomOption.AddToggle(Role.GetName<Lookout>() + ": Abilities Share Cooldown", true);

        public override void Load()
        {
            foreach (Role role in Roles)
            {
                // RoleAllowSpawns.Add(GetRoleAllowSpawnOption(role));
                RoleSpawnChances.Add(CreateRoleSpawnChanceOption(role));
            }

            for (var i = 0; i < RoleSlots.Length; i++)
            {
                RoleSlots[i] = CustomOption.AddString("Role Slot " + (i + 1),
                    new[]
                    {
                        "Crew", "Crew Investigative", "Crew Killing", "Crew Protective", "Crew Support", "Mafia",
                        "Mafia Deception", "Mafia Killing", "Mafia Support", "Neutral", "Neutral Benign",
                        "Neutral Chaos", "Neutral Evil", "Neutral Killing"
                    });
            }


            OptionPage.CreateOptionPage(new CustomOption[]
            {
                OptionShowPlayerNames, OptionMafiaSharedKillCooldown, OptionMafiaCustomSharedKillCooldown,
                OptionMafiaKillStart, OptionMafiaKillAlways, OptionLookoutSharesCooldown, OptionSheriffMaxHintAmount,
                OptionSheriffHintDecreaseInterval, OptionSheriffMinHintAmount, OptionBodyguardGuardRange,
                OptionDoctorShowShieldedPlayer
            });
            OptionPage.CreateOptionPage(RoleSlots);
            OptionPage.CreateOptionPage(RoleSpawnChances.Values);
            OptionPage.CreateOptionPage(RoleCooldowns.Values);
            OptionPage.CreateOptionPage(RoleDurations.Values);

            // Disable the https://github.com/DorCoMaNdO/Reactor-Essentials watermark.
            // The code said that you were allowed, as long as you provided credit elsewhere. 
            // I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
            // If the owner of this library has any problems with this, just message me on discord and we'll find a solution
            // BothLine#9610
            CustomOption.ShamelessPlug = false;
            Harmony.PatchAll();
        }

        public static IEnumerable<RoleSlot> GetRoleSlots()
        {
            var roleSlots = new RoleSlot[RoleSlots.Length];

            for (var i = 0; i < roleSlots.Length; i++)
            {
                switch (RoleSlots[i].GetValue())
                {
                    case 0:
                        roleSlots[i] = new RoleSlot(Faction.Crew);
                        break;
                    case 1:
                        roleSlots[i] = new RoleSlot(Faction.Crew, Alignment.Investigative);
                        break;
                    case 2:
                        roleSlots[i] = new RoleSlot(Faction.Crew, Alignment.Killing);
                        break;
                    case 3:
                        roleSlots[i] = new RoleSlot(Faction.Crew, Alignment.Protective);
                        break;
                    case 4:
                        roleSlots[i] = new RoleSlot(Faction.Crew, Alignment.Support);
                        break;
                    case 5:
                        roleSlots[i] = new RoleSlot(Faction.Mafia);
                        break;
                    case 6:
                        roleSlots[i] = new RoleSlot(Faction.Mafia, Alignment.Deception);
                        break;
                    case 7:
                        roleSlots[i] = new RoleSlot(Faction.Mafia, Alignment.Killing);
                        break;
                    case 8:
                        roleSlots[i] = new RoleSlot(Faction.Mafia, Alignment.Support);
                        break;
                    case 9:
                        roleSlots[i] = new RoleSlot(Faction.Neutral);
                        break;
                    case 10:
                        roleSlots[i] = new RoleSlot(Faction.Neutral, Alignment.Benign);
                        break;
                    case 11:
                        roleSlots[i] = new RoleSlot(Faction.Neutral, Alignment.Chaos);
                        break;
                    case 12:
                        roleSlots[i] = new RoleSlot(Faction.Neutral, Alignment.Evil);
                        break;
                    case 13:
                        roleSlots[i] = new RoleSlot(Faction.Neutral, Alignment.Killing);
                        break;
                }
            }

            return roleSlots;
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

        public static float GetRoleSpawnChance(Type type)
        {
            return RoleSpawnChances.TryGetValue(type, out CustomNumberOption value) ? value.GetValue() : 0F;
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