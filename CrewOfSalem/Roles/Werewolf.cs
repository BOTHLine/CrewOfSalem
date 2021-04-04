using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Werewolf : RoleGeneric<Werewolf>
    {
        // Properties Role
        public override byte   RoleID => 250;
        public override    string Name   => nameof(Werewolf);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You can go rampage in every second round on full moon. You will kill everybody within your reach";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}