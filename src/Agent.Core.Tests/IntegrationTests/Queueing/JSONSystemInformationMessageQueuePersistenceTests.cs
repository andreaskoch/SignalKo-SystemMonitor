using System;
using System.IO;
using System.Linq;

using Moq;

using Newtonsoft.Json;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Exceptions;
using SignalKo.SystemMonitor.Agent.Core.Queueing;
using SignalKo.SystemMonitor.Common.Services;

namespace Agent.Core.Tests.IntegrationTests.Queueing
{
    [TestFixture]
    public class JSONSystemInformationMessageQueuePersistenceTests
    {
        #region Test Setup
        
        private const string TestConfigFileExtension = ".test.config";

        private IEncodingProvider encodingProvider;

        [TestFixtureSetUp]
        public void Setup()
        {
            this.encodingProvider = new DefaultEncodingProvider();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            var configFiles = Directory.GetFiles(Environment.CurrentDirectory, "*" + TestConfigFileExtension, SearchOption.TopDirectoryOnly);
            foreach (var configFile in configFiles)
            {
                File.Delete(configFile);
            }
        }

        #endregion

        #region Load

        [Test]
        public void Load_StorageFilePathDoesNotExist_ResultIsNull()
        {
            // Arrange
            string configfurationFileName = "Non-Existing-Config-File" + TestConfigFileExtension;

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            var result = queuePersistence.Load();

            // Assert
            Assert.IsNull(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("   ")]
        public void Load_StorageFilePathContainsNoContent_ResultIsNull(string fileContent)
        {
            // Arrange
            string configfurationFileName = "File-With-No-Content" + TestConfigFileExtension;
            File.WriteAllText(configfurationFileName, fileContent, this.encodingProvider.GetEncoding());

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            var result = queuePersistence.Load();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Load_StorageFilePathContainsInvalidContent_ResultIsNull()
        {
            // Arrange
            string configfurationFileName = "File-With-Invalid-Content" + TestConfigFileExtension;
            File.WriteAllText(
                configfurationFileName,
                "Garble Garble. Yada Yada. I will cause an exception when you try to deserialize me.",
                this.encodingProvider.GetEncoding());

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            var result = queuePersistence.Load();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Load_StorageFilePathContainsEmptyArray_ResultIsEmpty()
        {
            // Arrange
            string configfurationFileName = "File-With-EmptyArray" + TestConfigFileExtension;
            File.WriteAllText(
                configfurationFileName,
                "[]",
                this.encodingProvider.GetEncoding());

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            var result = queuePersistence.Load();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void Load_StorageFilePathContainsValidContent_ResultContainsAllItems()
        {
            // Arrange
            string configfurationFileName = "File-With-Valid-Content" + TestConfigFileExtension;
            var itemArray = TestUtilities.GetSystemInformationObjects(5).Select(systemInformation => new SystemInformationQueueItem(systemInformation)).ToArray();

            File.WriteAllText(
                configfurationFileName,
                JsonConvert.SerializeObject(itemArray),
                this.encodingProvider.GetEncoding());

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            var result = queuePersistence.Load();

            // Assert
            Assert.AreEqual(itemArray, result);
        }

        #endregion

        #region Save

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_ParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            string configfurationFileName = "A-New-Config-File" + TestConfigFileExtension;

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            queuePersistence.Save(null);
        }

        [Test]
        [ExpectedException(typeof(MessageQueuePersistenceException))]
        public void Save_TargetFileIsLockedByAnotherProcess_MessageQueuePersistenceExceptionIsThrown()
        {
            // Arrange
            string configfurationFileName = "Locked-File" + TestConfigFileExtension;

            var itemArray = TestUtilities.GetSystemInformationObjects(5).Select(systemInformation => new SystemInformationQueueItem(systemInformation)).ToArray();

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            using (var fileStream = File.OpenWrite(configfurationFileName))
            {
                queuePersistence.Save(itemArray);
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [ExpectedException(typeof(MessageQueuePersistenceException))]
        public void Save_TargetFileNameIsInvalid_MessageQueuePersistenceExceptionIsThrown(string targetFileName)
        {
            // Arrange
            var itemArray = TestUtilities.GetSystemInformationObjects(5).Select(systemInformation => new SystemInformationQueueItem(systemInformation)).ToArray();

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = targetFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            queuePersistence.Save(itemArray);
        }

        [Test]
        public void Save_TargetFileIsValid_DoesNotYetExist_TargetfileIsCreated()
        {
            // Arrange
            string configfurationFileName = "New-File+" + Guid.NewGuid() + TestConfigFileExtension;
            var itemArray = TestUtilities.GetSystemInformationObjects(5).Select(systemInformation => new SystemInformationQueueItem(systemInformation)).ToArray();

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            queuePersistence.Save(itemArray);

            // Assert
            File.Exists(configfurationFileName);
        }

        [Test]
        public void Save_TargetFileIsValid_DoesNotYetExist_TargetfileIsIsNotEmptyAfterSave()
        {
            // Arrange
            string configfurationFileName = "New-File+" + Guid.NewGuid() + TestConfigFileExtension;
            var itemArray = TestUtilities.GetSystemInformationObjects(5).Select(systemInformation => new SystemInformationQueueItem(systemInformation)).ToArray();

            var configuration = new JSONMessageQueuePersistenceConfiguration { FilePath = configfurationFileName };
            var configurationProvider = new Mock<IJSONMessageQueuePersistenceConfigurationProvider>();
            configurationProvider.Setup(c => c.GetConfiguration()).Returns(configuration);

            var queuePersistence = new JSONSystemInformationMessageQueuePersistence(configurationProvider.Object, this.encodingProvider);

            // Act
            queuePersistence.Save(itemArray);

            // Assert
            Assert.IsNotNullOrEmpty(File.ReadAllText(configfurationFileName));
        }

        #endregion
    }
}