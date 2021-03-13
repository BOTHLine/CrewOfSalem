using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.KillButtonManagerPatches
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class PerformKillPatch
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data.IsDead) return false;

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Investigator investigator))
            {
                // investigator.Investigate(__instance);
            }

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Doctor doctor))
            {
                doctor.PerformAction(__instance);
            }

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Vigilante vigilante))
            {
                vigilante.PerformAction(__instance);
            }

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Escort escort))
            {
                escort.PerformAction(__instance);
            }

            PlayerControl closest = PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer);
            if (closest != null && PlayerControl.LocalPlayer.Data.IsImpostor && SpecialRoleIsAssigned<Doctor>(out var doctorKvp) && doctorKvp.Value.CheckShieldedPlayer(closest.PlayerId))
            {
                doctorKvp.Value.BreakShield();
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
            }

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Doctor doctorClosest))
            {
                return doctorClosest.ShieldedPlayer == null || PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer)?.PlayerId != doctorClosest.ShieldedPlayer?.PlayerId;
            }

            return true;
        }
    }
}