using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityVitals : Ability
    {
        // Fields
        private Minigame minigamePrefab;

        // Properties
        private Minigame MinigamePrefab => minigamePrefab ??=
            GameObject.Find("panel_vitals")?.GetComponent<SystemConsole>()?.MinigamePrefab;

        // Properties Ability
        protected override Sprite Sprite      => ButtonVitals;
        protected override bool   NeedsTarget => false;

        // Constructors
        public AbilityVitals(Role owner, float cooldown) : base(owner, cooldown) { }

        // Methods Ability
        protected override void UseInternal(PlayerControl target, out bool sendRpc, out bool setCooldown)
        {
            PlayerControl.LocalPlayer.NetTransform.Halt();
            Minigame minigame = Object.Instantiate(MinigamePrefab, Camera.main.transform, false);
            minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
            minigame.Begin(null);
            sendRpc = false;
            setCooldown = true;

            if (!Main.OptionLookoutSharesCooldown) return;

            foreach (Ability ability in owner.GetAllAbilities())
            {
                ability.SetCooldown(Cooldown);
            }
        }
    }
}