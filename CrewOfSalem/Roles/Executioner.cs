using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Executioner : RoleGeneric<Executioner>
    {
        // Properties
        public PlayerControl VoteTarget { get; private set; }

        // Properties Role
        protected override byte   RoleID => 244;
        public override    string Name   => nameof(Executioner);

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Evil;

        protected override Color Color => new Color(172F / 255F, 172F / 255F, 172F / 255F, 1F);

        protected override bool   HasSpecialButton    => false;
        protected override Sprite SpecialButtonSprite => null;

        protected override string StartText =>
            $"{base.StartText} to vote {ColorizedText(VoteTarget.name, Palette.PlayerColors[VoteTarget.Data.ColorId])}";

        // Methods
        public void ClearTasks()
        {
            if (Player == null) return;

            var tasksToRemove = new List<PlayerTask>();
            foreach (PlayerTask task in Player.myTasks)
            {
                if (task.TaskType != TaskTypes.FixComms && task.TaskType != TaskTypes.ResetReactor &&
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

        public void TurnIntoJester()
        {
            PlayerControl player = Player;
            ClearSettings();
            Jester jester = new Jester();
            AddSpecialRole(jester, player);

            MessageWriter writer = GetWriter(RPC.SetRole);
            writer.Write(Jester.GetRoleID());
            writer.Write(jester.Player.PlayerId);
            CloseWriter(writer);
        }

        // Methods Role
        protected override void InitializeRoleInternal()
        {
            PlayerControl[] players = PlayerControl.AllPlayerControls.ToArray()
               .Where(player =>
                    TryGetSpecialRoleByPlayer(player.PlayerId, out Role role) &&
                    role.Faction == Faction.Crew /* &&   !(role is Mayor || role is Jailor) */).ToArray();
            VoteTarget = players[Rng.Next(0, players.Length)];
        }
    }
}