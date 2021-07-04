using ProcessMemory;
using System;
using System.Diagnostics;
using SRTPluginProviderRE3C.Structs;
using SRTPluginProviderRE3C.Structs.GameStructs;

namespace SRTPluginProviderRE3C
{
    public unsafe class GameMemoryRE3CScanner : IDisposable
    {
        //private static readonly int MAX_ENTITIES = 32;
        private static readonly int MAX_ITEMS = 10;
        //private static readonly int MAX_BOX_ITEMS = 8;

        // Variables
        private ProcessMemoryHandler memoryAccess;
        private GameMemoryRE3C gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        public GameItemEntry EmptySlot = new GameItemEntry();

        // Addresses
        private int* AddressPlayerType = (int*)0;
        private int* AddressGameState = (int*)0;
        private int* AddressSave = (int*)0;
        private int* AddressTotal = (int*)0;
        private int* AddressNow = (int*)0;

        private int* AddressPlayer = (int*)0;
        private int* AddressNemesisHP = (int*)0;
        private int* AddressPlayerMaxHP = (int*)0;
        private int* AddressEquippedItemId = (int*)0;
        private int* AddressInventorySlots = (int*)0;
        private int* AddressInventory = (int*)0;

        internal GameMemoryRE3CScanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryRE3C();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            if (!SelectAddresses(GameHashes.DetectVersion(process.MainModule.FileName)))
                return; // Unknown version.

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemoryHandler(pid);
        }

        private bool SelectAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.Rebirth:
                    {
                        AddressPlayerType = (int*)0x7043CF;
                        AddressGameState = (int*)0xA61CA0;
                        AddressSave = (int*)0xA67378;
                        AddressTotal = (int*)0xA61D64;
                        AddressNow = (int*)0xA449D4;
                        AddressPlayer = (int*)0xA620E0;
                        AddressNemesisHP = (int*)0x655A58;
                        AddressEquippedItemId = (int*)0xA676AD;
                        AddressInventorySlots = (int*)0xA676AE;
                        AddressInventory = (int*)0xA67584;
                        return true;
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return false;
        }

        internal unsafe IGameMemoryRE3C Refresh()
        {
            fixed (byte* p = &gameMemoryValues._playerCharacter)
                memoryAccess.TryGetByteAt((IntPtr)AddressPlayerType, p);

            fixed (uint* p = &gameMemoryValues._gameState)
                memoryAccess.TryGetUIntAt((IntPtr)AddressGameState, p);

            fixed (uint* p = &gameMemoryValues._save)
                memoryAccess.TryGetUIntAt((IntPtr)AddressSave, p);

            fixed (uint* p = &gameMemoryValues._total)
                memoryAccess.TryGetUIntAt((IntPtr)AddressTotal, p);

            fixed (uint* p = &gameMemoryValues._now)
                memoryAccess.TryGetUIntAt((IntPtr)AddressNow, p);

            //Player HP
            if (SafeReadByteArray((IntPtr)AddressPlayer, sizeof(GamePlayerHP), out byte[] PlayerBytes))
            {
                var playerStats = GamePlayerHP.AsStruct(PlayerBytes);
                gameMemoryValues._playerMaxHealth = playerStats.Max;
                gameMemoryValues._playerCurrentHealth = playerStats.Current;
                gameMemoryValues._playerStatus = playerStats.Status;
            }

            fixed (byte* p = &gameMemoryValues._equippedItemId)
                memoryAccess.TryGetByteAt((IntPtr)AddressEquippedItemId, p);

            fixed (byte* p = &gameMemoryValues._availableSlots)
                memoryAccess.TryGetByteAt((IntPtr)AddressInventorySlots, p);

            // Inventory
            if (gameMemoryValues._playerInventory == null)
                gameMemoryValues._playerInventory = new GameItemEntry[MAX_ITEMS];

            for (int i = 0; i < MAX_ITEMS; ++i)
            {
                if (i > gameMemoryValues.AvailableSlots)
                    gameMemoryValues._playerInventory[i] = EmptySlot;

                if (SafeReadByteArray(IntPtr.Add((IntPtr)AddressInventory, (i * 0x4)), sizeof(GameItemEntry), out byte[] ItemBytes))
                    gameMemoryValues._playerInventory[i] = GameItemEntry.AsStruct(ItemBytes);
            }
                

            //Enemy HP
            if (SafeReadByteArray((IntPtr)AddressNemesisHP, sizeof(GameEnemyHP), out byte[] EnemyBytes))
            {
                var enemyHP = GameEnemyHP.AsStruct(EnemyBytes);
                gameMemoryValues._nemesis._maximumHP = enemyHP.Max;
                gameMemoryValues._nemesis._currentHP = enemyHP.Current;
            }

            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private unsafe bool SafeReadByteArray(IntPtr address, int size, out byte[] readBytes)
        {
            readBytes = new byte[size];
            fixed (byte* p = readBytes)
            {
                return memoryAccess.TryGetByteArrayAt(address, size, p);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
