using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Jester : RoleGeneric<Jester>
    {
        // Properties Role
        public override byte   RoleID => 245;
        public override    string Name   => nameof(Jester);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        public override Color Color => new Color(244F / 255F, 159F / 255F, 208F / 255F, 1F);

        public override string RoleTask    => $"{base.RoleTask} to vote {ColorizedText("you", Color)}";
        public override string Description => "You have to trick everyone else to vote you";

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}