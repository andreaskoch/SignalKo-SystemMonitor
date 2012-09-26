if (typeof (jQuery) === 'undefined') {
    throw new Error("jQuery is required to run this component");
}

if (typeof (ko) === 'undefined') {
    throw new Error("Knockout is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
    throw new Error("SystemMonitor is required to run this component");
}

$.extend(SystemMonitor, {

    "AgentConfiguration": (function () {

        function agentConfigurationViewModel() {
            var self = this;

            self.SystemInformationSenderUrl = ko.observable("http://localhost:49785/api/systeminformation");

            self.AgentsAreEnabled = ko.observable("true");
        }

        var viewModel = new agentConfigurationViewModel();
        ko.applyBindings(viewModel);

        return viewModel;

    })()
});