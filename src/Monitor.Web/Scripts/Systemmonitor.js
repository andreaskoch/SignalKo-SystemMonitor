if (typeof (jQuery) === 'undefined') {
	throw new Error("jQuery is required to run this component");
}

var SystemMonitor = (function () {

	var SystemMonitor = function () {
		return new SystemMonitor.init();
	};

	return {
		constructor: SystemMonitor,

		init: function () {
			return this;
		}
	};

})();