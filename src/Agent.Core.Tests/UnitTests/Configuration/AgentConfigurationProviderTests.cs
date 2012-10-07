using System;
using System.Threading;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;
using SignalKo.SystemMonitor.Common.Model;

namespace Agent.Core.Tests.UnitTests.Configuration
{
    [TestFixture]
    public class AgentConfigurationProviderTests
    {
        #region constructor

        [Test]
        public void Constructor_AllParametersAreSet_ObjectIsInstantiated()
        {
            // Arrange
            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();

            // Act
            var agentConfigurationProvider = new AgentConfigurationProvider(agentConfigurationAccessor.Object);

            // Assert
            Assert.IsNotNull(agentConfigurationProvider);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AgentConfigurationAccessorParameterIsNull_ArgumentNullExceptionIsThrown()
        {
            // Act
            new AgentConfigurationProvider(null);
        }

        #endregion

        #region Timer

        [Test]
        public void Timer_GetAgentConfiguration_IsCalled_On_AgentConfigurationAccessor_FromWithinTheConstructor()
        {
            // Arrange
            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();

            // Act
            new AgentConfigurationProvider(agentConfigurationAccessor.Object);

            // Assert
            agentConfigurationAccessor.Verify(a => a.GetAgentConfiguration(), Times.Once());
        }

        [Test]
        public void Timer_GetAgentConfiguration_IsCalled_On_AgentConfigurationAccessor_TwiceDuringTheDefaultInterval()
        {
            // Arrange
            var slackTimeInMilliseconds = 2000;
            var waitTimeInMilliseconds = (AgentConfigurationProvider.DefaultCheckIntervalInSeconds * 1000) + slackTimeInMilliseconds;

            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();

            // Act
            new AgentConfigurationProvider(agentConfigurationAccessor.Object);
            Thread.Sleep(waitTimeInMilliseconds);

            // Assert
            agentConfigurationAccessor.Verify(a => a.GetAgentConfiguration(), Times.Exactly(2));
        }

        [Test]
        public void Timer_IntervalIsChangedToOnceSecond_GetConfigurationIsCalledEverySecond()
        {
            // Arrange
            int newCheckIntervalInSeconds = 1;
            int waitTimeInSeconds = AgentConfigurationProvider.DefaultCheckIntervalInSeconds;
            int expectedNumberOfTimesGetConfigurationIsCalled = (int)((double)waitTimeInSeconds / newCheckIntervalInSeconds);

            int waitTimeInMilliseconds = waitTimeInSeconds * 1000;

            var agentConfigurationWithReducedInterval = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                Hostaddress = "127.0.0.1",
                Hostname = "www.example.com",
                CheckIntervalInSeconds = newCheckIntervalInSeconds,
                SystemInformationSenderPath = "/api/systeminformation"
            };

            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();
            agentConfigurationAccessor.Setup(a => a.GetAgentConfiguration()).Returns(agentConfigurationWithReducedInterval);

            // Act
            var agentConfigurationProvider = new AgentConfigurationProvider(agentConfigurationAccessor.Object);
            Thread.Sleep(waitTimeInMilliseconds);

            // Assert
            agentConfigurationAccessor.Verify(a => a.GetAgentConfiguration(), Times.AtLeast(expectedNumberOfTimesGetConfigurationIsCalled));
        }

        #endregion

        #region GetConfiguration

        [Test]
        public void GetConfiguration_ReturnsNullIfTheAgentConfigurationAccessorReturnsNull()
        {
            // Arrange
            AgentConfiguration agentConfiguration = null;
            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();
            agentConfigurationAccessor.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);
            var agentConfigurationProvider = new AgentConfigurationProvider(agentConfigurationAccessor.Object);
            Thread.Sleep(2000);

            // Act
            var result = agentConfigurationProvider.GetAgentConfiguration();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetConfiguration_ReturnsTheSameAgentConfigurationThatIsReturnedByTheAgentConfigurationAccessor()
        {
            // Arrange
            var agentConfiguration = new AgentConfiguration
            {
                AgentsAreEnabled = true,
                Hostaddress = "127.0.0.1",
                Hostname = "www.example.com",
                CheckIntervalInSeconds = 10,
                SystemInformationSenderPath = "/api/systeminformation"
            };
            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();
            agentConfigurationAccessor.Setup(a => a.GetAgentConfiguration()).Returns(agentConfiguration);
            var agentConfigurationProvider = new AgentConfigurationProvider(agentConfigurationAccessor.Object);
            Thread.Sleep(2000);

            // Act
            var result = agentConfigurationProvider.GetAgentConfiguration();

            // Assert
            Assert.AreEqual(agentConfiguration, result);
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_TimerIsNotExecutedAfterDisposeHasBeenCalled()
        {
            // Arrange
            int newCheckIntervalInSeconds = 1;
            int waitTimeInMilliseconds = (newCheckIntervalInSeconds * 1000) * 5;
            var agentConfigurationWithReducedInterval = new AgentConfiguration
                {
                    AgentsAreEnabled = true,
                    Hostaddress = "127.0.0.1",
                    Hostname = "www.example.com",
                    CheckIntervalInSeconds = newCheckIntervalInSeconds,
                    SystemInformationSenderPath = "/api/systeminformation"
                };

            var agentConfigurationAccessor = new Mock<IAgentConfigurationAccessor>();
            agentConfigurationAccessor.Setup(a => a.GetAgentConfiguration()).Returns(agentConfigurationWithReducedInterval);

            var agentConfigurationProvider = new AgentConfigurationProvider(agentConfigurationAccessor.Object);

            // Act
            agentConfigurationProvider.Dispose();
            Thread.Sleep(waitTimeInMilliseconds);

            // Assert
            agentConfigurationAccessor.Verify(a => a.GetAgentConfiguration(), Times.AtMostOnce());
        }

        #endregion
    }
}