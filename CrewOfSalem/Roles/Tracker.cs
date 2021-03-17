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
        protected override byte   RoleID => 212;
        public override    string Name   => nameof(Tracker);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        protected override bool   HasSpecialButton    => false;
        protected override Sprite SpecialButtonSprite => null;

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

        public override bool PerformAction(PlayerControl target)
        {
            // TODO
            return true;
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