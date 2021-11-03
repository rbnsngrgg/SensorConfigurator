using System;
using System.IO;
using System.Xml;

namespace SensorConfigurator.Objects
{
    public class SCConfig
    {
        public string RectStandConfigPath { get; private set; }
        public string RectImagesPath { get; private set; }
        public string FileName { get; } = "SCConfig.xml";
        public string FilePath { get; private set; }

        private readonly IFileWrapper fileWrapper;
        private readonly IDirectoryWrapper directoryWrapper;

        public SCConfig()
        {
            fileWrapper = new FileWrapper();
            directoryWrapper = new DirectoryWrapper();
        }

        public SCConfig(IFileWrapper fileWrapper, IDirectoryWrapper directoryWrapper)
        {
            this.fileWrapper = fileWrapper;
            this.directoryWrapper = directoryWrapper;
        }

        public virtual void Init()
        {
            FilePath = Path.Combine(directoryWrapper.GetCurrentDirectory(), FileName);
            if (!fileWrapper.Exists(FilePath))
            {
                CreateConfig();
            }
            LoadConfig();
        }

        private void CreateConfig()
        {
            MemoryStream memStream = new();
            XmlWriter writer = XmlWriter.Create(memStream);

            writer.WriteStartDocument();
            writer.WriteStartElement("Configurator");

            writer.WriteAttributeString("RectStandConfigPath", "\\\\castor\\ftproot\\RectData\\Configuration Files\\CMM1 Config\\");
            writer.WriteAttributeString("RectImagesPath", "H:\\RectImages\\");

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            memStream.Position = 0;
            using StreamReader streamReader = new(memStream);
            string xml = streamReader.ReadToEnd();
            fileWrapper.WriteAllText(FilePath, xml);
        }

        private void LoadConfig()
        {
            XmlDocument config = new();
            config.LoadXml(string.Join(' ', fileWrapper.ReadAllText(FilePath)));
            XmlNodeList elements = config.GetElementsByTagName("Configurator");
            if (elements != null)
            {
                _ = elements[0].Attributes.GetNamedItem("RectImagesPath") ?? throw new ArgumentException();
                RectImagesPath = elements[0].Attributes.GetNamedItem("RectImagesPath").Value;

                _ = elements[0].Attributes.GetNamedItem("RectStandConfigPath") ?? throw new ArgumentException();
                RectStandConfigPath = elements[0].Attributes.GetNamedItem("RectStandConfigPath").Value ?? throw new ArgumentException();
            }
        }
    }
}
