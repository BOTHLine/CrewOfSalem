using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Alignments;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.KillButtonManagerPatches
{
    [HarmonyPatch(typeof(KillButtonManager), nameof(KillButtonManager.PerformKill))]
    public static class PerformKillPatch
    {
        public static bool Prefix(KillButtonManager __instance)
        {
            if (__instance == null || PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data.IsDead)
            {
                return false;
            }

            PlayerControl closest = PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer);
            TryGetSpecialRoleByPlayer((byte) (closest?.PlayerId ?? -1), out Role closestRole);

            if (closest != null && closestRole is Veteran {IsAlerted: true} veteran)
            {
                veteran.KillPlayer(PlayerControl.LocalPlayer);
                return false;
            }

            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role))
            {
               // role.SpecialButton.Use();
            }

            if (closest != null && (PlayerControl.LocalPlayer.Data.IsImpostor || role.Alignment == Alignment.Killing) &&
                closestRole is Survivor {IsVested: true})
            {
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.LocalPlayer.Data.IsImpostor
                    ? PlayerControl.GameOptions.KillCooldown
                    : role.Cooldown);

                return false;
            }

            if (closest != null && PlayerControl.LocalPlayer.Data.IsImpostor && TryGetSpecialRole(out Doctor doctor) &&
                doctor.CheckShieldedPlayer(closest.PlayerId))
            {
                doctor.BreakShield();
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.LocalPlayer.Data.IsImpostor
                    ? PlayerControl.GameOptions.KillCooldown
                    : role.Cooldown);
                return false;
            }

            return true;
        }
    }
}