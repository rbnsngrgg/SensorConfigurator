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
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><Configurator " +
            "RectDataPath=\"\\\\castor\\Ftproot\\RectData\\\" " +
            "RectImagesPath=\"H:\\RectImages\\\" " +
            "RectStandConfigPath=\"\\\\castor\\ftproot\\RectData\\Configuration Files\\CMM1 Config\\\" />";

        [SetUp]
        public void Setup()
        {
            _ = directoryWrapperMock.Setup(x => x.GetCurrentDirectory()).Returns(@"C:\TestDirectory\");
            _ = fileWrapperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(newConfigExpectedText);

            config = new(fileWrapperMock.Object, directoryWrapperMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            fileWrapperMock.Reset();
            directoryWrapperMock.Reset();
        }

        [Test]
        public void TestInitWhenConfigNotPresent()
        {
            //Case where config file does not exist and must be created.
            _ = fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            config.Init();

            fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            fileWrapperMock.Verify(x => x.WriteAllText(It.Is<string>(it => it == @"C:\TestDirectory\SCConfig.xml"), newConfigExpectedText));
            directoryWrapperMock.Verify(x => x.GetCurrentDirectory(), Times.Once);
            AssertConfigProps();
        }

        [Test]
        public void TestInitWhenConfigPresent()
        {
            _ = fileWrapperMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            config.Init();
            fileWrapperMock.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            AssertConfigProps();
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

        private void AssertConfigProps()
        {
            Assert.AreEqual(@"\\castor\ftproot\RectData\Configuration Files\CMM1 Config\", config.RectStandConfigPath);
            Assert.AreEqual(@"H:\RectImages\", config.RectImagesPath);
            Assert.AreEqual(@"\\castor\Ftproot\RectData\", config.RectDataPath);
        }
    }
}