using SRTPluginProviderRE3C.Structs;
using SRTPluginProviderRE3C.Structs.GameStructs;
using System;

namespace SRTPluginProviderRE3C
{
    public interface IGameMemoryRE3C
    {
        uint GameState { get; }
        uint Save { get; }
        uint Total { get; }
        uint Now { get; }
        ushort PlayerCurrentHealth { get; }
        ushort PlayerMaxHealth { get; }
        byte PlayerStatus { get; }
        byte EquippedItemId { get; }
        byte AvailableSlots { get; }
        byte PlayerCharacter { get; }
        GameItemEntry[] PlayerInventory { get; }
        EnemyHP Nemesis { get; }
        TimeSpan IGTTimeSpan { get; }
        string IGTFormattedString { get; }
    }
}
