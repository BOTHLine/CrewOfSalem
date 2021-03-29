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
        private static readonly Func<DeadPlayer, string>[] Hints = new Func<DeadPlayer, string>[]
        {
            GetKillAge,
            GetKillerColorType,
            GetVictimRole,
            GetKillerLetter,
            GetKillerKillCount
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

        // Methods
        public string ParseBodyReport()
        {
            return Hints[Rng.Next(0, Hints.Length)].Invoke(this);
        }

        // Hint Methods
        private static string GetKillAge(DeadPlayer deadPlayer)
        {
            return $"The victim was killed {Mathf.Round(deadPlayer.KillAge / 1000F).ToString()} seconds ago.";
        }

        private static string GetKillerColorType(DeadPlayer deadPlayer)
        {
            Color32 color = Palette.PlayerColors[deadPlayer.Killer.Data.ColorId];
            int average = (color.r + color.g + color.a) / 3;
            string colorType = average >= 128 ? "Lighter" : "Darker";
            return $"The killer has a {colorType} color.";
        }

        private static string GetVictimRole(DeadPlayer deadPlayer)
        {
            return $"The victim was a(n) {deadPlayer.Victim.GetRole().Name}.";
        }

        private static string GetKillerLetter(DeadPlayer deadPlayer)
        {
            return
                $"The killer's name has the letter '{deadPlayer.Killer.name[Rng.Next(0, deadPlayer.Killer.name.Length)].ToString()}'";
        }

        private static string GetKillerKillCount(DeadPlayer deadPlayer)
        {
            return
                $"The killer has already killed a total of {DeadPlayers.Count(player => player.Killer.PlayerId == deadPlayer.Killer.PlayerId).ToString()} players.";
        }
    }
}