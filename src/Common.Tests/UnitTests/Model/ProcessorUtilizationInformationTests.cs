using System.Collections.Generic;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
    public class ProcessorUtilizationInformationTests
    {
        #region Default Values

        [Test]
        public void Default_ProcessorUtilizationInPercent_IsSetToZero()
        {
            // Arrange
            var processorUtilizationInformation = new ProcessorUtilizationInformation();

            // Assert
            Assert.AreEqual(0.0d, processorUtilizationInformation.ProcessorUtilizationInPercent);
        }

        #endregion

        #region ToString

        [Test]
        public void ToString_Contains_ProcessorUtilizationInPercent()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };

            // Act
            string result = object1.ToString();

            // Assert
            Assert.IsTrue(result.Contains(object1.ProcessorUtilizationInPercent.ToString()));
        }

        #endregion

        #region Equals

        [Test]
        public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };
            var object2 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoUninitializedObjects_ResultIsTrue()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation();
            var object2 = new ProcessorUtilizationInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNull_ResultIsFalse()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };
            ProcessorUtilizationInformation object2 = null;

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };
            var object2 = new ProcessorUtilizationInformation();

            // Act
            bool result = object1.Equals(object2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
        {
            // Arrange
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };
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
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };
            var object2 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 50.0d };

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
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 90.0d };
            var object2 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 10.0d };

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
            var package1 = new ProcessorUtilizationInformation();
            var package2 = new ProcessorUtilizationInformation();

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
            var processorUtilizationInPercent = 50.0d;
            var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = processorUtilizationInPercent };

            int expectedHashcode = object1.GetHashCode();

            for (var i = 0; i < 100; i++)
            {
                // Act
                object1.ProcessorUtilizationInPercent = processorUtilizationInPercent;
                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.AreEqual(expectedHashcode, generatedHashCode);
            }
        }

        [Test]
        public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
        {
            var hashCodes = new Dictionary<int, ProcessorUtilizationInformation>();

            for (var i = 0; i < 100; i++)
            {
                // Act
                var object1 = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = i };

                int generatedHashCode = object1.GetHashCode();

                // Assert
                Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
                hashCodes.Add(generatedHashCode, object1);
            }
        }

        #endregion         
    }
}