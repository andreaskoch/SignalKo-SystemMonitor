﻿using System.Web.Mvc;

namespace SignalKo.SystemMonitor.Monitor.Web.Controllers
{
	public partial class UIConfigurationController : Controller
	{
		public virtual ActionResult Editor()
		{
			return View();
		}

	}
}