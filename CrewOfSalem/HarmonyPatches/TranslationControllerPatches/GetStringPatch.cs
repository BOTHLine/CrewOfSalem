using CrewOfSalem.Roles;
using HarmonyLib;
using UnhollowerBaseLib;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.TranslationControllerPatches
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new System.Type[] {typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>)})]
    public static class GetStringPatch
    {
        public static void Postfix(ref string __result, StringNames DKEHCKOHMOH,
            Il2CppReferenceArray<Il2CppSystem.Object> DKBJCINDDCD)
        {
            if (ExileController.Instance == null || ExileController.Instance.exiled == null) return;

            byte playerId = ExileController.Instance.exiled.Object.PlayerId;
            TryGetSpecialRoleByPlayer(playerId, out Role role);

            switch (DKEHCKOHMOH) {
                case StringNames.ExileTextPN:
                case StringNames.ExileTextSN: {
                    string playerName = ExileController.Instance.exiled.PlayerName;
                    if (role != null) {
                        __result = role.EjectMessage(playerName);
                    } else {
                        __result = playerName + " was not The Impostor.";
                    }

                    break;
                }
                case StringNames.ImpostorsRemainP:
                case StringNames.ImpostorsRemainS: {
                    if (role is Jester) {
                        __result = "";
                    }

                    break;
                }
            }
        }
    }
}