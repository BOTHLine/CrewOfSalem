using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    // TODO Idea: Can kill out of Vents? Can become invisible (by button or standing still long enough) and then can kill (stay invisible?)
    public class Ambusher : RoleGeneric<Ambusher>
    {
        // Fields
        public int isKilling = 0;

        // Properties Role
        public override byte   RoleID => 231;
        public override    string Name   => nameof(Ambusher);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Killing;

        public override string Description { get; }

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Ambusher, AbilityKill>();
        }
    }
}