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
        public string name => GetType().Name;

        public string shortHandle => name.Substring(0, 1);
    }

    public class Crew : Faction
    {

    }

    public class Mafia : Faction
    {

    }

    public class Neutral : Faction
    {

    }

    public class Coven : Faction
    {

    }
}