using Moq;
using NUnit.Framework;
using SensorConfigurator.Objects;
using System;
using System.Xml;

namespace SensorConfiguratorTests.ObjectTests
{
    [TestFixture]
    public class ConfiguratorTests
    {
        private readonly Mock<SCConfig> configMock = new();
        public void MockReturnSetup()
        {
            _ = configMock.Setup(x => x.Init());
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestInitialization()
        {
            Assert.IsNull(Configurator.Config);
            Configurator.InitializeConfigurator(configMock.Object);
            Assert.IsInstanceOf<SCConfig>(Configurator.Config);
        }
    }
}
