using SRTPluginProviderRE3C.Structs;
using SRTPluginProviderRE3C.Structs.GameStructs;
using System;
using System.Globalization;

namespace SRTPluginProviderRE3C
{
    public struct GameMemoryRE3C : IGameMemoryRE3C
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";
        public uint GameState { get => _gameState; }
        internal uint _gameState;
        public uint Save { get => _save; }
        internal uint _save;
        public uint Total { get => _total; }
        internal uint _total;
        public uint Now { get => _now; }
        internal uint _now;
        public ushort PlayerCurrentHealth { get => _playerCurrentHealth; }
        internal ushort _playerCurrentHealth;
        public ushort PlayerMaxHealth { get => _playerMaxHealth; }
        internal ushort _playerMaxHealth;
        public byte PlayerStatus { get => _playerStatus; }
        internal byte _playerStatus;
        public byte EquippedItemId { get => _equippedItemId; }
        internal byte _equippedItemId;
        public byte AvailableSlots { get => _availableSlots; }
        internal byte _availableSlots;
        public byte PlayerCharacter { get => _playerCharacter; }
        internal byte _playerCharacter;
        public GameItemEntry[] PlayerInventory { get => _playerInventory; }
        internal GameItemEntry[] _playerInventory;
        public EnemyHP Nemesis { get => _nemesis; }
        internal EnemyHP _nemesis;

        public TimeSpan IGTTimeSpan => (GameState & 0x4000) == 0x4000 ? TimeSpan.FromSeconds(Save / 60) : TimeSpan.FromSeconds((Save - Total + Now) / 60);
        public string IGTFormattedString => IGTTimeSpan.ToString(IGT_TIMESPAN_STRING_FORMAT, CultureInfo.InvariantCulture);
    }
}
