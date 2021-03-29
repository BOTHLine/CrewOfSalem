using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class SerialKiller : RoleGeneric<SerialKiller>
    {
        // Properties Role
        protected override byte   RoleID => 249;
        public override    string Name   => "Serial Killer";

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description => "You have to kill everyone else";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}