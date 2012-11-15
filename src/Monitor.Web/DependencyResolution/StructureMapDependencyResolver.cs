using System;
using System.Web.Http.Dependencies;
using StructureMap;

namespace SignalKo.SystemMonitor.Monitor.Web.DependencyResolution
{
	/// <summary>
	/// The structure map dependency resolver.
	/// </summary>
	public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
	{
		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StructureMapDependencyResolver"/> class.
		/// </summary>
		/// <param name="container">
		/// The container.
		/// </param>
		public StructureMapDependencyResolver(IContainer container)
			: base(container)
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// The begin scope.
		/// </summary>
		/// <returns>
		/// The System.Web.Http.Dependencies.IDependencyScope.
		/// </returns>
		public IDependencyScope BeginScope()
		{
			IContainer child = this.Container.GetNestedContainer();
			return new StructureMapDependencyResolver(child);
		}

		#endregion

		public new object GetService(Type serviceType)
		{
			if (serviceType.IsAbstract || serviceType.IsInterface)
			{
				return ObjectFactory.TryGetInstance(serviceType);
			}

			return ObjectFactory.GetInstance(serviceType);
		}
	}
}