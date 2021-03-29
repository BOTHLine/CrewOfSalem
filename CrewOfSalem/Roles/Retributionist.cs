using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Retributionist : RoleGeneric<Retributionist>
    {
        // Properties Role
        protected override byte   RoleID => 224;
        public override    string Name   => nameof(Retributionist);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can use the abilities of the dead";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}