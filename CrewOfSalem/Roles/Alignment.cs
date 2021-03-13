namespace CrewOfSalem.Roles.Alignments
{
    public abstract class Alignment
    {
        // Instances
        public static readonly Alignment Investigative = new Investigative();
        public static readonly Alignment Killing = new Killing();
        public static readonly Alignment Protective = new Protective();
        public static readonly Alignment Support = new Support();
        public static readonly Alignment Deception = new Deception();
        public static readonly Alignment Chaos = new Chaos();
        public static readonly Alignment Evil = new Evil();

        public static readonly Alignment[] Alignments = new[] { Investigative, Killing, Protective, Support, Deception, Chaos, Evil };

        // Properties
        public string name => GetType().Name;

        public string shortHandle => name.Substring(0, 1);
    }

    public class Investigative : Alignment
    {

    }

    public class Killing : Alignment
    {

    }

    public class Protective : Alignment
    {

    }

    public class Support : Alignment
    {

    }

    public class Deception : Alignment
    {

    }

    public class Benign : Alignment
    {

    }

    public class Chaos : Alignment
    {

    }

    public class Evil : Alignment
    {

    }
}