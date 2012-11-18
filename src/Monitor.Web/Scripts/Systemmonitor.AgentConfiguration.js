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
		var httpResponseContentCheck = "Web Page Content Check";
		var httpResponseTimeCheck = "Response Time Check";
		var healthPageCheck = "Health Page Check";
		var sqlCheck = "Sql Check";

		var collectorTypes = [systemPerformanceCheck, httpStatusCodeCheck, httpResponseContentCheck, httpResponseTimeCheck, healthPageCheck, sqlCheck];
		
		function getHumanReadableTimespanFromSeconds(seconds) {
			return "{0} seconds".format(seconds);
		}

		function systemPerformanceCheckDefinition(options) {
			var self = this;
			
			self.CollectorType = systemPerformanceCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 1);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function httpStatusCodeCheckDefinition(options) {
			var self = this;

			self.CollectorType = httpStatusCodeCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 60);
			self.CheckUrl = ko.observable(options.CheckUrl);
			self.Hostheader = ko.observable(options.Hostheader);
			self.ExpectedStatusCode = ko.observable(options.ExpectedStatusCode);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function webPageContentCheckDefinition(options) {
			var self = this;

			self.CollectorType = httpResponseContentCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 60);
			self.CheckUrl = ko.observable(options.CheckUrl);
			self.Hostheader = ko.observable(options.Hostheader);
			self.CheckPattern = ko.observable(options.CheckPattern);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function responseTimeCheckDefinition(options) {
			var self = this;

			self.CollectorType = httpResponseTimeCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 60);
			self.CheckUrl = ko.observable(options.CheckUrl);
			self.Hostheader = ko.observable(options.Hostheader);
			self.MaxResponseTimeInSeconds = ko.observable(options.MaxResponseTimeInSeconds || 15);
			
			self.MaxResponseTimeHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.MaxResponseTimeInSeconds());
			}, this);
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		function healthPageCheckDefinition(options) {
			var self = this;

			self.CollectorType = healthPageCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 60);
			self.CheckUrl = ko.observable(options.CheckUrl);
			self.Hostheader = ko.observable(options.Hostheader);
			self.MaxResponseTimeInSeconds = ko.observable(options.MaxResponseTimeInSeconds || 10);
			
			self.MaxResponseTimeHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.MaxResponseTimeInSeconds());
			}, this);
			
			self.CheckIntervalHumanReadable = ko.computed(function() {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}
		
		function sqlCheckDefinition(options) {
			var self = this;

			self.CollectorType = sqlCheck;
			self.CheckIntervalInSeconds = ko.observable(options.CheckIntervalInSeconds || 60);
			self.ConnectionString = ko.observable(options.ConnectionString);
			self.SqlQuery = ko.observable(options.SqlQuery);

			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);
		}

		/**
			Creates a new agent instance configuration view model.
			@class Represents an agent instance configuration view model.
			@param {Object} instanceConfiguration An object containing the view model properties that shall be set.
		*/
		function agentInstanceConfigurationViewModel(instanceConfiguration) {
			
			/**
				The current agent configuration view model instance
			*/
			var self = this;

			/**
				Gets the machine/computer name of this agent instance.
				@returns {String} Returns the computer name of this agent instance.
			*/
			self.MachineName = instanceConfiguration.MachineName;
			
			/**
				A value indicating whether this agent is enabled or not.
				@returns {Boolean} Returns true if this agent is enabled; otherwise false.
			*/
			self.AgentIsEnabled = ko.observable(instanceConfiguration.AgentIsEnabled);
			self.AgentIsEnabled.ForEditing = ko.computed({
				read: function () {
					return self.AgentIsEnabled().toString();
				},
				write: function (newValue) {
					return self.AgentIsEnabled(newValue === "true");
				},
				owner: self
			}, this);
			
			self.CollectorDefinitions = ko.observableArray();

			var getCollectorDefinitionByType = function(collectorDefinitionType) {
				var collectorDefinitions = self.CollectorDefinitions();
				
				for (var i = 0; i < collectorDefinitions.length; i++) {
					var collectorDefinitionViewModel = collectorDefinitions[i];
					if (collectorDefinitionViewModel.CollectorType === collectorDefinitionType) {
						return collectorDefinitionViewModel;
					}
				}

				return null;
			};
			
			self.SystemPerformanceCollector = ko.computed(function () {
				return getCollectorDefinitionByType(systemPerformanceCheck);
			}, this);

			self.HttpStatusCodeCheck = ko.computed(function () {
				return getCollectorDefinitionByType(httpStatusCodeCheck);
			}, this);
			
			self.HttpResponseContentCheck = ko.computed(function () {
				return getCollectorDefinitionByType(httpResponseContentCheck);
			}, this);
			
			self.HttpResponseTimeCheck = ko.computed(function () {
				return getCollectorDefinitionByType(httpResponseTimeCheck);
			}, this);
			
			self.HealthPageCheck = ko.computed(function () {
				return getCollectorDefinitionByType(healthPageCheck);
			}, this);
			
			self.SqlCheck = ko.computed(function () {
				return getCollectorDefinitionByType(sqlCheck);
			}, this);
			
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
				@param {object} options Initialization parameters
			*/
			self.AddNewCollectorDefinition = function (collectorType, options) {
				
				if (!collectorType) {
					throw new Error("You must specifiy the type of the collector defintion you want to add.");
				}
				
				if (!options) {
					options = { };
				}
				
				switch (collectorType) {
					case systemPerformanceCheck:
						{
							self.CollectorDefinitions.push(new systemPerformanceCheckDefinition(options));
							break;
						}
					case httpStatusCodeCheck:
						{
							self.CollectorDefinitions.push(new httpStatusCodeCheckDefinition(options));
							break;
						}
					case httpResponseContentCheck:
						{
							self.CollectorDefinitions.push(new webPageContentCheckDefinition(options));
							break;
						}
					case httpResponseTimeCheck:
						{
							self.CollectorDefinitions.push(new responseTimeCheckDefinition(options));
							break;
						}
					case healthPageCheck:
						{
							self.CollectorDefinitions.push(new healthPageCheckDefinition(options));
							break;
						}

					case sqlCheck:
						{
							self.CollectorDefinitions.push(new sqlCheckDefinition(options));
							break;
						}

					default:
						throw new Error("The collector type '{0}' is unknown.".format(collectorType));
				}
			};

			/**
				Remove the supplied collector definition view model.
				@param {agentInstanceCollectorDefinitionViewModel} collectorDefinitionViewModel The collector definition to remove
			*/
			self.RemoveCollectorDefinition = function(collectorDefinitionViewModel) {
				self.CollectorDefinitions.remove(collectorDefinitionViewModel);
			};
			
			/**
				Initialize collector definitions
			*/
			if (instanceConfiguration.SystemPerformanceCollector) {
				self.AddNewCollectorDefinition(systemPerformanceCheck, instanceConfiguration.SystemPerformanceCollector);
			}
			
			if (instanceConfiguration.HttpStatusCodeCheck) {
				self.AddNewCollectorDefinition(httpStatusCodeCheck, instanceConfiguration.HttpStatusCodeCheck);
			}
			
			if (instanceConfiguration.HttpResponseContentCheck) {
				self.AddNewCollectorDefinition(httpResponseContentCheck, instanceConfiguration.HttpResponseContentCheck);
			}
			
			if (instanceConfiguration.HttpResponseTimeCheck) {
				self.AddNewCollectorDefinition(httpResponseTimeCheck, instanceConfiguration.HttpResponseTimeCheck);
			}
			
			if (instanceConfiguration.HealthPageCheck) {
				self.AddNewCollectorDefinition(healthPageCheck, instanceConfiguration.HealthPageCheck);
			}
			
			if (instanceConfiguration.SqlCheck) {
				self.AddNewCollectorDefinition(sqlCheck, instanceConfiguration.SqlCheck);
			}
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
			
			self.CheckIntervalHumanReadable = ko.computed(function () {
				return getHumanReadableTimespanFromSeconds(self.CheckIntervalInSeconds());
			}, this);

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

			var successCallback = function (message) { };
			var errorCallback = function (message) { };

			/**
				Save the agent configuration
			*/
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
						successCallback("Agent configuration has bee saved successfully.");
					},
					error: function () {
						errorCallback("Cannot save agent configuration to server.");
					}
				});
			};

			/**
				Apply external configuration values
			*/
			(function(configuration) {
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
				
				if (configuration.SuccessCallback && typeof (configuration.SuccessCallback) === 'function') {
					successCallback = function(message) {
						configuration.SuccessCallback(message);
					};
				}
				
				if (configuration.ErrorCallback && typeof (configuration.ErrorCallback) === 'function') {
					errorCallback = function(message) {
						configuration.ErrorCallback(message);
					};
				}
			})(viewModelConfiguration);

			/**
				Initialize the view model and apply the Knockout bindings
			*/
			(function() {
				$.ajax({
					url: self.GetAgentConfigurationApiUrl(),
					type: "GET",
					success: function (agentConfiguration) {
						if (!agentConfiguration) {
							errorCallback("Cannot load empty agent configuration.");
							return;
						}

						self.Hostaddress(agentConfiguration.Hostaddress);
						self.Hostname(agentConfiguration.Hostname);
						self.SystemInformationSenderPath(agentConfiguration.SystemInformationSenderPath);
						self.AgentsAreEnabled(agentConfiguration.AgentsAreEnabled);
						self.CheckIntervalInSeconds(agentConfiguration.CheckIntervalInSeconds);

						var agentInstanceViewModels = [];
						for (var i = 0; i < agentConfiguration.AgentInstanceConfigurations.length; i++) {
							var agentInstanceConfiguration = agentConfiguration.AgentInstanceConfigurations[i];

							var agentInstanceViewModel = new agentInstanceConfigurationViewModel(agentInstanceConfiguration);
							agentInstanceViewModels.push(agentInstanceViewModel);
						}
						self.AgentInstanceConfigurations = ko.observableArray(agentInstanceViewModels);

						ko.applyBindings(self);
						successCallback("Agent configuration loaded.");
					},
					error: function () {
						errorCallback("Cannot retrieve agent configuration from server.");
					}
				});
			})();
		}

		var viewModel = new agentConfigurationViewModel(moduleConfiguration);
		return viewModel;

	})(agentConfigurationOptions)
});