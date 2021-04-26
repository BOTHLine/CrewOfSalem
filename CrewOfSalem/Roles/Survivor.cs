using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Survivor : RoleGeneric<Survivor>
    {
        // Properties Role
        public override byte   RoleID => 239;
        public override string Name   => nameof(Survivor);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;

        public override Color Color => new Color(200F / 255F, 200F / 255F, 0F / 255F, 1F);

        public override string RoleTask => "Survive until the end";

        public override string Description => "You can vest to protect yourself from the next attack for a specific time. You can win with either other faction, as long as you live";

        // Methods Ability
        protected override void InitializeAbilities()
        {
            AddAbility<Survivor, AbilityVest>();
        }

        protected override void SetRoleTaskInternal()
        {
            Owner.ClearTasksCustom();
        }
    }
}