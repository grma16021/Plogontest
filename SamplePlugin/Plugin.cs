using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization.Metadata;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Command;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using StrongVibes.Windows;
using Lumina.Excel.Sheets;

namespace StrongVibes;


public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    
    
    private const string CommandName = "/pmycommand";
    private const string PersonalTestCommand = "/mytest";
    private ISharedImmediateTexture iconTexture;

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("StrongVibes");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    private TestWindow TestWindow { get; init; }
    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        // You might normally want to embed resources and load them from the manifest stream
        var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
        var fingerImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "finger.png");
        iconTexture = TextureProvider.GetFromGameIcon(new GameIconLookup(3406));
        ConfigWindow = new ConfigWindow(this);
        
        PluginInterface.UiBuilder.Draw += DrawUI;
        
        MainWindow = new MainWindow(this, goatImagePath, iconTexture);
        
        TestWindow = new TestWindow(this, fingerImagePath);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(TestWindow);

        var testLookup = new GameIconLookup(3406);
        //var icon = TextureProvider.GetFromGameIcon(testLookup);

        Framework.Update += OnFrameworkTick;
        
        
        var lookup = new GameIconLookup
        {
            IconId = 060411,
            ItemHq = false,
            HiRes = true,
            Language = null
        };
        
        //var jobIconTexture = textureProvider.GetFromGameIcon(lookup);
        
        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        CommandManager.AddHandler(PersonalTestCommand, new CommandInfo(TestCommand)
        {
            HelpMessage = "This is a second command made by myself"
        });

        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        // This adds a button to the plugin installer entry of this plugin which allows
        // toggling the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;

        // Adds another button doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;

        // Add a simple message to the log with level set to information
        // Use /xllog to open the log window in-game
        // Example Output: 00:57:54.959 | INF | [SamplePlugin] ===A cool log message from Sample Plugin===
        Log.Information($"===A cool log message from {PluginInterface.Manifest.Name}===");
        Log.Debug($"Finger Image path: {fingerImagePath}");
        Log.Debug($"Finger Image path: {goatImagePath}");
    }

    public void Dispose()
    {
        // Unregister all actions to not leak anything during disposal of plugin
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;
        
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        PluginInterface.UiBuilder.Draw -= DrawUI;
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // In response to the slash command, toggle the display status of our main ui
        MainWindow.Toggle();
    }

    private void TestCommand(string command, string args)
    {
        TestWindow.Toggle();
    }

    private void DrawUI()
    {
        var player = ClientState.LocalPlayer;
        
        bool hasRG = player.StatusList.Any(s => s.StatusId == 1833);
        if (iconTexture != null && iconTexture.TryGetWrap(out var wrap1, out _ ))
        {
            var pos = new Vector2(1000, 650);
            foreach (var status in player.StatusList)
            {
                var statusData = status.GameData.Value;
                iconTexture = TextureProvider.GetFromGameIcon(new GameIconLookup(statusData.Icon));
                iconTexture.TryGetWrap(out var wrap, out _);
                var drawList = ImGui.GetBackgroundDrawList();
                
                pos.X += 50;
                var size = new Vector2(64, 64);
                
                drawList.AddImage(wrap.Handle, pos, pos + size);
            }
            {
                
            }
            
            
            
        }
    }

    private void OnFrameworkTick(IFramework framework)
    {
        var player = ClientState.LocalPlayer;
        var playerThing = player.StatusList;
      //  var anotherPlayerThing = player.TargetObject.GameObjectId;

        if (player == null)
        {
            return;
        }
        else
        {
            
            //this 
            foreach (var status in  player.StatusList)
            {
                Log.Debug($"Status list:{status.StatusId}");
                var statusData = status.GameData.Value;
                Log.Debug($"Status Name: {statusData.Name}");
                Log.Debug($"Status Icon: {statusData.Icon}");
                
                
                    
                
                
            }

            //if (anotherPlayerThing != null)
            {
                //Log.Debug($"TARGET GAME OBJECT ID:{anotherPlayerThing}");
            }
            
        }

        

    }

 
    
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
    public void ToggleMainUi() => MainWindow.Toggle();
}
