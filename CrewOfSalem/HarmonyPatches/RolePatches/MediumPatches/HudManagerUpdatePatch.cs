using System.Linq;
using CrewOfSalem.Roles.Abilities;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.RolePatches.MediumPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerUpdatePatch
    {
        public static void Postfix()
        {
            if (ShipStatus.Instance == null) return;
            
            AbilitySeance[] seanceAbilities = Ability.GetAllAbilities<AbilitySeance>();
            if (seanceAbilities.Any(seance => seance.owner?.Owner == LocalPlayer && seance.HasDurationLeft && !LocalData.IsDead))
            {
                foreach (PlayerControl player in AllPlayers)
                {
                    player.Visible = !player.inVent;
                }
            } else
            {
                foreach (PlayerControl player in AllPlayers)
                {
                    player.Visible = (LocalData.IsDead || !player.Data.IsDead) && !player.inVent;
                }
            }
        }
    }
}