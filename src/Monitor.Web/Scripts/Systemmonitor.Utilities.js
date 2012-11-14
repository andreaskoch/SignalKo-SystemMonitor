if (typeof (jQuery) === 'undefined') {
	throw new Error("jQuery is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
	throw new Error("SystemMonitor is required to run this component");
}

$.extend(SystemMonitor, {
	"Utilities": {}
});