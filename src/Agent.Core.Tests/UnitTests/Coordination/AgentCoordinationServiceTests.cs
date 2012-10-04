using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Agent.Core.Coordination;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Coordination
{
    [TestFixture]
    public class AgentCoordinationServiceTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            // Act
            var agentCoordinationService = new AgentCoordinationService(agentConfigurationProvider.Object, () => { }, () => { });

            // Assert
            Assert.IsNotNull(agentCoordinationService);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AgentConfigurationProviderParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Act
            new AgentCoordinationService(null, pauseCallback: () => { }, resumeCallback: () => { });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_PauseCallbackParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            // Act
            new AgentCoordinationService(agentConfigurationProvider.Object, pauseCallback: null, resumeCallback: () => { });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ResumeCallbackParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            // Act
            new AgentCoordinationService(agentConfigurationProvider.Object, pauseCallback: () => { }, resumeCallback: null);
        }

        #endregion

        #region Start

        [Test]
        public void Start_GetAgentConfigurationReturnsNull_PauseCallbackIsCalled()
        {
            // Arrange
            bool pauseCallbackWasCalled = false;
            var runDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 1;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            AgentConfiguration agentConfiguration = null;
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(
                agentConfigurationProvider.Object,
                pauseCallback: () =>
                    {
                        pauseCallbackWasCalled = true;
                    },
                resumeCallback: () => { });

            // Act
            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();

            Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

            // Assert
            Assert.IsTrue(pauseCallbackWasCalled);
        }

        [Test]
        public void Start_AgentsAreDisabled_PauseCallbackIsCalled()
        {
            // Arrange
            bool pauseCallbackWasCalled = false;
            var runDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 1;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            var agentConfiguration = new AgentConfiguration()
                {
                    AgentsAreEnabled = false,
                    BaseUrl = "http://www.example.com",
                    CheckIntervalInSeconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds,
                    SystemInformationSenderPath = "/api/systeminformation"
                };
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(
                agentConfigurationProvider.Object,
                pauseCallback: () =>
                {
                    pauseCallbackWasCalled = true;
                },
                resumeCallback: () => { });

            // Act
            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();

            Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

            // Assert
            Assert.IsTrue(pauseCallbackWasCalled);
        }

        [Test]
        public void Start_AgentsAreEnabled_ResumeCallbackIsCalled()
        {
            // Arrange
            bool resumeCallbackWasCalled = false;
            var runDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 1;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            var agentConfiguration = new AgentConfiguration()
            {
                AgentsAreEnabled = true,
                BaseUrl = "http://www.example.com",
                CheckIntervalInSeconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds,
                SystemInformationSenderPath = "/api/systeminformation"
            };
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(
                agentConfigurationProvider.Object,
                pauseCallback: () => { },
                resumeCallback: () =>
                {
                    resumeCallbackWasCalled = true;
                });

            // Act
            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();

            Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

            // Assert
            Assert.IsTrue(resumeCallbackWasCalled);
        }

        [Test]
        public void Start_RunsForThreeIntervals_GetAgentConfigurationIsCalledThreeTimes()
        {
            // Arrange
            var runDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 3;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();
            var agentConfiguration = new AgentConfiguration()
                {
                    AgentsAreEnabled = true,
                    BaseUrl = "http://www.example.com",
                    CheckIntervalInSeconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds,
                    SystemInformationSenderPath = "/api/systeminformation"
                };
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(agentConfigurationProvider.Object, () => { }, () => { });

            // Act
            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();

            Task.WaitAll(new[] { agentCoordinationServiceTask }, runDurationInMilliseconds);

            // Assert
            agentConfigurationProvider.Verify(a => a.GetAgentConfiguration(), Times.Between(3, 4, Range.Inclusive));
        }

        #endregion

        #region Stop

        [Test]
        public void Stop_EndsTheService()
        {
            // Arrange
            var maxDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 3;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            var agentConfiguration = new AgentConfiguration()
            {
                AgentsAreEnabled = true,
                BaseUrl = "http://www.example.com",
                CheckIntervalInSeconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds,
                SystemInformationSenderPath = "/api/systeminformation"
            };
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(agentConfigurationProvider.Object, () => { }, () => { });

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();
            Thread.Sleep(500);
            agentCoordinationService.Stop();
            Task.WaitAll(new[] { agentCoordinationServiceTask }, maxDurationInMilliseconds);

            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, maxDurationInMilliseconds);
        }

        #endregion

        #region Stop

        [Test]
        public void Dispose_EndsTheService()
        {
            // Arrange
            var maxDurationInMilliseconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds * 3;
            var agentConfigurationProvider = new Mock<IAgentConfigurationProvider>();

            var agentConfiguration = new AgentConfiguration()
            {
                AgentsAreEnabled = true,
                BaseUrl = "http://www.example.com",
                CheckIntervalInSeconds = AgentCoordinationService.AgentConfigurationCheckIntervalInMilliseconds,
                SystemInformationSenderPath = "/api/systeminformation"
            };
            agentConfigurationProvider.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);

            var agentCoordinationService = new AgentCoordinationService(agentConfigurationProvider.Object, () => { }, () => { });

            // Act
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var agentCoordinationServiceTask = new Task(agentCoordinationService.Start);
            agentCoordinationServiceTask.Start();
            Thread.Sleep(500);
            agentCoordinationService.Dispose();
            Task.WaitAll(new[] { agentCoordinationServiceTask }, maxDurationInMilliseconds);

            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, maxDurationInMilliseconds);
        }

        #endregion
    }
}