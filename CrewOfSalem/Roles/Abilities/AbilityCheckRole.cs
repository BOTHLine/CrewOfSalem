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
                case Investigator _: return $"{role.Owner.Data.PlayerName} gathers information about people. They must be an {Investigator.GetName()}.";
                case Lookout _: return $"{role.Owner.Data.PlayerName} watches who visits people at night. They must be a {Lookout.GetName()}.";
                case Psychic _: return $"{role.Owner.Data.PlayerName} has the sight. They must be a {Psychic.GetName()}.";
                case Sheriff _: return $"{role.Owner.Data.PlayerName} is a protector of the {Faction.Crew.Name}. They must be a {Sheriff.GetName()}.";
                case Spy _: return $"{role.Owner.Data.PlayerName} secretly watches who someone visits. They must be a {Spy.GetName()}.";
                case Tracker _: return $"{role.Owner.Data.PlayerName} is a skilled in the art of tracking. They must be a {Tracker.GetName()}.";

                case Jailor _:        return $"{role.Owner.Data.PlayerName} detains people at night. They must be a {Jailor.GetName()}.";
                case VampireHunter _: return $"{role.Owner.Data.PlayerName} tracks Vampires. They must be a {VampireHunter.GetName()}!";
                case Veteran _:       return $"{role.Owner.Data.PlayerName} is a paranoid war hero. They must be a {Veteran.GetName()}.";
                case Vigilante _: return $"{role.Owner.Data.PlayerName} will bend the law to enact justice. They must be a {Vigilante.GetName()}.";

                case Bodyguard _: return $"{role.Owner.Data.PlayerName} is a trained protector. They must be a {Bodyguard.GetName()}.";
                case Doctor _:    return $"{role.Owner.Data.PlayerName} is a professional surgeon. They must be a {Doctor.GetName()}.";
                case Crusader _:  return $"{role.Owner.Data.PlayerName} is a divine protector. They must be a {Crusader.GetName()}.";
                case Trapper _:   return $"{role.Owner.Data.PlayerName} is waiting for a big catch. They must be a {Trapper.GetName()}.";

                case Escort _: return $"{role.Owner.Data.PlayerName} is a beautiful person working for the {Faction.Crew.Name}. They must be an {Escort.GetName()}.";
                case Mayor _: return $"{role.Owner.Data.PlayerName} is the leader of the {Faction.Crew.Name}. They must be the {Mayor.GetName()}.";
                case Medium _: return $"{role.Owner.Data.PlayerName} speaks with the dead. They must be a {Medium.GetName()}.";
                case Retributionist _: return $"{role.Owner.Data.PlayerName} wields mystical powers. They must be a {Retributionist.GetName()}.";
                case Transporter _: return $"{role.Owner.Data.PlayerName} specializes in transportation. They must be a {Transporter.GetName()}.";

                case Disguiser _: return $"{role.Owner.Data.PlayerName} makes other people appear to be someone they're not. They must be a {Disguiser.GetName()}";
                case Forger _: return $"{role.Owner.Data.PlayerName} is good at forging documents. They must be a {Forger.GetName()}.";
                case Framer _: return $"{role.Owner.Data.PlayerName} has a desire to deceive. They must be a {Framer.GetName()}!";
                case Hypnotist _: return $"{role.Owner.Data.PlayerName} is skilled at disrupting others. They must be a {Hypnotist.GetName()}.";
                case Janitor _: return $"{role.Owner.Data.PlayerName} cleans up dead bodies. They must be a {Janitor.GetName()}.";

                case Ambusher _: return $"{role.Owner.Data.PlayerName} lies in wait. They must be an {Ambusher.GetName()}.";
                case Godfather _: return $"{role.Owner.Data.PlayerName} is the leader of the {Faction.Mafia.Name}. They must be the {Godfather.GetName()}.";
                case Mafioso _: return $"{role.Owner.Data.PlayerName} does the {Godfather.GetName()}'s dirty work. They must be a {Mafioso.GetName()}.";

                case Blackmailer _: return $"{role.Owner.Data.PlayerName} uses information to silence people. They must be a {Blackmailer.GetName()}.";
                case Consigliere _: return $"{role.Owner.Data.PlayerName} gathers information for the {Faction.Mafia.Name}. They must be a {Consigliere.GetName()}.";
                case Consort _: return $"{role.Owner.Data.PlayerName} is a beautiful person working for the {Faction.Mafia.Name}. They must be a {Consort.GetName()}.";

                case Amnesiac _: return $"{role.Owner.Data.PlayerName} does not remember their role. They must be an {Amnesiac.GetName()}.";
                case GuardianAngel _: return $"{role.Owner.Data.PlayerName} is watching over someone. They must be a {GuardianAngel.GetName()}.";
                case Survivor _: return $"{role.Owner.Data.PlayerName} simply wants to live. They must be a {Survivor.GetName()}.";

                case Vampire _: return $"{role.Owner.Data.PlayerName} drinks blood. They must be a {Vampire.GetName()}!";

                case Executioner _: return $"{role.Owner.Data.PlayerName} wants someone to be lynched at any cost. They must be an {Executioner.GetName()}.";
                case Jester _: return $"{role.Owner.Data.PlayerName} wants to be lynched. They must be a {Jester.GetName()}.";
                case Witch _:  return $"{role.Owner.Data.PlayerName} casts spells on people. They must be a {Witch.GetName()}.";

                case Arsonist _: return $"{role.Owner.Data.PlayerName} likes to watch things burn. They must be an {Arsonist.GetName()}.";
                case SerialKiller _: return $"{role.Owner.Data.PlayerName} wants to kill everyone. They must be a {SerialKiller.GetName()}";
                case Werewolf _: return $"{role.Owner.Data.PlayerName} howls at the moon. They must be a {Werewolf.GetName()}.";

                default: return "No special role found.";
            }
        }
    }
}