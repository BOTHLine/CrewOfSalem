using System;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem
{
    public class DeadPlayer
    {
        // Fields
        public static readonly Func<DeadPlayer, string>[] Hints = {
            GetKillAge,
            GetKillerColorType,
            GetVictimRole,
            GetKillerLetter,
            GetKillerKillCount,
            GetKillerRole
        };

        // Properties
        public PlayerControl Victim   { get; }
        public PlayerControl Killer   { get; }
        public DateTime      KillTime { get; }
        public float         KillAge  => (float) (DateTime.UtcNow - KillTime).TotalMilliseconds;

        // Constructors
        public DeadPlayer(PlayerControl victim, PlayerControl killer, DateTime killTime)
        {
            Victim = victim;
            Killer = killer;
            KillTime = killTime;
        }

        // Hint Methods
        private static string GetKillAge(DeadPlayer deadPlayer)
        {
            return $"The victim was killed {Mathf.Round(deadPlayer.KillAge / 1000F).ToString()} seconds ago.";
        }

        private static string GetKillerColorType(DeadPlayer deadPlayer)
        {
            Color32 color = Palette.PlayerColors[deadPlayer.Killer.Data.ColorId];
            float average = (color.r + color.g + color.a) / 3F;
            string colorType = average >= 0.465F ? "Lighter" : "Darker";
            return $"The killer has a {colorType} color.";
            // TODO: Instead of using 0.465F use "Greyscale-Calculation" on this site to determine if the brightness is over 0.5? https://lodev.org/cgtutor/color.html
        }

        private static string GetVictimRole(DeadPlayer deadPlayer)
        {
            return $"The victim was a(n) {deadPlayer.Victim.GetRole().Name}.";
        }

        private static string GetKillerLetter(DeadPlayer deadPlayer)
        {
            return
                $"The killer's name has the letter '{deadPlayer.Killer.name[Rng.Next(deadPlayer.Killer.name.Length)].ToString()}'.";
        }

        private static string GetKillerKillCount(DeadPlayer deadPlayer)
        {
            return
                $"The killer has already killed a total of {DeadPlayers.Count(player => player.Killer.PlayerId == deadPlayer.Killer.PlayerId).ToString()} players.";
        }

        private static string GetKillerRole(DeadPlayer deadPlayer)
        {
            return $"The killer was a(n) {deadPlayer.Killer.GetRole().Name}";
        }
    }
}