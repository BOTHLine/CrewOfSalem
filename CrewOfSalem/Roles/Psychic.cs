using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Psychic : RoleGeneric<Psychic>
    {
        // Fields
        private bool evenMeeting = true;

        // Properties Role
        public override byte   RoleID => 209;
        public override string Name   => nameof(Psychic);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override string Description =>
            "You get 3 or 2 in an alternating pattern. Within 3 names AT LEAST 1 person is evil, within 2 names AT LEAST 1 person is good";

        // Methods
        public void StartMeeting(GameData.PlayerInfo info)
        {
            if (info == null) return;
            if (LocalPlayer.PlayerId != Owner.PlayerId || Owner.Data.IsDead) return;
            evenMeeting = !evenMeeting;

            List<PlayerControl> alivePlayers = AllPlayers
               .Where(player => !player.Data.IsDead && player.PlayerId != Owner.PlayerId).ToArray().ToList();

            PlayerControl[] goodPlayers = alivePlayers.Where(player =>
                player.GetRole().Faction == Faction.Crew || player.GetRole().Alignment == Alignment.Benign).ToArray();

            PlayerControl[] badPlayers = alivePlayers.Where(player => !goodPlayers.Contains(player)).ToArray();

            var shownPlayers = new List<PlayerControl>();

            if (evenMeeting)
            {
                if (goodPlayers.Length == 0)
                {
                    HudManager.Instance.Chat.AddChat(Owner, "The crew is too evil to find anyone good!");
                    return;
                }

                shownPlayers.Add(goodPlayers[Rng.Next(goodPlayers.Length)]);
                alivePlayers.Remove(shownPlayers[0]);
                shownPlayers.Add(alivePlayers[Rng.Next(alivePlayers.Count)]);

                HudManager.Instance.Chat.AddChat(Owner,
                    "A vision revealed that " + RandomizeNameOrder(shownPlayers) + " is good!");
            } else
            {
                if (alivePlayers.Count < 3)
                {
                    HudManager.Instance.Chat.AddChat(Owner, "The crew is too small to accurately find an evildoer!");
                    return;
                }

                shownPlayers.Add(badPlayers[Rng.Next(badPlayers.Length)]);
                alivePlayers.Remove(shownPlayers[0]);
                shownPlayers.Add(alivePlayers[Rng.Next(alivePlayers.Count)]);
                alivePlayers.Remove(shownPlayers[1]);
                shownPlayers.Add((alivePlayers[Rng.Next(alivePlayers.Count)]));

                HudManager.Instance.Chat.AddChat(Owner,
                    "A vision revealed that " + RandomizeNameOrder(shownPlayers) + " is evil!");
            }
        }

        private string RandomizeNameOrder(IEnumerable<PlayerControl> playerControls)
        {
            var result = "";
            string[] names = playerControls.Select(player => player.name).ToArray();
            for (var i = 0; i < names.Length; i++)
            {
                result += names[i];
                if (i < names.Length - 2)
                {
                    result += ", ";
                } else if (i == names.Length - 2)
                {
                    result += " or ";
                }
            }

            return result;
        }

        // Methods Role
        protected override void InitializeAbilities() { }

        protected override void ClearSettingsInternal()
        {
            evenMeeting = true;
        }
    }
}