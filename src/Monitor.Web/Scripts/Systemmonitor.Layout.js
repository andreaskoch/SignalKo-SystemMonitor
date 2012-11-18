if (typeof (jQuery) === 'undefined') {
	throw new Error("jQuery is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
	throw new Error("SystemMonitor is required to run this component");
}

$.extend(SystemMonitor, {

	"Layout": (function () {

		var layout = {
			"header": $("body>header:first"),
			"content": $("body>.content:first"),
			"footer": $("body>footer:first"),
		};
		
		/* set top margin */
		var headerHeight = $(layout.header).height();
		var currentTopMargin = parseInt($(layout.content).css("margin-top"));
		$(layout.content).css("margin-top", headerHeight + currentTopMargin);
		
		/* set bottom margin */
		var footerHeight = $(layout.footer).height();
		var currentBottomMargin = parseInt($(layout.content).css("margin-top"));
		$(layout.content).css("margin-bottom", footerHeight + currentBottomMargin);

		return layout;
	})()
});