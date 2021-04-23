using System.Collections.Generic;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Abilities;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class GuardianAngel : RoleGeneric<GuardianAngel>
    {
        // Properties
        public PlayerControl ProtectTarget { get; set; }

        // Properties Role
        public override byte   RoleID => 238;
        public override string Name   => "Guardian Angel";

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;
        public override Color     Color     => Color.white;

        public override string RoleTask => $"Protect {ColorizedText(ProtectTarget.name, ProtectTarget.GetPlayerColor())}";

        public override string Description => "You can protect your target to prevent the next attack within a given time. You win if they live until they end";

        // Methods
        public void TurnIntoSurvivor()
        {
            /*
            var task = Owner.myTasks.ToArray()[0] as ImportantTextTask;
            if (task != null) task.Text = Jester.GetRoleTask();
            isJester = true;
            */
            PlayerControl player = Owner;
            ClearSettings();
            AddRole(Survivor.Instance, player);

            Survivor.Instance.InitializeRole();
            
            player.ClearTasksCustom();
            Survivor.Instance.SetRoleTask();
            Survivor.Instance.ClearAbilities();

            WriteRPC(RPC.SetRole, Survivor.GetRoleID(), player.PlayerId);
        }

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<GuardianAngel, AbilityProtect>().ProtectTarget = ProtectTarget;
        }

        protected override void InitializeRoleInternal()
        {
            if (Owner != LocalPlayer) return;

            var players = new List<PlayerControl>();
            foreach (PlayerControl player in AllPlayers)
            {
                Role role = player.GetRole();
                switch (role)
                {
                    case GuardianAngel _:
                    case Executioner _:
                    case Jester _:
                        continue;
                    default:
                        players.Add(player);
                        break;
                }
            }

            ProtectTarget = players[Rng.Next(players.Count)];

            WriteRPC(RPC.GuardianAngelTarget, ProtectTarget.PlayerId);
        }

        protected override void ClearSettingsInternal()
        {
            ProtectTarget = null;
        }
    }
}