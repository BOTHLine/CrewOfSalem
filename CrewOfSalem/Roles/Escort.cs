using System;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Escort : RoleGeneric<Escort>
    {
        // Properties Role
        protected override byte   RoleID => 221;
        public override    string Name   => nameof(Escort);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Support;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => EscortButton;

        // Methods Role
        protected override bool PerformActionInternal()
        {
            if (TryGetSpecialRoleByPlayer(SpecialButton.Target.PlayerId, out Role role))
            {
                role.SpecialButton.AddCooldown(Duration);
            } else
            {
                SpecialButton.Target.SetKillTimer(SpecialButton.Target.killTimer + Duration);
            }

            MessageWriter writer = GetWriter(RPC.EscortIncreaseCooldown);
            writer.Write(SpecialButton.Target.PlayerId);
            CloseWriter(writer);
            return true;
        }
    }
}