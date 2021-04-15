using CrewOfSalem.Extensions;
using CrewOfSalem.Roles;
using HarmonyLib;
using UnhollowerBaseLib;

namespace CrewOfSalem.HarmonyPatches.TranslationControllerPatches
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(StringNames),
        typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
    public static class GetStringPatch
    {
        public static void Postfix(ref string __result, [HarmonyArgument(0)] StringNames id,
            [HarmonyArgument(1)] Il2CppReferenceArray<Il2CppSystem.Object> parts)
        {
            if (ExileController.Instance == null || ExileController.Instance.exiled == null) return;

            byte playerId = ExileController.Instance.exiled.Object.PlayerId;
            Role role = ExileController.Instance.exiled.Object.GetRole();

            switch (id)
            {
                case StringNames.ExileTextPN:
                case StringNames.ExileTextSN:
                {
                    string playerName = ExileController.Instance.exiled.PlayerName;

                    if (role != null)
                        __result = role.EjectMessage(playerName);
                    else
                        __result = playerName + " was not The Impostor.";

                    break;
                }
                case StringNames.ImpostorsRemainP:
                case StringNames.ImpostorsRemainS:
                    break;
            }
        }
    }
}