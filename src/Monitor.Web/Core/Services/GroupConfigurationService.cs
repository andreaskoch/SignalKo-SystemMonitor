using System;

using SignalKo.SystemMonitor.Common.Model;
using SignalKo.SystemMonitor.Monitor.Web.Controllers;
using SignalKo.SystemMonitor.Monitor.Web.Core.DataAccess;

namespace SignalKo.SystemMonitor.Monitor.Web.Core.Services
{
	public class GroupConfigurationService : IGroupConfigurationService
	{
		private readonly IConfigurationDataAccessor<GroupConfiguration> groupConfigurationAccessor;

		public GroupConfigurationService(IConfigurationDataAccessor<GroupConfiguration> groupConfigurationAccessor)
		{
			if (groupConfigurationAccessor == null)
			{
				throw new ArgumentNullException("groupConfigurationAccessor");
			}

			this.groupConfigurationAccessor = groupConfigurationAccessor;
		}

		public GroupConfiguration GetGroupConfiguration()
		{
			return this.groupConfigurationAccessor.Load() ?? new GroupConfiguration();
		}

		public void SaveGroupConfiguration(GroupConfiguration groupConfiguration)
		{
			this.groupConfigurationAccessor.Store(groupConfiguration);
		}
	}
}