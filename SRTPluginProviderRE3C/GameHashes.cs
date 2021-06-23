using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderRE3C
{
    public static class GameHashes
    {
        private static readonly byte[] BIOHAZARD_3_PC_Rebirth_20181225_1 = new byte[32] { 0xEA, 0xCF, 0x04, 0x45, 0x88, 0x82, 0xB5, 0xBA, 0x24, 0xD6, 0xC8, 0xF2, 0xD4, 0x8D, 0xC6, 0x57, 0xF5, 0x52, 0x10, 0xBF, 0xC8, 0xF7, 0x46, 0x6F, 0xD0, 0x5D, 0xD5, 0x1B, 0x06, 0xD0, 0x51, 0xA1 };

        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(BIOHAZARD_3_PC_Rebirth_20181225_1))
            {
                Console.WriteLine("RE3 Rebirth Detected");
                return GameVersion.Rebirth;
            }
            else
                return GameVersion.Unknown;
        }
    }
}
