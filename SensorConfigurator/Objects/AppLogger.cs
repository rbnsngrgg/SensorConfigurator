using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorConfigurator.Objects
{
    public class AppLogger
    {
        public string FileName { get; } = "Configurator.log";
        public string FilePath { get; private set; }

        private readonly IFileWrapper fileWrapper;
        private readonly IDirectoryWrapper directoryWrapper;

    }
}
