using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Spy : RoleGeneric<Spy>
    {
        public override Color Color => Color.blue;

        public override bool HasSpecialButton => false;

        public override Sprite SpecialButton => null;

        protected override string StartText => "Find the [FF0000FF]Mafia[]";

        public override byte RoleID => 211;

        public override string Name => nameof(Spy);

        public override Faction Faction => Faction.Crew;

        public override Alignment Alignment => Alignment.Investigative;

        protected override void ClearSettingsInternal()
        {

        }

        public override void PerformAction(KillButtonManager instance)
        {

        }

        protected override void SetConfigSettings()
        {

        }
    }
}