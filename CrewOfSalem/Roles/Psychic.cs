using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Psychic : RoleGeneric<Psychic>
    {
        // Fields
        private bool evenMeeting = true;

        // Properties Role
        public override byte RoleID => 209;
        public override string Name => nameof(Psychic);

        public override Faction Faction => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override bool HasSpecialButton => false;
        public override Sprite SpecialButton => null;

        // Methods
        public void StartMeeting()
        {
            evenMeeting = !evenMeeting;
            if (PlayerControl.LocalPlayer.PlayerId != Player.PlayerId || Player.Data.IsDead) return;

            List<PlayerControl> alivePlayers = PlayerControl.AllPlayerControls.ToArray()
                .Where(player => !player.Data.IsDead && player.PlayerId != Player.PlayerId).ToArray().ToList();

            PlayerControl[] goodPlayers = alivePlayers.Where(player =>
                GetSpecialRoleByPlayer(player.PlayerId)?.Faction == Faction.Crew ||
                GetSpecialRoleByPlayer(player.PlayerId)?.Alignment == Alignment.Benign).ToArray();

            PlayerControl[] badPlayers = alivePlayers.Where(player => !goodPlayers.Contains(player)).ToArray();

            var shownPlayers = new List<PlayerControl>();

            if (evenMeeting) {
                if (goodPlayers.Length == 0) {
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player,
                        "The town is too evil to find anyone good!");
                    return;
                }

                shownPlayers.Add(goodPlayers[Rng.Next(goodPlayers.Length)]);
                alivePlayers.Remove(shownPlayers[0]);
                shownPlayers.Add(alivePlayers[Rng.Next(alivePlayers.Count)]);

                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player,
                    "A vision revealed that " + RandomizeNameOrder(shownPlayers) + " is good!");
            } else {
                if (alivePlayers.Count < 3) {
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player,
                        "The town is too small to accurately find an evildoer!");
                    return;
                }

                shownPlayers.Add(badPlayers[Rng.Next(badPlayers.Length)]);
                alivePlayers.Remove(shownPlayers[0]);
                shownPlayers.Add(alivePlayers[Rng.Next(alivePlayers.Count)]);
                alivePlayers.Remove(shownPlayers[1]);
                shownPlayers.Add((alivePlayers[Rng.Next(alivePlayers.Count)]));

                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player,
                    "A vision revealed that " + RandomizeNameOrder(shownPlayers) + " is evil!");
            }
        }

        private string RandomizeNameOrder(IEnumerable<PlayerControl> playerControls)
        {
            string result = "";
            string[] names = playerControls.Select(player => player.name).ToArray();
            for (int i = 0; i < names.Length; i++) {
                result += names[i];
                if (i < names.Length - 2) {
                    result += ", ";
                } else if (i == names.Length - 2) {
                    result += " or ";
                }
            }

            return result;
        }

        // Methods Role
        protected override void ClearSettings()
        {
            evenMeeting = true;
        }
    }
}