using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorConfigurator.Objects
{
    internal static class Configurator
    {
        public static SCConfig Config { get; private set; }

        public static void InitializeConfigurator(SCConfig config = null)
        {
            Config = config ?? new();
            Config.Init();
        }
    }
}
