using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Arsonist : RoleGeneric<Arsonist>
    {
        // Properties Role
        public override byte   RoleID => 247;
        public override    string Name   => nameof(Arsonist);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Killing;
        
        public override string Description { get; }

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}