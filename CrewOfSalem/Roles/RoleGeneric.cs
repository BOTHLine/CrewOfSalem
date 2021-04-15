using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public abstract class RoleGeneric<T> : Role
        where T : RoleGeneric<T>, new()
    {
        // Singleton
        private static T instance = null;

        private static readonly object Lock = new object();

        public static T Instance
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
        protected RoleGeneric()
        {
            instance = (T) this;
        }

        // Methods
        public static byte GetRoleID() => Instance.RoleID;
        public static string GetName() => Instance.Name;
        public static Color GetColor() => Instance.Color;
        protected static string ColorizedName => ColorizedText(GetName(), GetColor());

        public static Faction GetFaction() => Instance.Faction;
        public static Alignment GetAlignment() => Instance.Alignment;

        public static PlayerControl GetPlayer() => Instance.Owner;

        public static string GetRoleTask() => Instance.RoleTask;
    }
}