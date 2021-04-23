using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Hypnotist : RoleGeneric<Hypnotist>
    {
        // Methods Role
        public override byte   RoleID => 229;
        public override string Name   => nameof(Hypnotist);

        public override Faction   Faction   => Faction.Mafia;
        public override Alignment Alignment => Alignment.Deception;

        public override string Description => "Hypnotize the Crew to shuffle their perception of other players";

        // Properties Role
        protected override void InitializeAbilities()
        {
            AddAbility<Hypnotist, AbilityHypnotize>();
        }
    }
}