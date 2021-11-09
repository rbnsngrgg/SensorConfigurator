using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

        public AppLogger()
        {
            fileWrapper = new FileWrapper();
            directoryWrapper = new DirectoryWrapper();
            Init();
        }

        public AppLogger(IFileWrapper fileWrapper, IDirectoryWrapper directoryWrapper)
        {
            this.fileWrapper = fileWrapper;
            this.directoryWrapper = directoryWrapper;
            Init();
        }

        public void Write(string message)
        {
            fileWrapper.AppendAllLines(FilePath, new string[] { $"{Timestamp()}\t{message}" });
        }

        private void Init() => FilePath = Path.Combine(directoryWrapper.GetCurrentDirectory(), FileName);

        private static string Timestamp() => DateTime.UtcNow.ToString("[yyyy-MM-dd HH:mm:ss:ffff]", CultureInfo.InvariantCulture);
    }
}
