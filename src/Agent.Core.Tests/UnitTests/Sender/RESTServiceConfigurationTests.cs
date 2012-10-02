using System;
using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Agent.Core.Configuration;

namespace Agent.Core.Tests.UnitTests.Sender
{
    [TestFixture]
    public class RESTServiceConfigurationTests
    {
        #region IsValid

        [Test]
        public void IsValid_BaseUrlAndResourcePathAreSet_ResultIsTrue()
        {
            // Arrange
            var config = this.GetConfig("http://localhost", "some/path");

            // Act
            var result = config.IsValid();

            // Assert
            Assert.IsTrue(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_BaseUrlIsValid_ResourcePathIsInvalid_ResultIsFalse(string resourcePath)
        {
            // Arrange
            var config = this.GetConfig("http://localhost", resourcePath);

            // Act
            var result = config.IsValid();

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_BaseUrlIsInvalid_ResourcePathIsValid_ResultIsFalse(string baseUrl)
        {
            // Arrange
            var config = this.GetConfig(baseUrl, "some/path");

            // Act
            var result = config.IsValid();

            // Assert
            Assert.IsFalse(result);
        }

        [TestCase(null, null)]
        [TestCase(null, "")]
        [TestCase(null, " ")]
        [TestCase(null, "some/path")]
        [TestCase("", null)]
        [TestCase("", "")]
        [TestCase("", " ")]
        [TestCase("", "some/path")]
        [TestCase(" ", null)]
        [TestCase(" ", "")]
        [TestCase(" ", " ")]
        [TestCase(" ", "some/path")]
        public void IsValid_BaseUrlAndOrResourcePathIsValid_ResultIsFalse(string baseUrl, string resourcePath)
        {
            // Arrange
            var config = this.GetConfig(baseUrl, resourcePath);

            // Act
            var result = config.IsValid();

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_BaseUrl()
        {
            // Arrange
            var config = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };

            // Act
            string result = config.ToString();

            // Assert
            Assert.IsTrue(result.Contains(config.BaseUrl));
        }

        [Test]
        public void ToString_Contains_ResourcePath()
        {
            // Arrange
            var config = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };

            // Act
            string result = config.ToString();

            // Assert
            Assert.IsTrue(result.Contains(config.ResourcePath));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration();
            var object2 = new RESTServiceConfiguration();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoIdenticalObject_WithDifferentNameCasing_ResultIsTrue()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration { BaseUrl = "http://LOCALHOST", ResourcePath = "SOME/PATH" };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoIdenticalObjects_WithDifferentWhitespaces_ResultIsFalse()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration { BaseUrl = "http://localhost ", ResourcePath = "some/path " };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            RESTServiceConfiguration object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new object();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region GetHashCode

        [Test]
        public void GetHashCode_TwoIdenticalObjects_BothInitialized_HashCodesAreEqual()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_TwoDistinctObjects_HashCodesAreDifferent()
        {
            // Arrange
            var object1 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "some/path" };
            var object2 = new RESTServiceConfiguration { BaseUrl = "http://localhost", ResourcePath = "another/path" };

            // Act
            int hashCodeObject1 = object1.GetHashCode();
            int hashCodeObject2 = object2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_TwoIdenticalObjects_BothUninitialized_HashCodesAreEqual()
        {
            // Arrange
            var package1 = new RESTServiceConfiguration();
            var package2 = new RESTServiceConfiguration();

            // Act
            int hashCodeObject1 = package1.GetHashCode();
            int hashCodeObject2 = package2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCodeObject1, hashCodeObject2);
        }

        [Test]
        public void GetHashCode_SameHashCodeIsReturnedEveryTimeTheMethodIsCalled_AsLongAsTheObjectDoesNotChange()
        {
            // Arrange
            var baseUrl = "http://localhost";
            var resourcePath = "some/path";
            var object1 = new RESTServiceConfiguration { BaseUrl = baseUrl, ResourcePath = resourcePath };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.BaseUrl = baseUrl;
                object1.ResourcePath = resourcePath;
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var hashCodes = new Dictionary<int, RESTServiceConfiguration>();

            for (var i = 0; i < 10000; i++)
            {
                // Act
                var object1 = new RESTServiceConfiguration { BaseUrl = "http://" + Guid.NewGuid().ToString(), ResourcePath = "api/" + Guid.NewGuid().ToString() };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion

        #region utility methods

        private RESTServiceConfiguration GetConfig(string baseUrl, string resourcePath)
        {
            return new RESTServiceConfiguration { BaseUrl = baseUrl, ResourcePath = resourcePath };
        }

        #endregion
    }
}