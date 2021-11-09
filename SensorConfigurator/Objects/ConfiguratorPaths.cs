using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SensorConfigurator.Objects
{
    internal static class ConfiguratorPaths
    {
        public static string SensorConfigFolder { get; private set; }
        public static string RectDataPath { get; private set; }

        private static string serialNumber = "";
        private static string partNumber = "";
        private static string sensorRev = "";
        private static IDirectoryWrapper directoryWrapper;

        public static void Init(string serialNumber, string partNumber, string sensorRev, IDirectoryWrapper directoryWrapper = null)
        {
            ConfiguratorPaths.directoryWrapper = directoryWrapper ?? new DirectoryWrapper();
            ConfiguratorPaths.serialNumber = serialNumber;
            ConfiguratorPaths.partNumber = partNumber;
            ConfiguratorPaths.sensorRev = sensorRev;

            SetSensorConfigFolder();
            SetRectDataPath();
            CheckValidPartNumber();

        }

        public static void Reset()
        {
            SensorConfigFolder = "";
            RectDataPath = "";
        }

        private static void SetSensorConfigFolder()
        {
            string folder = Path.Combine(
                Path.Combine($"{Configurator.Config.RectStandConfigPath}", "Sensors"),
                partNumber);
            SensorConfigFolder = directoryWrapper.Exists(folder)
                ? folder
                : throw new Exception($"SetSensorConfigFolder: " +
                $"There is no config folder for the specified part number at {folder}");
        }

        private static void SetRectDataPath() => RectDataPath = Path.Combine(GetGroupFolder(), $"SN{serialNumber}");

        private static void CheckValidPartNumber()
        {
            if (!Regex.IsMatch(partNumber, @"^\d{3}-\d{4}-\w{4}\w?$"))
            {
                throw new ArgumentException("CheckValidPartNumber: The provided part number is invalid.");
            }
        }
        private static string GetGroupFolder()
        {
            string folder = "";
            if (serialNumber.Length == 6)
            {
                string group = $"SN{serialNumber.Remove(3)}XXX";
                string newFolder = Path.Combine(Configurator.Config.RectDataPath, group);
                folder = directoryWrapper.Exists(newFolder)
                    ? newFolder
                    : throw new Exception($"GetGroupFolder: No serial number group folder exists for {serialNumber} at {newFolder}");
            }
            return folder;
        }
    }
}
