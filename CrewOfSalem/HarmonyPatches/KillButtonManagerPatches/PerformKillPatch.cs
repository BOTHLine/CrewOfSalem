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
            /*
            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role))
            {
                role.PerformAction();
                return false;
            }
            */
            return false;
            /*
            if (__instance == null || PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data.IsDead)
            {
                return false;
            }

            PlayerControl closest = PlayerTools.FindClosestTarget(PlayerControl.LocalPlayer);
            TryGetSpecialRoleByPlayer((byte) (closest?.PlayerId ?? -1), out Role closestRole);

            if (closest != null && closestRole is Veteran {HasDurationLeft: true} veteran)
            {
                veteran.KillPlayer(PlayerControl.LocalPlayer);
                return false;
            }

            if (TryGetSpecialRoleByPlayer(PlayerControl.LocalPlayer.PlayerId, out Role role))
            {
                // role.SpecialButton.Use();
            }

            if (closest != null && role.Alignment == Alignment.Killing &&
                closestRole is Survivor {HasDurationLeft: true})
            {
                PlayerControl.LocalPlayer.SetKillTimer(PlayerControl.LocalPlayer.Data.IsImpostor
                    ? PlayerControl.GameOptions.KillCooldown
                    : role.Cooldown);

                return false;
            }

            return false;
            */
        }
    }
}