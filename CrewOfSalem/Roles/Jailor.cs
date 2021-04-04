using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Jailor : RoleGeneric<Jailor>
    {
        // Properties Role
        public override byte   RoleID => 213;
        public override    string Name   => nameof(Jailor);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can jail a player to prevent them from playing a round";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}