using HarmonyLib;
using Hazel;
using static CrewOfSalem.CrewOfSalem;

namespace CrewOfSalem.HarmonyPatches.GeneralPatches
{
    [HarmonyPatch(typeof(MedScanSystem), nameof(MedScanSystem.Deserialize))]
    public static class MedScanSystemDeserializePatch
    {
        public static bool Prefix(MedScanSystem __instance, [HarmonyArgument(0)] MessageReader reader)
        {
            if (!Main.OptionRemoveMedbayProof) return true;
            
            __instance.UsersList.Clear();
            int num = reader.ReadPackedInt32();
            for (var i = 0; i < num; i++)
            {
                byte id = reader.ReadByte();
                if (id == LocalPlayer.PlayerId) __instance.UsersList.Add(id);
            }

            return false;
        }
    }
}