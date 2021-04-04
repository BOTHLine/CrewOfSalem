/*
using BepInEx;
using BepInEx.IL2CPP;
using Essentials.Options;
using HarmonyLib;
using Reactor;

[BepInPlugin(Id, Name, Version)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public class Main : BasePlugin
{
    private const string  Id      = "gg.reactor.ExceptionExamplePlugin";
    private const string  Name    = "Exception Example Plugin";
    private const string  Version = "0.1";
    private       Harmony Harmony { get; } = new Harmony(Id);

    private static readonly CustomOption CustomOption1 = CustomOption.AddToggle("Example Option 1", true);
    private static readonly CustomOption CustomOption2 = CustomOption.AddToggle("Example Option 2", true);

    public override void Load()
    {
        CustomOption1.HudVisible = CustomOption1.MenuVisible = true;
        CustomOption2.HudVisible = CustomOption2.MenuVisible = false;

        Harmony.PatchAll();
    }

    public static void TurnPage()
    {
        bool visible = CustomOption1.HudVisible;

        CustomOption1.HudVisible = CustomOption1.MenuVisible = !visible;
        CustomOption2.HudVisible = CustomOption2.MenuVisible = visible;

        UnityEngine.Object.FindObjectOfType<GameOptionsMenu>()?.Start();
    }
}
*/