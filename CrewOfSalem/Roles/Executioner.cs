using System.Collections.Generic;
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
        public PlayerControl VoteTarget { get; private set; }

        public bool IsJester => isJester;

        // Properties Role
        public override byte   RoleID => 244;
        public override string Name   => nameof(Executioner);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        protected override Color Color => new Color(172F / 255F, 172F / 255F, 172F / 255F, 1F);

        public override string RoleTask =>
            $"{base.RoleTask} to vote {ColorizedText(VoteTarget.name, Palette.PlayerColors[VoteTarget.Data.ColorId])}";

        public override string Description =>
            "You have to trick everyone else to vote your target. If they die before that, you will turn into a Jester";

        // Methods
        public void ClearTasks()
        {
            if (Owner == null) return;

            var tasksToRemove = new List<PlayerTask>();
            foreach (PlayerTask task in Owner.myTasks)
            {
                if (task.TaskType != TaskTypes.FixComms && task.TaskType != TaskTypes.ResetReactor &&
                    task.TaskType != TaskTypes.ResetSeismic && task.TaskType != TaskTypes.RestoreOxy)
                {
                    tasksToRemove.Add(task);
                }
            }

            foreach (PlayerTask task in tasksToRemove)
            {
                Owner.RemoveTask(task);
            }
        }

        public void Win()
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player == Owner) continue;
                player.RemoveInfected();
                player.Die(DeathReason.Exile);
                player.Data.IsDead = true;
                player.Data.IsImpostor = false;
            }

            Owner.Revive();
            Owner.Data.IsDead = false;
            Owner.Data.IsImpostor = true;
        }

        public void RpcTurnIntoJester()
        {
            if (AmongUsClient.Instance.AmClient) TurnIntoJester();

            WriteImmediately(RPC.ExecutionerToJester);
        }

        // TODO: Don't assign Jester-Role to Executioner. Just make them appear to be a jester?
        // Change their tasks, color name and -help etc.
        // Also Investigator/Consigliere should get Jester Results.
        public void TurnIntoJester()
        {
            var task = Owner.myTasks[0] as ImportantTextTask;
            if (task != null) task.Text = Jester.GetRoleTask();
            isJester = true;
            /*
            PlayerControl player = Owner;
            ClearSettings();
            var jester = new Jester();
            AddSpecialRole(jester, player);

            MessageWriter writer = GetWriter(RPC.SetRole);
            writer.Write(Jester.GetRoleID());
            writer.Write(jester.Owner.PlayerId);
            CloseWriter(writer);
            */
        }

        // Methods Role
        protected override void InitializeAbilities() { }

        protected override void InitializeRoleInternal()
        {
            PlayerControl[] players = PlayerControl.AllPlayerControls.ToArray()
               .Where(player => player.GetRole().Faction == Faction.Crew /* && !(role is Mayor || role is Jailor) */)
               .ToArray();
            VoteTarget = players[Rng.Next(0, players.Length)];
        }
    }
}