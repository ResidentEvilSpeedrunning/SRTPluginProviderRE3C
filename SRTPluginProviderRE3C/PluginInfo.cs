using SRTPluginBase;
using System;

namespace SRTPluginProviderRE3C
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Resident Evil 3 Classic (1998))";

        public string Description => "A game memory provider plugin for Resident Evil 3 Classic (1998).";

        public string Author => "VideoGameRoulette";

        public Uri MoreInfoURL => new Uri("https://github.com/ResidentEvilSpeedrunning/SRTPluginProviderRE3C");

        public int VersionMajor => assemblyVersion.Major;

        public int VersionMinor => assemblyVersion.Minor;

        public int VersionBuild => assemblyVersion.Build;

        public int VersionRevision => assemblyVersion.Revision;

        private Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
    }
}
