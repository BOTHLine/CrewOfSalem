using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.CustomOptions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using CrewOfSalem.Roles.Alignments;

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

        // Spawn Chances
        public static readonly CustomNumberOption OptionInvestigatorSpawnChance = GetRoleSpawnChanceOption<Investigator>();
        public static readonly CustomNumberOption OptionSheriffSpawnChance = GetRoleSpawnChanceOption<Sheriff>();
        public static readonly CustomNumberOption OptionSpySpawnChance = GetRoleSpawnChanceOption<Spy>();
        public static readonly CustomNumberOption OptionTrackerSpawnChance = GetRoleSpawnChanceOption<Tracker>();

        public static readonly CustomNumberOption OptionVigilanteSpawnChance = GetRoleSpawnChanceOption<Vigilante>();

        public static readonly CustomNumberOption OptionDoctorSpawnChance = GetRoleSpawnChanceOption<Doctor>();

        public static readonly CustomNumberOption OptionEscortSpawnChance = GetRoleSpawnChanceOption<Escort>();

        public static readonly CustomNumberOption OptionJesterSpawnChance = GetRoleSpawnChanceOption<Jester>();

        // Crew
        // Crew Investigative
        // Investigator
        public static readonly CustomNumberOption OptionInvestigatorCooldown = GetCooldownOption<Investigator>();

        // Sheriff

        // Spy

        // Tracker

        // Crew Killing
        // Vigilante
        public static readonly CustomNumberOption OptionVigilanteCooldown = GetCooldownOption<Vigilante>();

        // Crew Protective
        // Doctor
        public static readonly CustomNumberOption OptionDoctorCooldown = GetCooldownOption<Doctor>();
        public static readonly CustomStringOption OptionDoctorShowShieldedPlayer = CustomOption.AddString(nameof(Doctor) + ": Show Shielded Player", new[] { "Doctor", "Target", "Doctor & Target", "Everyone" });

        // Crew Support
        // Escort
        public static readonly CustomNumberOption OptionEscortCooldown = GetCooldownOption<Escort>();
        public static readonly CustomNumberOption OptionEscortCooldownIncrease = CustomOption.AddNumber(nameof(Escort) + ": Cooldown Increase", 30F, 10F, 60F, 0.25F);


        // Neutral
        // Neutral Evil
        // Jester

        // public ConfigEntry<string> Ip { get; set; }
        // public ConfigEntry<ushort> Port { get; set; }

        public override void Load()
        {
            // Ip = Config.Bind("Custom", "Ipv4 or Hostname", "127.0.0.1");
            // Port = Config.Bind("Custom", "Port", (ushort)22023);

            // TODO:Add Assets?



            //Disable the https://github.com/DorCoMaNdO/Reactor-Essentials watermark.
            //The code said that you were allowed, as long as you provided credit elsewhere. 
            //I added a link in the Credits of the GitHub page, and I'm also mentioning it here.
            //If the owner of this library has any problems with this, just message me on discord and we'll find a solution
            //BothLine#9610

            CustomOption.ShamelessPlug = false;

            Harmony.PatchAll();
        }

        private static CustomNumberOption GetRoleSpawnChanceOption<T>() where T : RoleGeneric<T>, new()
        {
            string name = Role.GetName<T>();
            Faction faction = Role.GetFaction<T>();
            Alignment alignment = Role.GetAlignment<T>();
            return CustomOption.AddNumber("(" + faction.shortHandle + alignment.shortHandle + ") " + name, 50F, 0F, 100F, 5F);
        }

        private static CustomNumberOption GetCooldownOption<T>(float value = 30F, float min = 10F, float max = 60F, float increment = 2.5F) where T : RoleGeneric<T>, new()
        {
            return CustomOption.AddNumber(typeof(T).Name + ": Cooldown", value, min, max, increment);
        }
    }
}
