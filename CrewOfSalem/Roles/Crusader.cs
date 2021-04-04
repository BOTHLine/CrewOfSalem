using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Crusader : RoleGeneric<Crusader>
    {
        // Properties Role
        public override byte   RoleID => 219;
        public override    string Name   => nameof(Crusader);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Protective;

        public override string Description { get; }

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}