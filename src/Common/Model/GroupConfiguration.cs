using System.Collections;
using System.Collections.Generic;

namespace SignalKo.SystemMonitor.Common.Model
{
	public class GroupConfiguration : IEnumerable<Group>
	{
		public GroupConfiguration()
		{
			this.Groups = new List<Group>();
		}

		public List<Group> Groups { get; set; }

		public IEnumerator<Group> GetEnumerator()
		{
			return this.Groups.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}