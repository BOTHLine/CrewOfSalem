using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.Main;

namespace CrewOfSalem.Roles
{
    public abstract class RoleGeneric<T> : Role
        where T : RoleGeneric<T>, new()
    {
        // Singleton
        private static T instance = null;
        private static readonly object @lock = new object();

        protected static T Instance
        {
            get
            {
                lock (@lock)
                {
                    return instance ??= new T();
                }
            }
        }

        // Methods
        public static byte GetRoleID() => Instance.RoleID;
        public static string GetName() => Instance.Name;

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