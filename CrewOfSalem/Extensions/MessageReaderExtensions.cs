using Hazel;

namespace CrewOfSalem.Extensions
{
    public static class MessageReaderExtensions
    {
        public static void ReadRPC(this MessageReader reader, ref byte[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = reader.ReadByte();
            }
        }
    }
}