using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Jester : RoleGeneric<Jester>
    {
        // Properties
        public bool CanSeeImpostor    { get; private set; }
        public bool CanDieToVigilante { get; private set; }

        // Properties Role
        protected override byte   RoleID => 245;
        public override string Name   => nameof(Jester);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        protected override Color Color => new Color(244F / 255F, 159F / 255F, 208F / 255F, 1F);

        protected override bool   HasSpecialButton => false;
        protected override Sprite SpecialButtonSprite    => null;

        // Methods
        public void ClearTasks()
        {
            if (Player == null) return;

            var tasksToRemove = new List<PlayerTask>();
            foreach (PlayerTask task in Player.myTasks)
            {
                if (task.TaskType != TaskTypes.FixComms     && task.TaskType != TaskTypes.ResetReactor &&
                    task.TaskType != TaskTypes.ResetSeismic && task.TaskType != TaskTypes.RestoreOxy)
                {
                    tasksToRemove.Add(task);
                }
            }

            foreach (PlayerTask task in tasksToRemove)
            {
                Player.RemoveTask(task);
            }
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

        // Methods Role
        protected override void SetConfigSettings() { }
    }
}