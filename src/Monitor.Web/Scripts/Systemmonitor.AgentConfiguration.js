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
		
		var systemPerformanceCheck = "System Performance";
		var httpStatusCodeCheck = "HTTP Status Code Check";
		var webPageContentCheck = "Web Page Content Check";
		var responseTimeCheck = "Response Time Check";
		var healthPageCheck = "Health Page Check";

		var collectorTypes = [systemPerformanceCheck, httpStatusCodeCheck, webPageContentCheck, responseTimeCheck, healthPageCheck];
		
		function getHumanReadableTimespanFromSeconds(seconds) {
			return "{0} seconds".format(seconds);
		}

		function systemPerformanceCheckDefinition() {
			var self = this;
			
			self.CheckIntervalInSeconds = ko.observable(1);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function httpStatusCodeCheckDefinition() {
			var self = this;

			self.CheckIntervalInSeconds = ko.observable(1);
			self.CheckUrl = ko.observable();
			self.Hostheader = ko.observable();
			self.ExpectedStatusCode = ko.observable();
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function webPageContentCheckDefinition() {
			var self = this;

			self.CheckIntervalInSeconds = ko.observable(1);
			self.CheckUrl = ko.observable();
			self.Hostheader = ko.observable();
			self.CheckPattern = ko.observable();
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function responseTimeCheckDefinition() {
			var self = this;

			self.CheckIntervalInSeconds = ko.observable(1);
			self.CheckUrl = ko.observable();
			self.Hostheader = ko.observable();
			self.MaxResponseTimeInSeconds = ko.observable(1);
			
			self.MaxResponseTimeHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.MaxResponseTimeInSeconds());
			}, this);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function healthPageCheckDefinition() {
			var self = this;

			self.CheckIntervalInSeconds = ko.observable(120);
			self.CheckUrl = ko.observable();
			self.Hostheader = ko.observable();
			self.MaxResponseTimeInSeconds = ko.observable(1);
			
			self.MaxResponseTimeHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.MaxResponseTimeInSeconds());
			}, this);
			
			self.CheckIntervalHumanReadable = ko.computed(function() {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function agentInstanceCollectorDefinitionViewModel(collectorType) {
			var self = this;

			self.CollectorType = collectorType;

			switch (collectorType) {
				case systemPerformanceCheck:
					{
						_.extend(self, new systemPerformanceCheckDefinition());
						break;
					}
				case httpStatusCodeCheck:
					{
						_.extend(self, new httpStatusCodeCheckDefinition());
						break;
					}
				case webPageContentCheck:
					{
						_.extend(self, new webPageContentCheckDefinition());
						break;
					}
				case responseTimeCheck:
					{
						_.extend(self, new responseTimeCheckDefinition());
						break;
					}
				case healthPageCheck:
					{
						_.extend(self, new healthPageCheckDefinition());
						break;
					}
				default:
					throw new Error("The collector type '{0}' is unknown.".format(collectorType));
			}
		}

		/**
			Creates a new agent instance configuration view model.
			@class Represents an agent instance configuration view model.
		*/
		function agentInstanceConfigurationViewModel(instanceName) {
			
			/**
				The current agent configuration view model instance
			*/
			var self = this;

			/**
				Gets the machine/computer name of this agent instance.
				@returns {String} Returns the computer name of this agent instance.
			*/
			self.MachineName = instanceName;
			
			/**
				A value indicating whether this agent is enabled or not.
				@returns {Boolean} Returns true if this agent is enabled; otherwise false.
			*/
			self.AgentIsEnabled = ko.observable(true);
			self.AgentIsEnabled.ForEditing = ko.computed({
				read: function () {
					return self.AgentIsEnabled().toString();
				},
				write: function (newValue) {
					return self.AgentIsEnabled(newValue === "true");
				},
				owner: self
			}, this);

			/**
				A list of all collector definitions that are currently assigned to this agent instance
				@returns {Array} Returns an array of {agentInstanceCollectorDefinitionViewModel} objects.
			*/
			self.CollectorDefinitions = ko.observableArray();
			
			/**
				A list of all collector types that are currently assigned to this agent instance
				@returns {Array} Returns a string array of the collector types that are currently in use.
			*/
			self.AvailableCollectorTypes = ko.computed(function () {
				var collectorDefinitions = self.CollectorDefinitions();
				var availableCollectorTypes = collectorTypes;
				for (var i = 0; i < collectorDefinitions.length; i++) {
					var collectorDefinitionViewModel = collectorDefinitions[i];
					availableCollectorTypes = _.without(availableCollectorTypes, collectorDefinitionViewModel.CollectorType);
				}

				return availableCollectorTypes;
			}, this);

			/**
				Add a new collector definition with the specified type
				@param {string} collectorType The type of the collector definition to add (System Performance | HTTP Status Code Check | Web Page Content Check | Response Time Check | Health Page Check)
			*/
			self.AddNewCollectorDefinition = function (collectorType) {
				var collectorDefinition = new agentInstanceCollectorDefinitionViewModel(collectorType);
				self.CollectorDefinitions.push(collectorDefinition);
			};

			/**
				Remove the supplied collector definition view model.
				@param {agentInstanceCollectorDefinitionViewModel} collectorDefinitionViewModel The collector definition to remove
			*/
			self.RemoveCollectorDefinition = function (collectorDefinitionViewModel) {
				self.CollectorDefinitions.remove(collectorDefinitionViewModel);
			};
		}

		/**
			Creates an agent configuration view model.
			@class Represents an agent configuration view model.
		*/
		function agentConfigurationViewModel(viewModelConfiguration) {
			
			/**
				The current agent configuration view model.
			*/
			var self = this;

			self.Hostaddress = ko.observable();
			self.Hostname = ko.observable();
			self.SystemInformationSenderPath = ko.observable();
			self.AgentsAreEnabled = ko.observable(true);
			self.CheckIntervalInSeconds = ko.observable();
			self.AgentInstanceConfigurations = ko.observableArray();

			self.AgentsAreEnabled.ForEditing = ko.computed({
				read: function () {
					return self.AgentsAreEnabled().toString();
				},
				write: function (newValue) {
					return self.AgentsAreEnabled(newValue === "true");
				},
				owner: self
			}, this);

			var showSuccessMessage = function (message) {
				if (self.SuccessCallback && typeof (self.SuccessCallback) === 'function') {
					self.SuccessCallback(message);
				}
			};

			var showErrorMessage = function (message) {
				if (self.ErrorCallback && typeof (self.ErrorCallback) === 'function') {
					self.SuccessCallback(message);
				}
			};

			self.LoadConfiguration = function () {
				$.ajax({
					url: self.GetAgentConfigurationApiUrl(),
					type: "GET",
					success: function (agentConfiguration) {
						if (!agentConfiguration) {
							showErrorMessage("Cannot load empty agent configuration.");
							return;
						}

						self.Hostaddress(agentConfiguration.Hostaddress);
						self.Hostname(agentConfiguration.Hostname);
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
					url: self.GetAgentConfigurationApiUrl(),
					type: "POST",
					contentType: "application/json",
					data: function () {
						var jsonData = ko.toJSON(self);
						return jsonData;
					}(),
					success: function () {
						showSuccessMessage("Agent configuration has bee saved successfully.");
					},
					error: function () {
						showErrorMessage("Cannot save agent configuration to server.");
					}
				});
			};

			var applyConfiguration = function (configuration) {
				if (!configuration) {
					return;
				}

				/**
					Get the AgentConfiguration API URL.
					@name GetAgentConfigurationApiUrl
				*/
				self.GetAgentConfigurationApiUrl = function () {
					return configuration.AgentConfigurationApiUrl;
				};

				/**
					Initialize the agent-instance configuration view models.
				*/
				var agentInstanceViewModels = [];
				if (configuration.KnownAgents && configuration.KnownAgents.length > 0) {
					for (var i = 0; i < configuration.KnownAgents.length; i++) {
						agentInstanceViewModels.push(new agentInstanceConfigurationViewModel(configuration.KnownAgents[i]));
					}
				}
				self.AgentInstanceConfigurations = ko.observableArray(agentInstanceViewModels);

				/**
					Initialize the success- and error callback functions.
				*/
				self.SuccessCallback = configuration.SuccessCallback;
				self.ErrorCallback = configuration.ErrorCallback;
			};

			applyConfiguration(viewModelConfiguration);

			self.LoadConfiguration();
		}

		var viewModel = new agentConfigurationViewModel(moduleConfiguration);
		ko.applyBindings(viewModel);

		return viewModel;

	})(agentConfigurationOptions)
});