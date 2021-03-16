using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.CrewOfSalem;

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
        public abstract string Name { get; }

        public string ShortHandle => Name.Substring(0, 1);

        public abstract string Task { get; }
        public abstract bool IsTaskForOwnFaction { get; }

        public string GetTask(Faction faction) => $"{Task} {faction.Enemy}";
        public string GetColorizedTask(Faction faction) => $"{Task} {ColorizedText(IsTaskForOwnFaction ? faction.Name : faction.Enemy.Name, IsTaskForOwnFaction ? faction.Color : faction.Enemy.Color)}";
    }

    public class Investigative : Alignment
    {
        public override string Name => nameof(Investigative);

        public override string Task => "Find the";

        public override bool IsTaskForOwnFaction => false;
    }

    public class Killing : Alignment
    {
        public override string Name => nameof(Killing);

        public override string Task => "Kill the";

        public override bool IsTaskForOwnFaction => false;
    }

    public class Protective : Alignment
    {
        public override string Name => nameof(Protective);

        public override string Task => "Protect the";

        public override bool IsTaskForOwnFaction => true;
    }

    public class Support : Alignment
    {
        public override string Name => nameof(Support);

        public override string Task => "Support the";

        public override bool IsTaskForOwnFaction => true;
    }

    public class Deception : Alignment
    {
        public override string Name => nameof(Deception);

        public override string Task => "Deceive the";

        public override bool IsTaskForOwnFaction => false;
    }

    public class Benign : Alignment
    {
        public override string Name => nameof(Benign);

        public override string Task => "TODO";

        public override bool IsTaskForOwnFaction => false;
    }

    public class Chaos : Alignment
    {
        public override string Name => nameof(Chaos);

        public override string Task => "Bring Chaos to the";

        public override bool IsTaskForOwnFaction => false;
    }

    public class Evil : Alignment
    {
        public override string Name => nameof(Evil);

        public override string Task => "Trick the";

        public override bool IsTaskForOwnFaction => false;
    }
}