using CrewOfSalem.Extensions;
using CrewOfSalem.Roles.Factions;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityCheckRole : Ability
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonInvestigate;
        protected override bool   NeedsTarget => true;

        // Constructors
        public AbilityCheckRole(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            string result = GetResult(target.GetRole());
            HudManager.Instance.Chat.AddChat(owner.Owner, result);

            sendRpc = setCooldown = true;
        }

        // Methods
        private static string GetResult(Role role)
        {
            switch (role)
            {
                case Investigator _:
                    return $"Your target gathers information about people. They must be an {Investigator.GetName()}.";
                case Lookout _:
                    return $"Your target watches who visits people at night. They must be a {Lookout.GetName()}.";
                case Psychic _: return $"Your target has the sight. They must be a {Psychic.GetName()}.";
                case Sheriff _:
                    return
                        $"Your target is a protector of the {Faction.Crew.Name}. They must be a {Sheriff.GetName()}.";
                case Spy _: return $"Your target secretly watches who someone visits. They must be a {Spy.GetName()}.";
                case Tracker _:
                    return $"Your target is a skilled in the art of tracking. They must be a {Tracker.GetName()}.";

                case Jailor _:        return $"Your target detains people at night. They must be a {Jailor.GetName()}.";
                case VampireHunter _: return $"Your target tracks Vampires. They must be a {VampireHunter.GetName()}!";
                case Veteran _:       return $"Your target is a paranoid war hero. They must be a {Veteran.GetName()}.";
                case Vigilante _:
                    return $"Your target will bend the law to enact justice. They must be a {Vigilante.GetName()}.";

                case Bodyguard _: return $"Your target is a trained protector. They must be a {Bodyguard.GetName()}.";
                case Doctor _:    return $"Your target is a professional surgeon. They must be a {Doctor.GetName()}.";
                case Crusader _:  return $"Your target is a divine protector. They must be a {Crusader.GetName()}.";
                case Trapper _:   return $"Your target is waiting for a big catch. They must be a {Trapper.GetName()}.";

                case Escort _:
                    return
                        $"Your target is a beautiful person working for the {Faction.Crew.Name}. They must be an {Escort.GetName()}.";
                case Mayor _:
                    return $"Your target is the leader of the {Faction.Crew.Name}. They must be the {Mayor.GetName()}.";
                case Medium _: return $"Your target speaks with the dead. They must be a {Medium.GetName()}.";
                case Retributionist _:
                    return $"Your target wields mystical powers. They must be a {Retributionist.GetName()}.";
                case Transporter _:
                    return $"Your target specializes in transportation. They must be a {Transporter.GetName()}.";

                case Disguiser _:
                    return
                        $"Your target makes other people appear to be someone they're not. They must be a {Disguiser.GetName()}";
                case Forger _: return $"Your target is good at forging documents. They must be a {Forger.GetName()}.";
                case Framer _: return $"Your target has a desire to deceive. They must be a {Framer.GetName()}!";
                case Hypnotist _:
                    return $"Your target is skilled at disrupting others. They must be a {Hypnotist.GetName()}.";
                case Janitor _: return $"Your target cleans up dead bodies. They must be a {Janitor.GetName()}.";

                case Ambusher _: return $"Your target lies in wait. They must be an {Ambusher.GetName()}.";
                case Godfather _:
                    return
                        $"Your target is the leader of the {Faction.Mafia.Name}. They must be the {Godfather.GetName()}.";
                case Mafioso _:
                    return
                        $"Your target does the {Godfather.GetName()}'s dirty work. They must be a {Mafioso.GetName()}.";

                case Blackmailer _:
                    return $"Your target uses information to silence people. They must be a {Blackmailer.GetName()}.";
                case Consigliere _:
                    return
                        $"Your target gathers information for the {Faction.Mafia.Name}. They must be a {Consigliere.GetName()}.";
                case Consort _:
                    return
                        $"Your target is a beautiful person working for the {Faction.Mafia.Name}. They must be a {Consort.GetName()}.";

                case Amnesiac _:
                    return $"Your target does not remember their role. They must be an {Amnesiac.GetName()}.";
                case GuardianAngel _:
                    return $"Your target is watching over someone. They must be a {GuardianAngel.GetName()}.";
                case Survivor _: return $"Your target simply wants to live. They must be a {Survivor.GetName()}.";

                case Vampire _: return $"Your target drinks blood. They must be a {Vampire.GetName()}!";

                case Executioner _:
                    return
                        $"Your target wants someone to be lynched at any cost. They must be an {Executioner.GetName()}.";
                case Jester _: return $"Your target wants to be lynched. They must be a {Jester.GetName()}.";
                case Witch _:  return $"Your target casts spells on people. They must be a {Witch.GetName()}.";

                case Arsonist _:
                    return $"Your target likes to watch things burn. They must be an {Arsonist.GetName()}.";
                case SerialKiller _:
                    return $"Your target wants to kill everyone. They must be a {SerialKiller.GetName()}";
                case Werewolf _: return $"Your target howls at the moon. They must be a {Werewolf.GetName()}.";

                default: return "No special role found.";
            }
        }
    }
}