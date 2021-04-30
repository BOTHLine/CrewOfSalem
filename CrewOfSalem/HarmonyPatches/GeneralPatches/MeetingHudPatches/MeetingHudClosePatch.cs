using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches.MeetingHudPatches
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingHudClosePatch
    {
        public static void Postfix()
        {
            foreach (PlayerControl player in AllPlayers)
            {
                IReadOnlyList<Ability> abilities = player.GetAbilities();
                foreach (Ability ability in abilities)
                {
                    ability.MeetingEnd();
                }
            }
        }
    }
}