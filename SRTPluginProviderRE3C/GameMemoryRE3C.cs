﻿using SRTPluginProviderRE3C.Structs;
using SRTPluginProviderRE3C.Structs.GameStructs;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SRTPluginProviderRE3C
{
    public struct GameMemoryRE3C : IGameMemoryRE3C
    {
        private const string IGT_TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";
        public string GameName => "RE3";
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public uint GameState { get => _gameState; }
        internal uint _gameState;
        public uint Save { get => _save; }
        internal uint _save;
        public uint Total { get => _total; }
        internal uint _total;
        public uint Now { get => _now; }
        internal uint _now;
        public GamePlayer Player { get => _player; }
        internal GamePlayer _player;
        public string PlayerName { get => _playerName; }
        internal string _playerName;
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
