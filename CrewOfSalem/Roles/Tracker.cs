using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using System.Collections.Generic;

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

        public override string Description => "You can track your target";

        // Methods
        public void SendChatMessage(MessageType type)
        {
            if (AmongUsClient.Instance.AmClient && HudManager.Instance && !Owner.Data.IsDead)
            {
                HudManager.Instance.Chat.AddChat(Owner, messages[type]);
            }
        }

        // Methods Role
        protected override void InitializeAbilities() { }

        // Nested Types
        public enum MessageType
        {
            PlayerEnteredVent,
            PlayerExitedVent,
            PlayerDied
        }
    }
}