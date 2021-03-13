using CrewOfSalem.Roles;
using System;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem
{
    public class BodyReport
    {
        public DeadPlayer DeadPlayer { get; }
        public PlayerControl Killer { get; }
        public float KillAge { get; }

        public BodyReport(DeadPlayer deadPlayer, PlayerControl killer, float killAge)
        {
            DeadPlayer = deadPlayer;
            Killer = killer;
            KillAge = killAge;
        }

        public string ParseBodyReport()
        {
            String roleName = "Crewmate";
            if (TryGetSpecialRoleByPlayer(DeadPlayer.Victim.PlayerId, out var role))
            {
                roleName = role.Name;
            }
            else if (DeadPlayer.Victim.Data.IsImpostor)
            {
                roleName = "Impostor";
            }

            // TODO: Hints

            return "Default Body Report";
        }
    }
}