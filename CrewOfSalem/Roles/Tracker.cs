using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;
using UnityEngine;

namespace CrewOfSalem.Roles
{
    public class Tracker : RoleGeneric<Tracker>
    {
        // Fields
        private readonly Dictionary<MessageType, string> messages = new Dictionary<MessageType, string>()
        {
            {MessageType.PlayerEnteredVent, "Someone entered a vent..."},
            {MessageType.PlayerExitedVent, "Someone exited a vent..."},
            {MessageType.PlayerDied, "Someone died..."}
        };

        // Properties Role
        public override byte   RoleID => 212;
        public override string Name   => nameof(Tracker);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override bool   HasSpecialButton => false;
        public override Sprite SpecialButton    => null;

        // Methods
        public void SendChatMessage(MessageType type)
        {
            if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance && !Player.Data.IsDead)
            {
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player, messages[type]);
            }
        }

        // Methods Role
        protected override void SetConfigSettings() { }

        public override void PerformAction(KillButtonManager instance)
        {
            // TODO
        }

        protected override void ClearSettings()
        {
            // TODO
        }

        // Nested Types
        public enum MessageType
        {
            PlayerEnteredVent,
            PlayerExitedVent,
            PlayerDied
        }
    }
}