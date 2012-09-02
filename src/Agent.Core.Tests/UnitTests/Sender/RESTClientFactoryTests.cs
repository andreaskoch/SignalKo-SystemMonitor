using System;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Sender;

namespace Agent.Core.Tests.UnitTests.Sender
{
    [TestFixture]
    public class RESTClientFactoryTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRESTClient_BaseUrlParameterIsInvalid_ArgumentExceptionIsThrown(string baseUrl)
        {
            // Arrange
            var clientFactory = new RESTClientFactory();

            // Act
            clientFactory.GetRESTClient(baseUrl);
        }
    }
}