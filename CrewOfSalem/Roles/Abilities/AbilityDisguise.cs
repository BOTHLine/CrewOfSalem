using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityDisguise : AbilityDuration
    {
        // Properties Ability
        protected override Sprite Sprite      => ButtonDisguise;
        protected override bool   NeedsTarget => false;

        protected override RPC               RpcAction => RPC.DisguiseStart;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId};

        protected override RPC               RpcEndAction => RPC.DisguiseEnd;
        protected override IEnumerable<byte> RpcEndData   => new[] {owner.Owner.PlayerId};

        // Constructors
        public AbilityDisguise(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                player.nameText.Text = "";
                player.myRend.material.SetColor(ShaderBackColor, Color.grey);
                player.myRend.material.SetColor(ShaderBodyColor, Color.grey);
                player.HatRenderer.SetHat(0, 0);
                SetSkinWithAnim(player.MyPhysics, 0);
                if (player.CurrentPet) Object.Destroy(player.CurrentPet.gameObject);
            }

            sendRpc = setCooldown = true;
        }

        // Methods AbilityDuration
        protected override void EffectEndInternal()
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                player.SetName(player.Data.PlayerName);
                player.SetHat(player.Data.HatId, player.Data.ColorId);
                SetSkinWithAnim(player.MyPhysics, player.Data.SkinId);
                player.SetPet(player.Data.PetId);
                player.CurrentPet.Visible = player.Visible;
                player.SetColor(player.Data.ColorId);
            }
        }
    }
}