using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{

    public class Investigator : RoleGeneric<Investigator>
    {
        public override Color Color => Color.green;

        protected override string StartText => "Find the [FF0000FF]Mafia[]";

        public override bool HasSpecialButton => true;

        public override Sprite SpecialButton => InvestigatorButton;

        public override byte RoleID => 207;

        public override string Name => nameof(Investigator);

        public override Faction Faction => Faction.Crew;

        public override Alignment Alignment => Alignment.Investigative;

        protected override void ClearSettingsInternal()
        {

        }

        protected override void SetConfigSettings()
        {
            Cooldown = Main.OptionInvestigatorCooldown.GetValue();
        }

        public override void PerformAction(KillButtonManager instance)
        {

        }

        protected override void InitializeRoleInternal()
        {

        }
    }
}