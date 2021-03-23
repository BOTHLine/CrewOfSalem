using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public class Investigator : RoleGeneric<Investigator>
    {
        // Properties Role
        protected override byte   RoleID => 207;
        public override    string Name   => nameof(Investigator);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        protected override bool   HasSpecialButton    => true;
        protected override Sprite SpecialButtonSprite => InvestigatorButton;

        // Methods
        private static IEnumerable<string> GetResults(Role role)
        {
            switch (role)
            {
                case Vigilante _:
                case Veteran _:
                case Mafioso _:
                    // case Pirate _:
                    // case Ambusher _:
                    return new[] {Vigilante.GetName(), Veteran.GetName(), Mafioso.GetName()};

                // case Medium _:
                // case Janitor _:
                // case Retributionist _:
                // case Necromancer _:
                // case Trapper _:
                // return new [] {};

                case Survivor _:
                // case Vampire Hunter _:
                // case Amnesiac _:
                // case Medusa _:
                case Psychic _:
                    return new[] {Psychic.GetName()};

                case Spy _:
                    // case Blackmailer _:
                    // case Jailor _:
                    // case Guardian Angel _:
                    return new[] {Spy.GetName()};

                case Sheriff _:
                case Executioner _:
                    // case Werewolf _:
                    // case Poisoner _: 
                    return new[] {Sheriff.GetName(), Executioner.GetName()};

                // case Framer _:
                // case Vampire _:
                case Jester _:
                    // case Hex Master _:
                    return new[] {Jester.GetName()};

                // case Lookout _:
                // case Forager _:
                // case Juggernaut _:
                // case Coven Leader _:

                case Escort _:
                    // case Transporter _:
                    // case Consort _:
                    // case Hypnotist _:
                    return new[] {Escort.GetName()};

                case Doctor _:
                    // case Disguiser _:
                    // case Serial Killer _:
                    // case Potion Master _:
                    return new[] {Doctor.GetName()};

                case Investigator _:
                    // case Consigliere _:
                    // case Mayor _:
                    // case Tracker _:
                    // case Plaguebearer _:
                    return new[] {GetName()};

                // case Bodyguard _:
                // case Godfather _:
                // case Arsonist _:
                // case Crusader _:
            }

            return new string[] { };
        }

        // Methods Role
        protected override bool PerformActionInternal()
        {
            string result = Player.name + " could be a(n) ";
            string[] results = GetResults(GetSpecialRoleByPlayer(SpecialButton.Target.PlayerId)).ToArray();
            for (var i = 0; i < results.Length; i++)
            {
                result += results[i];
                if (i < results.Length - 2)
                {
                    result += ", ";
                } else if (i == results.Length - 2)
                {
                    result += " or ";
                }
            }

            HudManager.Instance.Chat.AddChat(Player, result);
            return true;
        }
    }
}