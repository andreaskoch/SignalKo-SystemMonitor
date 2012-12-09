if (typeof (jQuery) === 'undefined') {
	throw new Error("jQuery is required to run this component");
}

if (typeof (ko) === 'undefined') {
	throw new Error("Knockout is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
	throw new Error("SystemMonitor is required to run this component");
}

ko.bindingHandlers.sortableListOfAgents = {
	init: function (element, valueAccessor) {
		$(element).data("sortList", valueAccessor());
		$(element).sortable({
			update: function (event, ui) {
				var item = ko.dataFor(ui.item[0]);
				if (item) {
					var originalParent = ui.item.data("parentList");
					var newParent = ui.item.parent().data("sortList");
					
					var position = ko.utils.arrayIndexOf(ui.item.parent().children(), ui.item[0]);
					if (position >= 0) {
						originalParent.remove(item);
						newParent.splice(position, 0, item);
					}

					ui.item.remove();
				}
			},
			connectWith: '.agent-dropzone'
		});
	}
};

ko.bindingHandlers.sortableListOfAgentGroups = {
	init: function (element, valueAccessor) {
		$(element).data("sortList", valueAccessor());
		$(element).sortable({
			update: function (event, ui) {
				var item = ko.dataFor(ui.item[0]);
				
				if (item) {
					var originalParent = ui.item.data("parentList");
					var newParent = ui.item.parent().data("sortList");
					
					var position = ko.utils.arrayIndexOf(ui.item.parent().children(), ui.item[0]);
					if (position >= 0) {
						originalParent.remove(item);
						newParent.splice(position, 0, item);
					}

					ui.item.remove();
				}
			},
			connectWith: ''
		});
	}
};

ko.bindingHandlers.sortableItem = {
	init: function (element, valueAccessor) {
		var options = valueAccessor();
		$(element).data("sortItem", options.item);
		$(element).data("parentList", options.parentList);
	}
};

ko.bindingHandlers.visibleAndSelect = {
	update: function (element, valueAccessor) {
		ko.bindingHandlers.visible.update(element, valueAccessor);
		if (valueAccessor()) {
			setTimeout(function () {
				$(element).find("input").focus().select();
			}, 0);
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
			self.KnownAgents = ko.observableArray().extend({ unique: "No duplicates allowed" });

			self.KnownAgents.subscribe(function (newValue) {
				console.log("Before", self.KnownAgents());
				console.log("New value", newValue);
				console.log("After", self.KnownAgents());
			});

			self.addGroup = function () {
				var group = new groupViewModel({ "Name": "New Group", "Agents": [] });
				self.Groups.push(group);
			};

			self.removeGroup = function(group) {
				self.Groups.remove(group);
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
					url: self.GetApiUrl(),
					type: "POST",
					contentType: "application/json",
					dataType: "json",
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
					Get the api URL.
					@name GetApiUrl
				*/
				self.GetApiUrl = function () {
					return configuration.ApiUrl;
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
					url: self.GetApiUrl(),
					type: "GET",
					dataType: "json",
					success: function (model) {
						if (!model) {
							errorCallback("Cannot load empty group configuration.");
							return;
						}

						_.each(model.KnownAgents, function(agent) {
							self.KnownAgents.push(agent);
						});

						_.each(model.Groups, function (group) {
							self.Groups.push(new groupViewModel(group));
						});

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