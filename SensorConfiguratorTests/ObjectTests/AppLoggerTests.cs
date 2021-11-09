using Moq;
using NUnit.Framework;
using SensorConfigurator.Objects;
using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Xml;

namespace SensorConfiguratorTests.ObjectTests
{
    [TestFixture]
    class AppLoggerTests
    {
        private AppLogger logger;
        private readonly Mock<FileWrapper> fileWrapperMock = new();
        private readonly Mock<DirectoryWrapper> directoryWrapperMock = new();
        private readonly string testDirectory = @"C:\TestFolder\";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _ = directoryWrapperMock.Setup(x => x.GetCurrentDirectory()).Returns(testDirectory);
        }

        [SetUp]
        public void SetUp()
        {
            logger = new(fileWrapperMock.Object, directoryWrapperMock.Object);
        }

        [Test]
        public void TestInitialValues()
        {
            Assert.AreEqual("Configurator.log", logger.FileName);
            Assert.AreEqual(Path.Combine(testDirectory, logger.FileName), logger.FilePath);
        }

        [Test]
        public void TestWriteLog()
        {
            string testMessage = "Test Message 1";
            logger.Write(testMessage);

            fileWrapperMock.Verify
                (x => x.AppendAllLines(
                    It.Is<string>(path => path == Path.Combine(testDirectory, logger.FileName)),
                    It.Is<string[]>(x => VerifyWriteMessage(x, testMessage))),
                    Times.Once);
        }
        private static bool VerifyWriteMessage(string[] array, string message)
        {
            string[] messageSplit = array[0].Split('\t');
            return array.Length == 1
                && DateTime.TryParseExact(messageSplit[0], "[yyyy-MM-dd HH:mm:ss:ffff]", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                && messageSplit[1] == message;
        }
    }
}
