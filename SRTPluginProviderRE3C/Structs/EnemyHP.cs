using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SRTPluginProviderRE3C.Structs
{
    public class Boss
    {
        //public static float[] HitPoints = { 2900, 3400, 3700, 9000, 25000, 26000, 30000, 75000, 100000 };
        public static Dictionary<float, string> Bosses = new Dictionary<float, string>()
        {
            { 800, "Nemesis" },
        };
    }

    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct EnemyHP
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsAlive)
                {
                    return string.Format("{0} / {1} ({2:P1})", CurrentHP, MaximumHP, Percentage);
                }
                return "DEAD / DEAD (0%)";
            }
        }
        
        public bool IsBoss => true;
        public string BossName => "Nemesis";
        public ushort MaximumHP { get => _maximumHP; }
        internal ushort _maximumHP;
        public ushort CurrentHP { get => _currentHP; }
        internal ushort _currentHP;
        public bool IsDead => CurrentHP > MaximumHP || CurrentHP == 0xFFFF;
        public bool IsAlive => !IsDead && CurrentHP <= MaximumHP && CurrentHP != 0;
        public float Percentage => IsAlive ? (float)(CurrentHP / MaximumHP) : 0f;
    }
}
