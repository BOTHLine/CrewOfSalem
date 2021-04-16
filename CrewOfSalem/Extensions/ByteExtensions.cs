using System.Linq;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Extensions
{
    public static class ByteExtensions
    {
        public static PlayerControl ToPlayerControl(this byte b)
        {
            return AllPlayers.FirstOrDefault(player => player.PlayerId == b);
        }
    }
}