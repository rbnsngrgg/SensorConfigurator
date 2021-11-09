using Moq;
using NUnit.Framework;
using SensorConfigurator.Objects;
using System;
using System.Xml;

namespace SensorConfiguratorTests.ObjectTests
{
    [TestFixture]
    public class ConfiguratorPathsTests
    {

        private readonly string testSN = "139123";
        private readonly string testPN = "920-0201-RGSS";
        private readonly string testRev = "B";
        private readonly string rectDataPath = @"\\castor\Ftproot\RectData\";
        private readonly string rectStandConfigPath = @"\\castor\Ftproot\RectData\Configuration Files\CMM1 Config\";
        private readonly Mock<SCConfig> config = new();
        private readonly Mock<DirectoryWrapper> directoryMock = new();
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _ = config.SetupGet(x => x.RectStandConfigPath).Returns(rectStandConfigPath);
            _ = config.SetupGet(x => x.RectDataPath).Returns(rectDataPath);

            Configurator.InitializeConfigurator(config.Object);
        }

        [SetUp]
        public void SetUp()
        {
            directoryMock.Reset();
            _ = directoryMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            ConfiguratorPaths.Init(testSN, testPN, testRev, directoryMock.Object);
        }
        
        [Test]
        public void TestSensorConfigFolder()
        {
            Assert.AreEqual(@"\\castor\Ftproot\RectData\Configuration Files\CMM1 Config\Sensors\920-0201-RGSS", ConfiguratorPaths.SensorConfigFolder);
        }
        [Test]
        public void TestInvalidSensorConfigFolder()
        {
            _ = directoryMock.Setup(x => x.Exists(It.Is<string>(s => s == @"\\castor\Ftproot\RectData\Configuration Files\CMM1 Config\Sensors\920-0201-RGSS"))).Returns(false);
            _ = Assert.Throws<Exception>(() => ConfiguratorPaths.Init(testSN, testPN, testRev, directoryMock.Object));
        }

        [Test]
        public void TestRectDataFolder()
        {
            Assert.AreEqual(@"\\castor\Ftproot\RectData\SN139XXX\SN139123", ConfiguratorPaths.RectDataPath);
        }
        [Test]
        public void TestInvalidGroupFolder()
        {
            _ = directoryMock.Setup(x => x.Exists(It.Is<string>(s => s == @"\\castor\Ftproot\RectData\SN139XXX"))).Returns(false);
            _ = Assert.Throws<Exception>(() => ConfiguratorPaths.Init(testSN, testPN, testRev, directoryMock.Object));
        }

        [Test]
        public void TestReset()
        {
            Assert.AreEqual(@"\\castor\Ftproot\RectData\Configuration Files\CMM1 Config\Sensors\920-0201-RGSS", ConfiguratorPaths.SensorConfigFolder);

            ConfiguratorPaths.Reset();

            Assert.AreEqual("", ConfiguratorPaths.SensorConfigFolder);
        }

        [Test]
        public void TestValidPartNumber()
        {
            Assert.DoesNotThrow(() => ConfiguratorPaths.Init(testSN, testPN, testRev, directoryMock.Object));
        }

        [Test]
        public void TestInvalidPartNumber()
        {
            _ = Assert.Throws<ArgumentException>(() => ConfiguratorPaths.Init(testSN, "920-0201S-RGSS", testRev, directoryMock.Object));
            _ = Assert.Throws<ArgumentException>(() => ConfiguratorPaths.Init(testSN, "920-0201-RGSSRR", testRev, directoryMock.Object));
            _ = Assert.Throws<ArgumentException>(() => ConfiguratorPaths.Init(testSN, "920-0201RGSS", testRev, directoryMock.Object));
            _ = Assert.Throws<ArgumentException>(() => ConfiguratorPaths.Init(testSN, "92-0201S-RGSS", testRev, directoryMock.Object));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Configurator.Reset();
        }
    }
}
