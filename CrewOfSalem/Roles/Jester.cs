using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;
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

        protected override Color Color => new Color(244F / 255F, 159F / 255F, 208F / 255F, 1F);

        public override string RoleTask    => $"{base.RoleTask} to vote {ColorizedText("you", Color)}";
        public override string Description => "You have to trick everyone else to vote you";

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

        // Methods Role
        protected override void InitializeAbilities() { }
    }
}