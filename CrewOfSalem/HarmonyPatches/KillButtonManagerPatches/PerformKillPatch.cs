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

            PlayerControl closest = PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer);
            if (closest != null && TryGetSpecialRole(out Veteran veteran) && veteran.Player.PlayerId == closest.PlayerId && veteran.IsOnAlert)
            {
                veteran.KillPlayer(PlayerControl.LocalPlayer);
            }

            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role))
            {
                role.PerformAction(__instance);
            }

            if (closest != null && PlayerControl.LocalPlayer.Data.IsImpostor && TryGetSpecialRole(out Doctor doctor) && doctor.CheckShieldedPlayer(closest.PlayerId))
            {
                doctor.BreakShield();
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.GameOptions.KillCooldown);
            }

            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out doctor))
            {
                return doctor.ShieldedPlayer == null || PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer)?.PlayerId != doctor.ShieldedPlayer?.PlayerId;
            }

            return true;
        }
    }
}