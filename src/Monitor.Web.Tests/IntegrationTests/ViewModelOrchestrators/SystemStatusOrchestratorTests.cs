using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.ViewModelOrchestrators;
using SignalKo.SystemMonitor.Monitor.Web.ViewModels;

namespace Monitor.Web.Tests.IntegrationTests.ViewModelOrchestrators
{
    [TestFixture]
    public class SystemStatusOrchestratorTests
    {
        private ISystemStatusOrchestrator systemStatusOrchestrator;

        #region Test Setup

        [TestFixtureSetUp]
        public void Setup()
        {
            IProcessorStatusOrchestrator processorStatusOrchestrator = new ProcessorStatusOrchestrator();
            IMemoryStatusOrchestrator memoryStatusOrchestrator = new MemoryStatusOrchestrator();
            IStorageStatusOrchestrator storageStatusOrchestrator = new StorageStatusOrchestrator();

            this.systemStatusOrchestrator = new SystemStatusOrchestrator(processorStatusOrchestrator, memoryStatusOrchestrator, storageStatusOrchestrator);
        }

        [SetUp]
        public void BeforeEachTest()
        {
        }

        #endregion

        #region GetSystemStatusViewModel

        [Test]
        public void GetSystemStatusViewModel_ParameterIsNotInitialized_ResultIsUninitializedViewModel()
        {
            // Arrage
            var sytemInformation = new SystemInformation();

            // Act
            var result = this.systemStatusOrchestrator.GetSystemStatusViewModel(sytemInformation);

            // Assert
            var emptyViewModel = new SystemStatusViewModel();
            Assert.AreEqual(emptyViewModel.MachineName, result.MachineName);
            Assert.AreEqual(emptyViewModel.Timestamp, result.Timestamp);
            Assert.AreEqual(emptyViewModel.DataPoints, result.DataPoints);
        }

        #endregion
    }
}