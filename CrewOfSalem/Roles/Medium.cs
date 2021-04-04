using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Medium : RoleGeneric<Medium>
    {
        // Properties Role
        public override byte   RoleID => 223;
        public override    string Name   => nameof(Medium);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        public override string Description =>
            "You can interact with the dead and can once interact with a living playing while being dead yourself";

        // Methods
        public static void TurnAllGray()
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                player.nameText.Text = "";
                player.myRend.material.SetColor(ShaderBackColor, Color.grey);
                player.myRend.material.SetColor(ShaderBodyColor, Color.grey);
                player.HatRenderer.SetHat(0, 0);
                SetSkinWithAnim(player.MyPhysics, 0);
                if (player.CurrentPet) Object.Destroy(player.CurrentPet.gameObject);
            }
        }

        public static void MakeDeadVisible()
        {
            foreach (PlayerControl playerControl in PlayerControl.AllPlayerControls)
            {
                playerControl.Visible = true;
            }
        }

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}