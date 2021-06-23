using SRTPluginProviderRE3C.Enumerations;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE3C.Structs.GameStructs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameItemEntry
    {
        [FieldOffset(0x0)] private byte itemId;
        [FieldOffset(0x1)] private byte stackSize;
        [FieldOffset(0x2)] private byte itemType;

        public static GameItemEntry AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameItemEntry*)pb;
            }
        }

        // Public Properties
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsItem)
                {
                    return string.Format("ID: {0} | Name: {1} | Quantity: {2}", ItemID, ItemID.ToString(), Quantity);
                }
                return "Empty Slot";
            }
        }

        public ItemEnumeration ItemID { get => (ItemEnumeration)itemId; }
        public string ItemName => ItemID.ToString();
        public byte Quantity { get => stackSize; }
        public ItemTypeEnumeration ItemType { get => (ItemTypeEnumeration)itemType; }
        public string ItemTypeString => ItemType.ToString();
        public bool IsItem => Enum.IsDefined(typeof(ItemEnumeration), itemId);
    }
}