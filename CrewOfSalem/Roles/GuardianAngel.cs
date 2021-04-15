using System.Collections.Generic;
using System.Linq;
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
        public PlayerControl ProtectTarget { get; private set; }
        
        // Properties Role
        public override byte   RoleID => 238;
        public override string Name   => "Guardian Angel";

        public override Faction   Faction   => Faction.Neutral;
        public override Alignment Alignment => Alignment.Benign;
        public override Color     Color     => Color.white;

        public override string RoleTask => $"Protect {ColorizedText(ProtectTarget.name, Palette.PlayerColors[ProtectTarget.Data.ColorId])}";
        public override string Description => "You can protect your target to prevent the next attack within a given time. You win if they live until they end";

        // Methods Role
        protected override void InitializeAbilities()
        {
            AddAbility<GuardianAngel, AbilityProtect>().ProtectTarget = ProtectTarget;
        }

        protected override void InitializeRoleInternal()
        {
            if (Owner != PlayerControl.LocalPlayer) return;

            var playerControls = new List<PlayerControl>();
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                Role role = player.GetRole();
                switch (role)
                {
                    case GuardianAngel _:
                    case Executioner _:
                    case Jester _:
                        continue;
                    default:
                        playerControls.Add(player);
                        break;
                }
            }

            ProtectTarget = playerControls[Rng.Next(playerControls.Count)];
        }
    }
}