if (typeof (jQuery) === 'undefined') {
	throw new Error("jQuery is required to run this component");
}

if (typeof (ko) === 'undefined') {
	throw new Error("Knockout is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
	throw new Error("SystemMonitor is required to run this component");
}

//control visibility, give element focus, and select the contents (in order)
ko.bindingHandlers.visibleAndSelect = {
	update: function (element, valueAccessor) {
		ko.bindingHandlers.visible.update(element, valueAccessor);
		if (valueAccessor()) {
			setTimeout(function () {
				$(element).find("input").focus().select();
			}, 0); //new tasks are not in DOM yet
		}
	}
};

$.extend(SystemMonitor, {

	"UIConfiguration": (function (moduleConfiguration) {

		function groupViewModel(options) {
			var self = this;

			self.Name = ko.observable(options.Name);
			self.Agents = ko.observableArray(options.Agents);
		}

		/**
			Creates an group configuration view model.
			@class Represents an agent configuration view model.
		*/
		function groupConfigurationViewModel(viewModelConfiguration) {

			/**
				The current group configuration view model.
			*/
			var self = this;

			self.Groups = ko.observableArray();
			self.UnassignedAgents = ko.observableArray();

			//self.Groups.subscribe(function (newValue) {
			//	alert("The person's new name is " + newValue);
			//});

			self.addGroup = function () {
				var group = new groupViewModel({ "Name": "New Group", "Agents": [] });
				self.Groups.push(group);
			};

			self.selectedTask = ko.observable();
			self.clearTask = function (data, event) {
				if (data === self.selectedTask()) {
					self.selectedTask(null);
				}
			};

			self.isTaskSelected = function (task) {
				return task === self.selectedTask();
			};

			var successCallback = function (message) { };
			var errorCallback = function (message) { };

			/**
				Save the agent configuration
			*/
			self.SaveConfiguration = function () {
				$.ajax({
					url: self.GetViewModelSaveUrl(),
					type: "POST",
					contentType: "application/json; charset=utf-8",
					data: function () {
						var jsonData = ko.toJSON(self);
						return jsonData;
					}(),
					success: function () {
						successCallback("Group configuration has been saved successfully.");
					},
					error: function () {
						errorCallback("Cannot save group configuration to server.");
					}
				});
			};

			/**
				Apply external configuration values
			*/
			(function (configuration) {
				if (!configuration) {
					return;
				}

				/**
					Get the ViewModel save URL.
					@name GetViewModelSaveUrl
				*/
				self.GetViewModelSaveUrl = function () {
					return configuration.ViewModelSaveUrl;
				};

				/**
					Get the ViewModel load URL.
					@name GetViewModelLoadUrl
				*/
				self.GetViewModelLoadUrl = function () {
					return configuration.ViewModelLoadUrl;
				};

				/**
					The callback function that is executed when the view model has been loaded
					@name ModelReadyCallback
				*/
				self.ModelReadyCallback = function () {
					if (typeof (configuration.ModelReadyCallback) === "function") {
						try {
							configuration.ModelReadyCallback();
						} catch (callbackError) {
							console.log("Error while executing the model ready callback function. {0}".format(callbackError));
						}

					}
				};

				if (configuration.SuccessCallback && typeof (configuration.SuccessCallback) === 'function') {
					successCallback = function (message) {
						configuration.SuccessCallback(message);
					};
				}

				if (configuration.ErrorCallback && typeof (configuration.ErrorCallback) === 'function') {
					errorCallback = function (message) {
						configuration.ErrorCallback(message);
					};
				}
			})(viewModelConfiguration);

			/**
				Initialize the view model and apply the Knockout bindings
			*/
			(function () {
				$.ajax({
					url: self.GetViewModelLoadUrl(),
					type: "GET",
					contentType: "application/json; charset=utf-8",
					success: function (groupConfiguration) {
						if (!groupConfiguration) {
							errorCallback("Cannot load empty group configuration.");
							return;
						}

						self.UnassignedAgents = ko.observableArray(groupConfiguration.UnassignedAgents);

						for (var i = 0; i < groupConfiguration.Groups.length; i++) {
							var groupConfig = groupConfiguration.Groups[i];
							self.Groups.push(new groupViewModel(groupConfig));
						}

						ko.applyBindings(self);
						successCallback("Group configuration loaded.");

						self.ModelReadyCallback();
					},
					error: function () {
						errorCallback("Cannot retrieve agent configuration from server.");
					}
				});
			})();
		}

		var viewModel = new groupConfigurationViewModel(moduleConfiguration);
		return viewModel;

	})(uiConfigurationOptions)
});