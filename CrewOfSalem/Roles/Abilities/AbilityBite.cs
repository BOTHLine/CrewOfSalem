using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.PlayerControlPatches
{
    public class AbilityBite : Ability
    {
        // Fields
        private static readonly List<PlayerControl> vampires = new List<PlayerControl>();

        // Properties Ability
        protected override Sprite Sprite      => ButtonBite;
        protected override bool   NeedsTarget => true;

        // Constructors
        public AbilityBite(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods
        private void RpcConvertVampire(PlayerControl target)
        {
            if (AmongUsClient.Instance.AmClient) ConvertVampire(target);

            MessageWriter writer = GetWriter(RPC.VampireConvert);
            writer.Write(owner.Owner.PlayerId);
            writer.Write(target.PlayerId);
            CloseWriter(writer);
        }

        public static void ConvertVampire(PlayerControl target)
        {
            vampires.Insert(0, target);
            Role targetRole = target.GetRole();
            targetRole.ClearAbilities();
            targetRole.AddAbility<Vampire, AbilityBite>();
            // TODO: Vielleicht "this" anstatt new AbilityBite übergeben? Andere Vampire sehen dann auch den Cooldown
        }

        public static bool IsVampire(PlayerControl localPlayer)
        {
            return vampires.Contains(localPlayer);
        }

        // Methods Ability
        protected override bool CanUse()
        {
            return base.CanUse() && vampires.FirstOrDefault(vampire => !vampire.Data.IsDead) == owner.Owner;
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            IReadOnlyList<AbilityGuard> abilityGuards = GetAllAbilities<AbilityGuard>();
            foreach (AbilityGuard abilityGuard in abilityGuards)
            {
                if (abilityGuard.owner.Owner == target) continue;
                if (!abilityGuard.IsGuarding || abilityGuard.IsInTask) continue;
                if (!PlayerTools.IsPlayerInRange(abilityGuard.owner.Owner, target)) continue;

                abilityGuard.owner.Owner.RpcMurderPlayer(owner.Owner);
                abilityGuard.owner.Owner.RpcMurderPlayer(abilityGuard.owner.Owner);
                sendRpc = false;
                setCooldown = true;
                return;
            }

            IReadOnlyList<AbilityVest> abilityVests = GetAllAbilities<AbilityVest>();
            foreach (AbilityVest abilityVest in abilityVests)
            {
                if (abilityVest.owner.Owner != target) continue;

                abilityVest.RpcEffectEnd();
                sendRpc = false;
                setCooldown = true;
                return;
            }

            IReadOnlyList<AbilityShield> abilityShields = GetAllAbilities<AbilityShield>();
            foreach (AbilityShield abilityShield in abilityShields)
            {
                if (abilityShield.ShieldedPlayer != target) continue;

                abilityShield.RpcEffectEnd();
                sendRpc = false;
                setCooldown = true;
                return;
            }

            owner.Owner.MurderPlayer(target);
            sendRpc = setCooldown = true;
        }
    }
}