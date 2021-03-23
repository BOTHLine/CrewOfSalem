using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.Main;

namespace CrewOfSalem.Roles
{
    public abstract class RoleGeneric<T> : Role
        where T : RoleGeneric<T>, new()
    {
        // Singleton
        private static T instance = null;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        protected static T Instance
        {
            get
            {
                lock (Lock)
                {
                    return instance ??= new T();
                }
            }
        }

        // Constructors
        protected RoleGeneric() : base()
        {
            instance = (T) this;
        }

        protected RoleGeneric(PlayerControl player) : base(player)
        {
            instance = (T) this;
        }

        // Methods
        public static byte GetRoleID() => Instance.RoleID;
        public static string GetName() => Instance.Name;
        public static Color GetColor() => Instance.Color;

        public static Faction GetFaction() => Instance.Faction;
        public static Alignment GetAlignment() => Instance.Alignment;

        public static PlayerControl GetPlayer() => Instance.Player;

        protected override void SetConfigSettings()
        {
            Cooldown = GetRoleCooldown<T>();
            Duration = GetRoleDuration<T>();
        }
    }
}