using System;
using CrewOfSalem.Roles;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem
{
    public class DeadPlayer
    {
        public PlayerControl Victim   { get; }
        public PlayerControl Killer   { get; }
        public DateTime      KillTime { get; }
        public float         KillAge  => (float) (DateTime.UtcNow - KillTime).TotalMilliseconds;

        public DeadPlayer(PlayerControl victim, PlayerControl killer, DateTime killTime)
        {
            Victim = victim;
            Killer = killer;
            KillTime = killTime;
        }

        public string ParseBodyReport()
        {
            string roleName = "Crewmate";
            if (TryGetSpecialRoleByPlayer(Victim.PlayerId, out Role role))
            {
                roleName = role.Name;
            } else if (Victim.Data.IsImpostor)
            {
                roleName = "Impostor";
            }

            // TODO: Hints

            return "Default Body Report";
        }
    }
}