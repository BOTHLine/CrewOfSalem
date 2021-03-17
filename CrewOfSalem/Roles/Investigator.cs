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
        public override byte   RoleID => 207;
        public override string Name   => nameof(Investigator);

        public override Faction   Faction   => Faction.Crew;
        public override Alignment Alignment => Alignment.Investigative;

        public override bool   HasSpecialButton => true;
        public override Sprite SpecialButton    => InvestigatorButton;

        // Methods
        private static IEnumerable<string> GetResults(Role role)
        {
            switch (role)
            {
                case Vigilante _:
                case Veteran _:
                    // Mafioso
                    // Pirate
                    // Ambusher
                    return new[] {Vigilante.GetName(), Veteran.GetName()};

                // Medium
                // Janitor
                // Retributionist
                // Necromancer
                // Trapper

                // Survivor
                // Vampire Hunter
                // Amnesiac
                // Medusa
                case Psychic _:
                    return new[] {Psychic.GetName()};

                case Spy _:
                    // Blackmailer
                    // Jailor
                    // Guardian Angel
                    return new[] {Spy.GetName()};

                case Sheriff _:
                    // Executioner
                    // Werewolf
                    // Poisoner
                    return new[] {Sheriff.GetName()};

                // Framer
                // Vampire
                case Jester _:
                    // Hex Master
                    return new[] {Jester.GetName()};

                // Lookout
                // Forager
                // Juggernaut
                // Coven Leader

                case Escort _:
                    // Transporter
                    // Consort
                    // Hypnotist
                    return new[] {Escort.GetName()};

                case Doctor _:
                    // Disguiser
                    // Serial Killer
                    // Potion Master
                    return new[] {Doctor.GetName()};

                case Investigator _:
                    // Consigliere
                    // Mayor
                    // Tracker
                    // Plaguebearer
                    return new[] {Investigator.GetName()};

                // Bodyguard
                // Godfather
                // Arsonist
                // Crusader
            }

            return new string[] { };
        }

        // Methods Role
        public override void PerformAction(KillButtonManager instance)
        {
            if (instance.isCoolingDown) return;

            PlayerControl target = PlayerTools.FindClosestTarget(Player);
            if (target == null) return;

            Player.SetKillTimer(Cooldown);

            string result = Player.name + " could be a(n) ";
            string[] results = GetResults(GetSpecialRoleByPlayer(target.PlayerId)).ToArray();
            for (int i = 0; i < results.Length; i++)
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

            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(Player, result);
        }
    }
}