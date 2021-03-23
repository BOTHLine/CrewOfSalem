using Hazel;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;
using static CrewOfSalem.Main;

namespace CrewOfSalem.Roles.Abilities
{
    public class AbilityDoctorShield : AbilityDuration<Doctor>
    {
        
        
        // Properties Ability
        protected override bool NeedsTarget => true;

        // Constructors
        public AbilityDoctorShield(Vector3 offset) : base(GetSpecialRole<Doctor>(), GetRoleCooldown<Doctor>(),
            GetRoleDuration<Doctor>(), DoctorButton, offset) { }

        // Methods
        protected override void UseInternal()
        {
            role.ShieldedPlayer = buttonManager.CurrentTarget;

            MessageWriter writer = GetWriter(RPC.DoctorSetShielded);
            writer.Write(role.ShieldedPlayer.PlayerId);
            CloseWriter(writer);
        }
    }
}