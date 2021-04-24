using System;

namespace CrewOfSalem
{
    public class Benchmark
    {
        private string   name;
        private DateTime startTime;

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