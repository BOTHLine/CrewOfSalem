using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Sheriff : RoleGeneric<Sheriff>
    {
        // Properties Role
        public override byte RoleID => 210;
        public override string Name => nameof(Sheriff);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override Color Color => Color.blue;
        protected override string StartText => "Find the Impostors";

        public override bool HasSpecialButton => false;
        public override Sprite SpecialButton => null;

        public override void PerformAction(KillButtonManager instance)
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearSettingsInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override void InitializeRoleInternal()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetConfigSettings()
        {
            throw new System.NotImplementedException();
        }
    }
}