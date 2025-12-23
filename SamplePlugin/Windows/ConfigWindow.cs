using System;
using System.Configuration;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Serilog;

namespace StrongVibes.Windows;

public class ConfigWindow : Window, IDisposable
{
    [PluginService]internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    private Configuration configuration { get; init;}
    
    
    // We give this window a constant ID using ###.
    // This allows for labels to be dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("A Wonderful Configuration Window###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(500, 500);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
        
        
}

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (configuration.IsConfigWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
        
    }

    public override void Draw()
    {
        // Can't ref a property, so use a local copy
        var configValue = configuration.SomePropertyToBeSavedAndWithADefault;
        
        if (ImGui.Checkbox("Random Config Bool", ref configValue))
        {
            configuration.SomePropertyToBeSavedAndWithADefault = configValue;
            // Can save immediately on change if you don't want to provide a "Save and Close" button
            configuration.Save();
        }

        var movable = configuration.IsConfigWindowMovable;
        if (ImGui.Checkbox("Movable Config Window", ref movable))
        {
            configuration.IsConfigWindowMovable = movable;
            configuration.Save();
        }
        int Xpos = configuration.XPos;
        if (ImGui.InputInt("Enter X position", ref Xpos))
        {
            configuration.XPos = Xpos;
            configuration.Save();
        }
        int Ypos = configuration.YPos;
        if (ImGui.InputInt("Enter Y position", ref Ypos))
        {
            configuration.YPos = Ypos;
            configuration.Save(); 
        }

        int scaleX = configuration.scaleX;
        if (ImGui.InputInt("Scale X", ref scaleX))
        {
            configuration.scaleX = scaleX;
            configuration.Save();
        }
        int scaleY = configuration.scaleY;
        if (ImGui.InputInt("Scale Y", ref scaleY))
        {
            configuration.scaleY = scaleY;
            configuration.Save();
        }

        float red = configuration.textRed;
        if (ImGui.InputFloat("red value", ref red))
        {
            configuration.textRed = red;
            configuration.Save();
        }
        float green = configuration.textGreen;
        if (ImGui.InputFloat("green value", ref green))
        {
            configuration.textGreen = green;
            configuration.Save();
        }
        float blue = configuration.textBlue;
        if (ImGui.InputFloat("blue value", ref blue))
        {
            configuration.textBlue = blue;
            configuration.Save();
        }
        float alpha = configuration.textAlpha;
        if (ImGui.InputFloat("red value", ref alpha))
        {
            configuration.textAlpha = alpha;
            configuration.Save();
        }
        
        
        ImGui.Text($"Current X Value is: {Xpos}");
        ImGui.Text($"Current Y Value is: {Ypos}");
        ImGui.Text($"Scale X Value is: {scaleX}");
        ImGui.Text($"Scale Y Value is: {scaleY}");
        ImGui.Text($"Alpha Value is: {alpha}");
        ImGui.Text($"Red Value is: {red}");
        ImGui.Text($"Green Value is: {green}");
        ImGui.Text($"Blue Value is: {blue}");
    }
}
