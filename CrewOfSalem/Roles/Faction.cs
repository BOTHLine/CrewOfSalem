using UnityEngine;

namespace CrewOfSalem.Roles.Factions
{
    public abstract class Faction
    {
        // Instances
        public static readonly Faction Crew = new Crew();
        public static readonly Faction Mafia = new Mafia();
        public static readonly Faction Neutral = new Neutral();
        public static readonly Faction Coven = new Coven();

        public static readonly Faction[] Factions = new[] { Crew, Mafia, Neutral, Coven };

        // Properties
        public abstract string Name { get; }
        public string ShortHandle => Name.Substring(0, 1);

        public abstract Color Color { get; }

        public abstract Faction Enemy { get; }
    }

    public class Crew : Faction
    {
        public override string Name => nameof(Crew);

        public override Faction Enemy => Mafia;

        public override Color Color => Color.green;
    }

    public class Mafia : Faction
    {
        public override string Name => nameof(Mafia);

        public override Faction Enemy => Crew;

        public override Color Color => Color.red;
    }

    public class Neutral : Faction
    {
        public override string Name => nameof(Neutral);

        public override Faction Enemy => Crew;

        public override Color Color => Color.grey;
    }

    public class Coven : Faction
    {
        public override string Name => nameof(Coven);

        public override Faction Enemy => Crew;

        public override Color Color => Color.magenta;
    }
}