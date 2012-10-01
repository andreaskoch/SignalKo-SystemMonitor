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

    "AgentConfiguration": (function (moduleConfiguration) {

        function agentConfigurationViewModel(viewModelConfiguration) {
            var self = this;

            self.BaseUrl = ko.observable();
            self.SystemInformationSenderPath = ko.observable();
            self.AgentsAreEnabled = ko.observable(true);
            self.CheckIntervalInSeconds = ko.observable();

            self.AgentsAreEnabled.ForEditing = ko.computed({
                read: function () {
                    return self.AgentsAreEnabled().toString();
                },
                write: function (newValue) {
                    return self.AgentsAreEnabled(newValue === "true");
                },
                owner: self
            });

            var showSuccessMessage = function (message) {
                if (self.SuccessCallback && typeof(self.SuccessCallback) === 'function') {
                    self.SuccessCallback(message);
                }
            };

            var showErrorMessage = function (message) {
                if (self.ErrorCallback && typeof(self.ErrorCallback) === 'function') {
                    self.SuccessCallback(message);
                }
            };

            self.LoadConfiguration = function () {
                $.ajax({
                    url: "/api/agentconfiguration",
                    type: "GET",
                    success: function (agentConfiguration) {
                        if (!agentConfiguration) {
                            showErrorMessage("Cannot load empty agent configuration.");
                            return;
                        }

                        self.BaseUrl(agentConfiguration.BaseUrl);
                        self.SystemInformationSenderPath(agentConfiguration.SystemInformationSenderPath);
                        self.AgentsAreEnabled(agentConfiguration.AgentsAreEnabled);
                        self.CheckIntervalInSeconds(agentConfiguration.CheckIntervalInSeconds);

                        showSuccessMessage("Agent configuration loaded.");
                    },
                    error: function () {
                        showErrorMessage("Cannot retrieve agent configuration from server.");
                    }
                });
            };

            self.SaveConfiguration = function () {
                $.ajax({
                    url: "/api/agentconfiguration",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({
                        "BaseUrl": self.BaseUrl(),
                        "SystemInformationSenderPath": self.SystemInformationSenderPath(),
                        "AgentsAreEnabled": self.AgentsAreEnabled(),
                        "CheckIntervalInSeconds": self.CheckIntervalInSeconds()
                    }),
                    success: function () {
                        showSuccessMessage("Agent configuration has bee saved successfully.");
                    },
                    error: function () {
                        showErrorMessage("Cannot save agent configuration to server.");
                    }
                });
            };

            self.LoadConfiguration();

            var applyConfiguration = function (configuration) {
                if (!configuration) {
                    return;
                }

                self.SuccessCallback = configuration.SuccessCallback;
                self.ErrorCallback = configuration.ErrorCallback;
            };

            applyConfiguration(viewModelConfiguration);
        }

        var viewModel = new agentConfigurationViewModel(moduleConfiguration);
        ko.applyBindings(viewModel);

        return viewModel;

    })(agentConfigurationOptions)
});