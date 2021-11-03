using Moq;
using NUnit.Framework;
using SensorConfigurator.Objects;
using System;
using System.Xml;

namespace SensorConfiguratorTests.ObjectTests
{
    [TestFixture]
    public class SCConfigTests
    {
        private SCConfig config;
        private readonly Mock<FileWrapper> fileWrapperMock = new();
        private readonly Mock<DirectoryWrapper> directoryWrapperMock = new();
        private readonly string newConfigExpectedText =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><Configurator RectStandConfigPath=\"\\\\castor\\ftproot\\RectData\\Configuration Files\\CMM1 Config\\\" RectImagesPath=\"H:\\RectImages\\\" />";

        [OneTimeSetUp]
        public void MockReturnSetup()
        {
            _ = directoryWrapperMock.Setup(x => x.GetCurrentDirectory()).Returns(@"C:\TestDirectory\");
            _ = fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            _ = fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(newConfigExpectedText);
        }

        [SetUp]
        public void Setup()
        {
            config = new(fileWrapperMock.Object, directoryWrapperMock.Object);
        }

        [Test]
        public void TestInitWhenConfigNotPresent()
        {
            //Case where config file does not exist and must be created.
            _ = fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            config.Init();

            fileWrapperMock.Verify(x => x.WriteAllText(It.Is<string>(it => it == @"C:\TestDirectory\SCConfig.xml"), newConfigExpectedText));
            directoryWrapperMock.Verify(x => x.GetCurrentDirectory(), Times.AtLeastOnce);
            Assert.AreEqual(@"\\castor\ftproot\RectData\Configuration Files\CMM1 Config\", config.RectStandConfigPath);
            Assert.AreEqual(@"H:\RectImages\", config.RectImagesPath);
        }

        [Test]
        public void TestInitWhenConfigPresent()
        {
            config.Init();

            Assert.AreEqual(@"\\castor\ftproot\RectData\Configuration Files\CMM1 Config\", config.RectStandConfigPath);
            Assert.AreEqual(@"H:\RectImages\", config.RectImagesPath);
        }

        [Test]
        public void TestInvalidConfig()
        {
            //Empty file
            _ = fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns("");
            _ = Assert.Throws<XmlException>(() => config.Init());

            //Missing attribute
            _ = fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(
                "<Configurator RectStandConfigPath=\"\\\\castor\\ftproot\\RectData\\Configuration Files\\CMM1 Config\\\"/>"
            );
            _ = Assert.Throws<ArgumentException>(() => config.Init());
        }
    }
}