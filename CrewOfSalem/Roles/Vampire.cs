using CrewOfSalem.HarmonyPatches.PlayerControlPatches;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;

namespace CrewOfSalem.Roles
{
    public class Vampire : RoleGeneric<Vampire>
    {
        // Properties Role
        public override byte   RoleID => 243;
        public override    string Name   => nameof(Vampire);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Chaos;

        public override string Description => "You can bite to turn another player into a vampire. But you will kill yourself on the Mafia. Only the youngest vampire can bite within a round";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<Vampire, AbilityBite>();
        }
    }
}