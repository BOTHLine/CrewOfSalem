using System.Collections.Generic;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Extensions
{
    public static class PlayerControlExtensions
    {
        public static Role GetRole(this PlayerControl playerControl)
        {
            return GetSpecialRoleByPlayer(playerControl);
        }

        public static IReadOnlyList<Ability> GetAbilities(this PlayerControl playerControl)
        {
            return playerControl.GetRole()?.GetAllAbilities();
        }

        public static T GetAbility<T>(this PlayerControl playerControl)
            where T : Ability
        {
            return playerControl.GetRole()?.GetAbility<T>();
        }

        public static void UseAbility<T>(this PlayerControl playerControl, PlayerControl target)
            where T : Ability
        {
            playerControl.GetAbility<T>()?.Use(target, out bool sendRpc);
        }

        public static void EndAbility<T>(this PlayerControl playerControl)
            where T : AbilityDuration
        {
            playerControl.GetAbility<T>().EffectEnd();
        }
    }
}