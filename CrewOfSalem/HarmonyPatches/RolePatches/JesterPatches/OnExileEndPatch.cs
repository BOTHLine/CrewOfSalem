using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.JesterPatches
{
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public static class OnExileEndPatch
    {
        public static void Postfix()
        {
            if (TryGetSpecialRole(out Jester jester) &&
                jester.Owner.PlayerId == ExileController.Instance.exiled?.PlayerId)
            {
                jester.Owner.WinSolo();
            }
        }
    }
}