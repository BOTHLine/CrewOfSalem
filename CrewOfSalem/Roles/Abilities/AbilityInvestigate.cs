using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityInvestigate : Ability
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonInvestigate;
        protected override bool   NeedsTarget => true;

        // Constructors
        public AbilityInvestigate(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods
        private static IEnumerable<string> GetResults(Role role)
        {
            switch (role)
            {
                case Vigilante _:
                case Veteran _:
                case Mafioso _:
                // case Pirate _:
                case Ambusher _:
                    return new[] {Vigilante.GetName(), Veteran.GetName(), Mafioso.GetName(), Ambusher.GetName()};

                case Medium _:
                case Janitor _:
                case Retributionist _:
                // case Necromancer _:
                case Trapper _:
                    return new[] {Medium.GetName(), Janitor.GetName(), Retributionist.GetName(), Trapper.GetName()};

                case Survivor _:
                case VampireHunter _:
                case Amnesiac _:
                // case Medusa _:
                case Psychic _:
                    return new[] {Survivor.GetName(), VampireHunter.GetName(), Amnesiac.GetName(), Psychic.GetName()};

                case Spy _:
                case Blackmailer _:
                case Jailor _:
                case GuardianAngel _:
                    return new[] {Spy.GetName(), Blackmailer.GetName(), Jailor.GetName(), GuardianAngel.GetName()};

                case Sheriff _:
                case Executioner _:
                case Werewolf _:
                    // case Poisoner _:
                    return new[] {Sheriff.GetName(), Executioner.GetName(), Werewolf.GetName()};

                case Framer _:
                case Vampire _:
                case Jester _:
                    // case HexMaster _:
                    return new[] {Framer.GetName(), Vampire.GetName(), Jester.GetName()};

                case Lookout _:
                case Forger _:
                    return new[] {Lookout.GetName(), Forger.GetName()};
                // case Juggernaut _:
                // case CovenLeader _:

                case Escort _:
                case Transporter _:
                case Consort _:
                case Hypnotist _:
                    return new[] {Escort.GetName(), Transporter.GetName(), Consort.GetName(), Hypnotist.GetName()};

                case Doctor _:
                case Disguiser _:
                case SerialKiller _:
                    // case PotionMaster _:
                    return new[] {Doctor.GetName(), Disguiser.GetName(), SerialKiller.GetName()};

                case Investigator _:
                case Consigliere _:
                case Mayor _:
                case Tracker _:
                    // case Plaguebearer _:
                    return new[] {Investigator.GetName(), Consigliere.GetName(), Mayor.GetName(), Tracker.GetName()};

                case Bodyguard _:
                case Godfather _:
                case Arsonist _:
                case Crusader _:
                    return new[] {Bodyguard.GetName(), Godfather.GetName(), Arsonist.GetName(), Crusader.GetName()};
            }

            return new string[] { };
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            string result = owner.Owner.name + " could be a(n) ";
            string[] results = GetResults(target.GetRole()).ToArray();
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

            HudManager.Instance.Chat.AddChat(owner.Owner, result);
            sendRpc = setCooldown = true;
        }
    }
}