using SRTPluginProviderRE3C.Structs;
using SRTPluginProviderRE3C.Structs.GameStructs;
using System;

namespace SRTPluginProviderRE3C
{
    public interface IGameMemoryRE3C
    {
        string GameName { get; }
        string VersionInfo { get; }
        uint GameState { get; }
        uint Save { get; }
        uint Total { get; }
        uint Now { get; }
        GamePlayer Player { get; }
        string PlayerName { get; }
        byte EquippedItemId { get; }
        byte AvailableSlots { get; }
        byte PlayerCharacter { get; }
        GameItemEntry[] PlayerInventory { get; }
        EnemyHP Nemesis { get; }
        TimeSpan IGTTimeSpan { get; }
        string IGTFormattedString { get; }
    }
}
