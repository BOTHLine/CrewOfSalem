using System.Collections.Generic;
using System.Linq;
using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Abilities;
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

            WriteRPC(RPC.VampireConvert, owner.Owner.PlayerId, target.PlayerId);
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

        protected override void UpdateTarget()
        {
            Button.SetTarget(LocalPlayer == owner.Owner ? PlayerTools.FindClosestTarget(owner.Owner, player => !vampires.Contains(player)) : null);
        }

        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            // TODO: Bite convert instead of always kill
            owner.Owner.RpcKillPlayer(target, owner.Owner);
            sendRpc = setCooldown = true;
        }
    }
}