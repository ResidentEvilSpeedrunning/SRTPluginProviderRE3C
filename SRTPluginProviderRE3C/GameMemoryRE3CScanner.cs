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

        private IntPtr BaseAddress { get; set; }

        // Addresses
        private int AddressPlayerType;
        private int AddressGameState;
        private int AddressSave;
        private int AddressTotal;
        private int AddressNow;
        private int AddressPlayer;
        private int AddressNemesisHP;
        private int AddressEquippedItemId;
        private int AddressInventorySlots;
        private int AddressInventory;

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
            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_32BIT); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.
            }
        }

        private bool SelectAddresses(GameVersion version)
        {
            switch (version)
            {
                case GameVersion.Rebirth:
                    {
                        AddressPlayerType = 0x3043CF;
                        AddressGameState = 0x661CA0;
                        AddressSave = 0x667378;
                        AddressTotal = 0x661D64;
                        AddressNow = 0x6449D4;
                        AddressPlayer = 0x6620E0;
                        AddressNemesisHP = 0x255A58;
                        AddressEquippedItemId = 0x6676AD;
                        AddressInventorySlots = 0x6676AE;
                        AddressInventory = 0x667584;
                        return true;
                    }
            }

            // If we made it this far... rest in pepperonis. We have failed to detect any of the correct versions we support and have no idea what pointer addresses to use. Bail out.
            return false;
        }

        internal unsafe IGameMemoryRE3C Refresh()
        {
            gameMemoryValues._playerCharacter = memoryAccess.GetByteAt(IntPtr.Add(BaseAddress, AddressPlayerType));

            gameMemoryValues._gameState = memoryAccess.GetUIntAt(IntPtr.Add(BaseAddress, AddressGameState));

            gameMemoryValues._save = memoryAccess.GetUIntAt(IntPtr.Add(BaseAddress, AddressSave));

            gameMemoryValues._total = memoryAccess.GetUIntAt(IntPtr.Add(BaseAddress, AddressTotal));

            gameMemoryValues._now = memoryAccess.GetUIntAt(IntPtr.Add(BaseAddress, AddressNow));

            //Player HP
            GamePlayerHP gphp = memoryAccess.GetAt<GamePlayerHP>(IntPtr.Add(BaseAddress, AddressPlayer));
            gameMemoryValues._playerMaxHealth = gphp.Max;
            gameMemoryValues._playerCurrentHealth = gphp.Current;
            gameMemoryValues._playerStatus = gphp.Status;

            gameMemoryValues._equippedItemId = memoryAccess.GetByteAt(IntPtr.Add(BaseAddress, AddressEquippedItemId));

            gameMemoryValues._availableSlots = memoryAccess.GetByteAt(IntPtr.Add(BaseAddress, AddressInventorySlots));

            // Inventory
            if (gameMemoryValues._playerInventory == null)
                gameMemoryValues._playerInventory = new GameItemEntry[MAX_ITEMS];

            for (int i = 0; i < MAX_ITEMS; ++i)
            {
                if (i > gameMemoryValues.AvailableSlots)
                    gameMemoryValues._playerInventory[i] = EmptySlot;

                gameMemoryValues._playerInventory[i] = memoryAccess.GetAt<GameItemEntry>(IntPtr.Add(BaseAddress + AddressInventory, (i * 0x4)));
            }

            //Enemy HP
            GameEnemyHP gehp = memoryAccess.GetAt<GameEnemyHP>(IntPtr.Add(BaseAddress, AddressNemesisHP));
            gameMemoryValues._nemesis._maximumHP = gehp.Max;
            gameMemoryValues._nemesis._currentHP = gehp.Current;

            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
