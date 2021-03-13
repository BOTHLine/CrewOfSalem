using System;
using System.Collections.Generic;

namespace CrewOfSalem.HarmonyPatches.VentPatches
{
    public static class PlayerVentTimeUtility
    {
        private static readonly Dictionary<byte, DateTime> AllVentTimes = new Dictionary<byte, DateTime>();

        public static void SetLastVent(byte player)
        {
            AllVentTimes[player] = DateTime.UtcNow;
        }

        public static DateTime GetLastVent(byte player)
        {
            return AllVentTimes.ContainsKey(player) ? AllVentTimes[player] : new DateTime(0);
        }
    }
}
