using System;

namespace CrewOfSalem
{
    public class DeadPlayer
    {
        public PlayerControl Victim   { get; }
        public PlayerControl Killer   { get; }
        public DateTime      KillTime { get; }

        public DeadPlayer(PlayerControl victim, PlayerControl killer, DateTime killTime)
        {
            Victim = victim;
            Killer = killer;
            KillTime = killTime;
        }
    }
}