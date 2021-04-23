using System.Linq;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Extensions
{
    public static class SByteExtensions
    {
        public static PlayerControl ToPlayerControl(this sbyte b)
        {
            return AllPlayers.FirstOrDefault(player => player.PlayerId == b);
        }
    }
}