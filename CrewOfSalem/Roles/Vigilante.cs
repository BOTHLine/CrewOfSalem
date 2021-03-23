﻿using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Vigilante : RoleGeneric<Vigilante>
    {
        // Properties Role
        protected override byte   RoleID => 216;
        public override    string Name   => nameof(Vigilante);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Killing;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => VigilanteButton;

        // Methods
        public void KillPlayer(PlayerControl target)
        {
            MessageWriter writer = GetWriter(RPC.VigilanteKill);
            writer.Write(Player.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
            Player.MurderPlayer(target);
        }

        private bool IsKillable(PlayerControl target)
        {
            TryGetSpecialRoleByPlayer(target.PlayerId, out Role role);
            return role.Faction == Faction.Mafia || role.Faction == Faction.Neutral || role.Faction == Faction.Coven;
        }

        // Methods Role
        // TODO: Move "KillPlayer" And DoctorShield-Checks etc. to global method?
        // TODO: Create "Die()" Method to handle something like BreakShield for Medic (If it should apply on death)
        protected override bool PerformActionInternal()
        {
            TryGetSpecialRoleByPlayer(SpecialButton.Target.PlayerId, out Role targetRole);
            switch (targetRole)
            {
                case Survivor {HasDurationLeft: true}:
                    return true;
                case Doctor doctor when doctor.ShieldedPlayer?.PlayerId == SpecialButton.Target.PlayerId:
                    doctor.BreakShield();
                    return true;
                default:
                    KillPlayer(IsKillable(SpecialButton.Target) ? SpecialButton.Target : Player);
                    break;
            }

            return true;
        }
    }
}