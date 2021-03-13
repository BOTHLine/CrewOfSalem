using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Jester : RoleGeneric<Jester>
    {
        public bool canSeeImpostor { get; private set; }
        public bool canDieToVigilante { get; private set; }

        public override Color Color => Color.grey;

        protected override string StartText => "Trick the others to vote you!";

        public override bool HasSpecialButton => false;
        public override Sprite SpecialButton => null;

        public override byte RoleID => 245;

        public override string Name => nameof(Jester);

        public override Faction Faction => Faction.Neutral;

        public override Alignment Alignment => Alignment.Evil;

        public void ClearTasks()
        {
            if (Player == null) return;

            var tasksToRemove = new List<PlayerTask>();
            foreach (PlayerTask task in Player.myTasks)
            {
                if (task.TaskType != TaskTypes.FixComms && task.TaskType != TaskTypes.ResetReactor && task.TaskType != TaskTypes.ResetSeismic && task.TaskType != TaskTypes.RestoreOxy)
                {
                    tasksToRemove.Add(task);
                }
            }
            foreach (PlayerTask task in tasksToRemove)
            {
                Player.RemoveTask(task);
            }
        }

        protected override void ClearSettingsInternal()
        {

        }

        protected override void SetConfigSettings()
        {

        }

        public override void PerformAction(KillButtonManager instance)
        {

        }

        protected override void InitializeRoleInternal()
        {

        }

        public void Win()
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player == Player) continue;
                player.RemoveInfected();
                player.Die(DeathReason.Exile);
                player.Data.IsDead = true;
                player.Data.IsImpostor = false;
            }

            Player.Revive();
            Player.Data.IsDead = false;
            Player.Data.IsImpostor = true;
        }
    }
}