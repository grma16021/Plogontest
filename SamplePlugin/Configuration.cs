using System;
using Dalamud.Configuration;

namespace StrongVibes;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;
    public int XPos { get; set; } = 500;
    public int YPos { get; set; } = 500;
    public int scaleX { get; set; } = 64;
    public int scaleY { get; set; } = 64;
    // The below exists just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
