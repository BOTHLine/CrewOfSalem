using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Executioner : RoleGeneric<Executioner>
    {
        // Fields
        private bool isJester = false;

        // Properties
        public PlayerControl VoteTarget { get; set; }

        public bool IsJester => isJester;

        // Properties Role
        public override byte   RoleID => 244;
        public override string Name   => nameof(Executioner);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        public override Color Color => new Color(172F / 255F, 172F / 255F, 172F / 255F, 1F);

        public override string RoleTask =>
            $"{base.RoleTask} to vote {ColorizedText(VoteTarget.name, VoteTarget.GetPlayerColor())}";

        public override string Description =>
            "You have to trick everyone else to vote your target. If they die before that, you will turn into a Jester";

        // Methods
        // TODO: Don't assign Jester-Role to Executioner. Just make them appear to be a jester?
        // Change their tasks, color name and -help etc.
        // Also Investigator/Consigliere should get Jester Results.
        public void TurnIntoJester()
        {
            /*
            var task = Owner.myTasks.ToArray()[0] as ImportantTextTask;
            if (task != null) task.Text = Jester.GetRoleTask();
            isJester = true;
            */
            PlayerControl player = Owner;
            ClearSettings();
            AddRole(Jester.Instance, player);

            player.ClearTasksCustom();
            Jester.Instance.SetRoleTask();
        }

        // Methods Role
        protected override void InitializeAbilities() { }

        protected override void InitializeRoleInternal()
        {
            if (Owner != LocalPlayer) return;

            PlayerControl[] players = AllPlayers
               .Where(player => player.GetRole().Faction == Faction.Crew && !(player.GetRole() is Mayor)).ToArray();
            VoteTarget = players[Rng.Next(players.Length)];

            WriteRPC(RPC.ExecutionerTarget, VoteTarget.PlayerId);
        }

        protected override void SetRoleTaskInternal()
        {
            Owner.ClearTasksCustom();
        }

        protected override void ClearSettingsInternal()
        {
            VoteTarget = null;
        }
    }
}