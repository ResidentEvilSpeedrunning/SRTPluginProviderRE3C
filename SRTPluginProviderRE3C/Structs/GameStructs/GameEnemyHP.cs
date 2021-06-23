using System.Runtime.InteropServices;

namespace SRTPluginProviderRE3C.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]

    public unsafe struct GameEnemyHP
    {
        [FieldOffset(0x0)] public ushort Current;
        [FieldOffset(0x2)] public ushort Max;

        public static GameEnemyHP AsStruct(byte[] data)
        {
            fixed (byte* pb = &data[0])
            {
                return *(GameEnemyHP*)pb;
            }
        }
    }
}