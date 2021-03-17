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
        public override bool PerformAction(PlayerControl target)
        {
            if (target == null) return false;

            if (TryGetSpecialRoleByPlayer(target.PlayerId, out Role role)) role.SpecialButton.AddCooldown(Duration);
            else target.SetKillTimer(target.killTimer + Duration);

            MessageWriter writer = GetWriter(RPC.EscortIncreaseCooldown);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            return true;
        }
    }
}