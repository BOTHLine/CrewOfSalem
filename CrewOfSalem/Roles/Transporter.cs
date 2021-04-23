using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Transporter : RoleGeneric<Transporter>
    {
        public PlayerControl Transported1;
        public PlayerControl Transported2;
        
        // Properties Role
        public override byte   RoleID => 225;
        public override    string Name   => nameof(Transporter);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description => "You can transport the locations of 2 targets in a meeting. Their received votes get swapped";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}