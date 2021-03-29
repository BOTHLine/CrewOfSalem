using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrewOfSalem.Roles.Alignments;
using CrewOfSalem.Roles.Factions;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.Roles
{
    public readonly struct RoleSlot
    {
        // Fields
        private readonly Faction   faction;
        private readonly Alignment alignment;
        private readonly Role      role;

        // Properties
        public bool IsInfected => faction == Faction.Mafia || faction == Faction.Coven ||
                                  role?.Faction == Faction.Mafia || role?.Faction == Faction.Coven;

        // Constructors
        public RoleSlot(Faction faction)
        {
            this.faction = faction;
            alignment = null;
            role = null;
        }

        public RoleSlot(Faction faction, Alignment alignment)
        {
            this.faction = faction;
            this.alignment = alignment;
            role = null;
        }

        public RoleSlot(Role role)
        {
            faction = null;
            alignment = null;
            this.role = role;
        }

        // Methods
        [SuppressMessage("ReSharper", "LocalVariableHidesMember")]
        public Role GetRole(ref List<Role> availableRoles)
        {
            if (faction == null)
            {
                availableRoles.Remove(role);
                return role;
            }

            if (alignment == null)
            {
                Faction faction = this.faction;
                Role[] possibleRoles = availableRoles.Where(r => r.Faction == faction).ToArray();
                Role role = possibleRoles[Rng.Next(0, possibleRoles.Length)];
                availableRoles.Remove(role);
                return role;
            } else
            {
                Faction faction = this.faction;
                Alignment alignment = this.alignment;
                Role[] possibleRoles = availableRoles.Where(r => r.Faction == faction && r.Alignment == alignment)
                   .ToArray();
                Role role = possibleRoles[Rng.Next(0, possibleRoles.Length)];
                availableRoles.Remove(role);
                return role;
            }
        }
    }
}