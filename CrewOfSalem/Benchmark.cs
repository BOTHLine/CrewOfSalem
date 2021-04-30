using System;

namespace CrewOfSalem
{
    public class Benchmark
    {
        private readonly string   name;
        private readonly DateTime startTime;

        public Benchmark(string name)
        {
            startTime = DateTime.UtcNow;
            this.name = name;
        }

        public void End()
        {
            float time = (DateTime.UtcNow - startTime).Milliseconds;
            ConsoleTools.Info("Benchmark " + name + " took " + time + " milliseconds");
        }
    }
}