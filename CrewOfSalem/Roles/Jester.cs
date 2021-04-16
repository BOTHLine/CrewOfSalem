using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
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

        public override Color Color => new Color(244F / 255F, 159F / 255F, 208F / 255F, 1F);

        public override string RoleTask    => $"{base.RoleTask} to vote {ColorizedText("you", Color)}";
        public override string Description => "You have to trick everyone else to vote you";

        // Methods
        public void RpcWin()
        {
            if (AmongUsClient.Instance.AmClient) Win();
            
            WriteRPC(RPC.JesterWin);
        }

        public void Win()
        {
            foreach (PlayerControl player in AllPlayers)
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