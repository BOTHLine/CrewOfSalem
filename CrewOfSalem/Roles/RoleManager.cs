/*
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Essentials.CustomOptions;
using Hazel;
using System;
using System.Collections.Generic;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public static class RoleManager
    {
        // Fields
        public static readonly Dictionary<Type, RoleInformation> roleInformations = new Dictionary<Type, RoleInformation>
        {
            // Crew
            // Crew Investigative
            { typeof(Investigator), new RoleInformation (207, nameof(Investigator), Faction.Crew, Alignment.Investigative ) },
            { typeof(Sheriff), new RoleInformation(210, nameof(Sheriff), Faction.Crew, Alignment.Investigative) },
            { typeof(Spy), new RoleInformation(211, nameof(Spy), Faction.Crew, Alignment.Investigative) },
            { typeof(Tracker), new RoleInformation(212, nameof(Tracker), Faction.Crew, Alignment.Investigative) },

            // Crew Killing
            { typeof(Vigilante), new RoleInformation(216, nameof(Vigilante), Faction.Crew, Alignment.Killing) },

            // Crew Protective
            { typeof(Doctor), new RoleInformation(218, nameof(Doctor), Faction.Crew, Alignment.Protective) },

            // Crew Support
            {typeof(Escort) , new RoleInformation(221, nameof(Escort), Faction.Crew, Alignment.Support) },

            // Neutral
            // Neutral Evil
            {typeof(Jester), new RoleInformation(245, nameof(Jester), Faction.Neutral, Alignment.Evil) }
        };

        // Methods
        public static void SetRole<T>(ref List<PlayerControl> crew, CustomNumberOption spawnChanceOption)
            where T : RoleGeneric<T>, new()
        {
            float spawnChance = spawnChanceOption.GetValue();
            if (spawnChance < 1 || crew.Count <= 0) return;
            bool spawnChanceAchieved = RNG.Next(1, 101) <= spawnChance;
            if (!spawnChanceAchieved) return;

            int random = RNG.Next(0, crew.Count);
            T role = new T();
            role.InitializeRole(crew[random]);
            AddSpecialRole(role);
            crew.RemoveAt(random);

            MessageWriter writer = GetWriter(RPC.SetRole);
            writer.Write(GetRoleID<T>());
            writer.Write(role.Player.PlayerId);
            CloseWriter(writer);
        }

        public static RoleInformation GetRoleInformation(Type roleType)
        {
            return roleInformations[roleType];
        }

        public static RoleInformation GetRoleInformation<T>() where T : RoleGeneric<T>
        {
            return GetRoleInformation(typeof(T));
        }

        public static byte GetRoleID<T>() where T : RoleGeneric<T>, new()
        {
            return GetRoleInformation<T>().RoleID;
        }

        public static byte GetRoleID<T>(T role) where T : RoleGeneric<T>, new()
        {
            return GetRoleID<T>();
        }

        public static string GetName<T>() where T : RoleGeneric<T>, new()
        {
            return GetRoleInformation<T>().Name;
        }

        public static string GetName<T>(T role) where T : RoleGeneric<T>
        {
            return GetName<T>();
        }

        public static Faction GetFaction<T>() where T : RoleGeneric<T>
        {
            return GetRoleInformation<T>().Faction;
        }

        public static Faction GetFaction<T>(T role) where T : RoleGeneric<T>
        {
            return GetFaction<T>();
        }

        public static Alignment GetAlignment<T>() where T : RoleGeneric<T>
        {
            return GetRoleInformation<T>().Alignment;
        }

        public static Alignment GetAlignment<T>(T role) where T : RoleGeneric<T>
        {
            return GetAlignment<T>();
        }

        public static bool GetDoSpawnRole<T>() where T : RoleGeneric<T>
        {
            return GetRoleInformation<T>().DoSpawnRole;
        }

        public static bool GetDoSpawnRole<T>(T role) where T : RoleGeneric<T>
        {
            return GetDoSpawnRole<T>();
        }

        // Nested Types
        public class RoleInformation
        {
            public byte RoleID { get; }
            public string Name { get; }
            public Faction Faction { get; }
            public Alignment Alignment { get; }
            public bool DoSpawnRole { get; }

            public RoleInformation(byte roleID, string name, Faction faction, Alignment alignment, bool doSpawnRole = true)
            {
                RoleID = roleID;
                Name = name;
                Faction = faction;
                Alignment = alignment;
                DoSpawnRole = doSpawnRole;
            }
        }
    }
}
*/