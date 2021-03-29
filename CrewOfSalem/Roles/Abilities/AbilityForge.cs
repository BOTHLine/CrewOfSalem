using System.Collections.Generic;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityForge : AbilityDuration
    {
        // Fields
        private PlayerControl currentSample;

        // Properties Ability
        protected override Sprite Sprite => currentSample == null ? ButtonSteal : ButtonForge;

        protected override bool NeedsTarget => currentSample == null;

        protected override RPC               RpcAction => RPC.ForgeStart;
        protected override IEnumerable<byte> RpcData   => new[] {owner.Owner.PlayerId, currentSample.PlayerId};

        protected override RPC               RpcEndAction => RPC.ForgeEnd;
        protected override IEnumerable<byte> RpcEndData   => new[] {owner.Owner.PlayerId};

        // Constructors
        public AbilityForge(Role owner, float cooldown, float duration) : base(owner, cooldown, duration) { }

        // Methods
        private void ForgeStart(PlayerControl target)
        {
            owner.Owner.nameText.Text = target.Data.PlayerName;
            owner.Owner.myRend.material.SetColor(ShaderBackColor, Palette.ShadowColors[target.Data.ColorId]);
            owner.Owner.myRend.material.SetColor(ShaderBodyColor, Palette.PlayerColors[target.Data.ColorId]);
            owner.Owner.HatRenderer.SetHat(target.Data.HatId, target.Data.ColorId);
            owner.Owner.nameText.transform.localPosition =
                new Vector3(0F, target.Data.HatId == 0U ? 0.7F : 1.05F, -0.5F);

            if (owner.Owner.MyPhysics.Skin.skin.ProdId != HatManager.Instance.AllSkins[(int) target.Data.SkinId].ProdId)
            {
                SetSkinWithAnim(owner.Owner.MyPhysics, target.Data.SkinId);
            }

            if (owner.Owner.CurrentPet == null ||
                owner.Owner.CurrentPet.ProdId != HatManager.Instance.AllPets[(int) target.Data.PetId].ProdId)
            {
                if (owner.Owner.CurrentPet) Object.Destroy(owner.Owner.CurrentPet.gameObject);
                owner.Owner.CurrentPet = Object.Instantiate(HatManager.Instance.AllPets[(int) target.Data.PetId]);
                owner.Owner.CurrentPet.transform.position = owner.Owner.transform.position;
                owner.Owner.CurrentPet.Source = owner.Owner;
                owner.Owner.CurrentPet.Visible = owner.Owner.Visible;
                PlayerControl.SetPlayerMaterialColors(target.Data.ColorId, owner.Owner.CurrentPet.rend);
            } else if (owner.Owner.CurrentPet)
            {
                PlayerControl.SetPlayerMaterialColors(target.Data.ColorId, owner.Owner.CurrentPet.rend);
            }
        }

        private void ForgeEnd()
        {
            currentSample = null;
            owner.Owner.SetName(owner.Owner.Data.PlayerName);
            owner.Owner.SetHat(owner.Owner.Data.HatId, owner.Owner.Data.ColorId);
            SetSkinWithAnim(owner.Owner.MyPhysics, owner.Owner.Data.SkinId);
            owner.Owner.SetPet(owner.Owner.Data.PetId);
            owner.Owner.CurrentPet.Visible = owner.Owner.Visible;
            owner.Owner.SetColor(owner.Owner.Data.ColorId);
        }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            sendRpc = setCooldown = false;
            if (NeedsTarget && target == null) return;
            if (currentSample == null)
            {
                currentSample = target;
                CurrentCooldown = 1F;
                isEffectActive = false;
                return;
            } else
            {
                ForgeStart(currentSample);
                sendRpc = setCooldown = true;
            }
        }

        // Methods AbilityDuration
        protected override void EffectEndInternal()
        {
            ForgeEnd();
        }
    }
}