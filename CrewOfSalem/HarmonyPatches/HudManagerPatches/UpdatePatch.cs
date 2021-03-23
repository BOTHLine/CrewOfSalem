using CrewOfSalem.Roles;
using CrewOfSalem.Roles.Factions;
using HarmonyLib;
using UnityEngine;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.HudManagerPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class UpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            PlayerControl localPlayer = PlayerControl.LocalPlayer;

            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            defaultKillButton ??= __instance.KillButton.renderer.sprite;

            /*
            if (localPlayer.Data.IsImpostor)
            {
                __instance.KillButton.gameObject.SetActive(true);
                __instance.KillButton.renderer.enabled = true;
                __instance.KillButton.renderer.sprite = defaultKillButton;
            }
            */

            /*
            bool lastQ = Input.GetKeyUp(KeyCode.Q);
            if (!localPlayer.Data.IsImpostor && Input.GetKeyDown(KeyCode.Q) && !lastQ &&
                __instance.UseButton.isActiveAndEnabled)
            {
                PerformKillPatch.Prefix(null);
            }
            */

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                player.nameText.Color = Color.white;
            }

            // Jester Tasks have to be reset every frame.. maybe rework later?
            // GetSpecialRole<Executioner>()?.ClearTasks();
            // GetSpecialRole<Jester>()?.ClearTasks();

            GetSpecialRole<Doctor>()?.CheckShowShieldedPlayer();

            if (TryGetSpecialRoleByPlayer(localPlayer.PlayerId, out Role role))
            {
                role.SetNameColor();
                role.UpdateButton();
                role.UpdateDuration(Time.deltaTime);
                switch (role)
                {
                    // TODO: Add all Roles
                    case Jester jester:
                        if (jester.CanSeeImpostor) { }

                        break;
                }
            }

            // Add Mafia / Coven / Lover Chat
            if (role?.Faction == Faction.Mafia || role?.Faction == Faction.Coven || role is Spy)
            {
                if (!__instance.Chat.isActiveAndEnabled)
                {
                    __instance.Chat.SetVisible(true);
                }
            }

            UpdatePlayerNames();
        }

        // TODO: Currently working with PhysicsHelpers.AnyNonTriggersBetween, change to something like "Vision" later?
        private static void UpdatePlayerNames()
        {
            int showPlayerNames = Main.OptionShowPlayerNames.GetValue();
            if (showPlayerNames != 1) return;
            Vector2 fromPosition = PlayerControl.LocalPlayer.GetTruePosition();

            PlayerControl[] allPlayers = PlayerControl.AllPlayerControls.ToArray();
            foreach (PlayerControl player in allPlayers)
            {
                Vector2 distanceVector = player.GetTruePosition() - fromPosition;
                float distance = distanceVector.magnitude;
                if (PhysicsHelpers.AnyNonTriggersBetween(fromPosition, distanceVector.normalized, distance,
                    Constants.ShipOnlyMask))
                {
                    player.nameText.Text = "";
                } else
                {
                    player.nameText.Text = player.name;
                }
            }
        }
    }
}