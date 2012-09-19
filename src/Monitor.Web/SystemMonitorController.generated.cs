// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace SignalKo.SystemMonitor.Monitor.Web.Controllers {
    public partial class SystemMonitorController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SystemMonitorController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected SystemMonitorController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Group() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Group);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public SystemMonitorController Actions { get { return MVC.SystemMonitor; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "SystemMonitor";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "SystemMonitor";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string GroupOverview = "GroupOverview";
            public readonly string Group = "Group";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants {
            public const string GroupOverview = "GroupOverview";
            public const string Group = "Group";
        }


        static readonly ActionParamsClass_Group s_params_Group = new ActionParamsClass_Group();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Group GroupParams { get { return s_params_Group; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Group {
            public readonly string groupName = "groupName";
        }
        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Group = "~/Views/SystemMonitor/Group.cshtml";
            public readonly string GroupOverview = "~/Views/SystemMonitor/GroupOverview.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_SystemMonitorController: SignalKo.SystemMonitor.Monitor.Web.Controllers.SystemMonitorController {
        public T4MVC_SystemMonitorController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ActionResult GroupOverview() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.GroupOverview);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Group(string groupName) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Group);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "groupName", groupName);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
