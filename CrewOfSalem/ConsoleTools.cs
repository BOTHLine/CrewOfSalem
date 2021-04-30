using System;

namespace CrewOfSalem
{
    public static class ConsoleTools
    {
        public static void Info(object message)
        {
            ConsoleColor color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"[CrewOfSalem INF] {DateTime.UtcNow.TimeOfDay}: {message}");
            System.Console.ForegroundColor = color;
        }

        public static void Error(object message)
        {
            ConsoleColor color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"[CrewOfSalem ERR] {DateTime.UtcNow.TimeOfDay}: {message}");
            System.Console.ForegroundColor = color;
        }
    }
}