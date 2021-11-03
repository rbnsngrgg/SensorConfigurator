using System;
using System.IO;
using System.Xml;

namespace SensorConfigurator.Objects
{
    public class SCConfig
    {
        public string RectStandConfigPath { get; private set; }
        public string RectImagesPath { get; private set; }

        private readonly IFileWrapper fileWrapper;
        private readonly IDirectoryWrapper directoryWrapper;
        private readonly string fileName = "SCConfig.xml";
        private string filePath;

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
            filePath = Path.Combine(this.directoryWrapper.GetCurrentDirectory(), fileName);
            if (!fileWrapper.Exists(filePath))
            {
                CreateConfig();
            }
            LoadConfig();
        }

        private void CreateConfig()
        {
            MemoryStream configStream = new();
            XmlWriter configWriter = XmlWriter.Create(configStream);

            configWriter.WriteStartDocument();
            configWriter.WriteStartElement("Configurator");

            configWriter.WriteAttributeString("RectStandConfigPath", "\\\\castor\\ftproot\\RectData\\Configuration Files\\CMM1 Config\\");
            configWriter.WriteAttributeString("RectImagesPath", "H:\\RectImages\\");

            configWriter.WriteEndElement();
            configWriter.WriteEndDocument();
            configWriter.Close();

            configStream.Position = 0;
            using StreamReader streamReader = new(configStream);
            string xml = streamReader.ReadToEnd();
            fileWrapper.WriteAllText(filePath, xml);
        }

        private void LoadConfig()
        {
            XmlDocument config = new();
            config.LoadXml(string.Join(' ', fileWrapper.ReadAllText(filePath)));
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
