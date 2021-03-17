using System;

namespace CrewOfSalem
{
    public static class ConsoleTools
    {
        public static void Info(object message)
        {
            var color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("[CrewOfSalem INF] " + message);
            System.Console.ForegroundColor = color;
        }

        public static void Error(object message)
        {
            var color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("[CrewOfSalem ERR] " + message);
            System.Console.ForegroundColor = color;
        }
    }
}