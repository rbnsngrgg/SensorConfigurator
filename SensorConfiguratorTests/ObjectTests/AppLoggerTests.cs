using Moq;
using NUnit.Framework;
using SensorConfigurator.Objects;
using System;
using System.Xml;

namespace SensorConfiguratorTests.ObjectTests
{
    [TestFixture]
    class AppLoggerTests
    {
        private AppLogger logger;
        private readonly Mock<FileWrapper> fileWrapperMock = new();
        private readonly Mock<DirectoryWrapper> directoryWrapperMock = new();

        [SetUp]
        public void SetUp()
        {
            logger = new();
        }

        [Test]
        public void TestInitialValues()
        {
            Assert.AreEqual("Configurator.log", logger.FileName);
            Assert.AreEqual("", logger.FilePath);
        }
    }
}
