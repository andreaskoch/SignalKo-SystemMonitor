using System.Collections.Generic;

using Moq;

using NUnit.Framework;

using SignalKo.SystemMonitor.Common.Model;

namespace Common.Tests.UnitTests.Model
{
	[TestFixture]
	public class SystemPerformanceDataTests
	{
		#region Default Values

		[Test]
		public void Default_ProcessorStatus_IsNull()
		{
			// Arrange
			var performanceData = new SystemPerformanceData();

			// Assert
			Assert.IsNull(performanceData.ProcessorStatus);
		}

		[Test]
		public void Default_MemoryStatus_IsNull()
		{
			// Arrange
			var performanceData = new SystemPerformanceData();

			// Assert
			Assert.IsNull(performanceData.MemoryStatus);
		}

		[Test]
		public void Default_StorageStatus_IsNull()
		{
			// Arrange
			var performanceData = new SystemPerformanceData();

			// Assert
			Assert.IsNull(performanceData.StorageStatus);
		}

		#endregion

		#region ToString

		[Test]
		public void ToString_Contains_ProcessorStatus()
		{
			// Arrange
			var expectedString = "ProzessorStatus 1234";
			var processorUtilizationInformation = new Mock<ProcessorUtilizationInformation>();
			processorUtilizationInformation.Setup(p => p.ToString()).Returns(expectedString);

			var object1 = new SystemPerformanceData { ProcessorStatus = processorUtilizationInformation.Object };

			// Act
			string result = object1.ToString();

			// Assert
			Assert.IsTrue(result.Contains(expectedString));
		}

		[Test]
		public void ToString_Contains_MemoryStatus()
		{
			// Arrange
			var expectedString = "Memory Status 1234";
			var systemMemoryInformation = new Mock<SystemMemoryInformation>();
			systemMemoryInformation.Setup(s => s.ToString()).Returns(expectedString);

			var object1 = new SystemPerformanceData { MemoryStatus = systemMemoryInformation.Object };

			// Act
			string result = object1.ToString();

			// Assert
			Assert.IsTrue(result.Contains(expectedString));
		}

		[Test]
		public void ToString_Contains_StorageStatus()
		{
			// Arrange
			var expectedString = "Storage Status 1234";
			var storageInformation = new Mock<SystemStorageInformation>();
			storageInformation.Setup(s => s.ToString()).Returns(expectedString);

			var object1 = new SystemPerformanceData { StorageStatus = storageInformation.Object };

			// Act
			string result = object1.ToString();

			// Assert
			Assert.IsTrue(result.Contains(expectedString));
		}

		#endregion

		#region Equals

		[Test]
		public void Equals_TwoIdenticalInitializedObjects_ResultIsTrue()
		{
			// Arrange
			var object1 = new SystemPerformanceData
				{
					MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
					ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
				};

			var object2 = new SystemPerformanceData
				{
					MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
					ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
				};

			// Act
			bool result = object1.Equals(object2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Equals_TwoUninitializedObjects_ResultIsTrue()
		{
			// Arrange
			var object1 = new SystemPerformanceData();
			var object2 = new SystemPerformanceData();

			// Act
			bool result = object1.Equals(object2);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void Equals_SuppliedObjectIsNull_ResultIsFalse()
		{
			// Arrange
			var object1 = new SystemPerformanceData
				{
					MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
					ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
				};

			SystemPerformanceData object2 = null;

			// Act
			bool result = object1.Equals(object2);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Equals_SuppliedObjectIsNotInitialized_ResultIsFalse()
		{
			// Arrange
			var object1 = new SystemPerformanceData
				{
					MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
					ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
				};

			var object2 = new SystemPerformanceData();

			// Act
			bool result = object1.Equals(object2);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void Equals_SuppliedObjectIsOfOtherType_ResultIsFalse()
		{
			// Arrange
			var object1 = new SystemPerformanceData
				{
					MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
					ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
				};

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
			var object1 = new SystemPerformanceData
			{
				MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
				ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
			};

			var object2 = new SystemPerformanceData
			{
				MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
				ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
			};

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
			var object1 = new SystemPerformanceData
			{
				MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 },
				ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
			};

			var object2 = new SystemPerformanceData
			{
				MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 4 },
				ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 90d }
			};

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
			var package1 = new SystemPerformanceData();
			var package2 = new SystemPerformanceData();

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
			var memoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = 3 };
			var processorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d };
			var storageStatus = new SystemStorageInformation();

			var object1 = new SystemPerformanceData { MemoryStatus = memoryStatus, ProcessorStatus = processorStatus, StorageStatus = storageStatus };

			int expectedHashcode = object1.GetHashCode();

			for (var i = 0; i < 100; i++)
			{
				// Act
				object1.MemoryStatus = memoryStatus;
				object1.ProcessorStatus = processorStatus;
				object1.StorageStatus = storageStatus;

				int generatedHashCode = object1.GetHashCode();

				// Assert
				Assert.AreEqual(expectedHashcode, generatedHashCode);
			}
		}

		[Test]
		public void GetHashCode_ForAllUniqueObject_AUniqueHashCodeIsReturned()
		{
			var hashCodes = new Dictionary<int, SystemPerformanceData>();

			for (var i = 0; i < 10000; i++)
			{
				// Act
				var object1 = new SystemPerformanceData
					{
						MemoryStatus = new SystemMemoryInformation { AvailableMemoryInGB = i },
						ProcessorStatus = new ProcessorUtilizationInformation { ProcessorUtilizationInPercent = 30d }
					};

				int generatedHashCode = object1.GetHashCode();

				// Assert
				Assert.IsFalse(hashCodes.ContainsKey(generatedHashCode));
				hashCodes.Add(generatedHashCode, object1);
			}
		}

		#endregion
	}
}