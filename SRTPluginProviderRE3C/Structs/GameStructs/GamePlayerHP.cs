using System.Runtime.InteropServices;

namespace SRTPluginProviderRE3C.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GamePlayerHP
    {
        [FieldOffset(0x0)] public ushort Current;
        [FieldOffset(0x2)] public ushort Max;
        [FieldOffset(0x7)] public byte Status;

        public static GamePlayerHP AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GamePlayerHP*)pb;
            }
        }
    }
}