using CrewOfSalem.Roles;
using HarmonyLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.VentPatches
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.Method_38))]
    public static class VentEnterPatch
    {
        public static void Postfix(PlayerControl NMEAPOJFNKA)
        {
            PlayerVentTimeUtility.SetLastVent(NMEAPOJFNKA.PlayerId);

            if (TryGetSpecialRole(PlayerControl.LocalPlayer.PlayerId, out Tracker tracker))
            {
                tracker.SendChatMessage(Tracker.MessageType.PlayerEnteredVent);
            }
        }
    }
}