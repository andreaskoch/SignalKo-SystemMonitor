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
		var newTopMargin = headerHeight + currentTopMargin;
		$(layout.content).css("margin-top", newTopMargin);
		$(".agentAnchor").css("top", newTopMargin * -1);
		
		/* set bottom margin */
		var footerHeight = $(layout.footer).height();
		var currentBottomMargin = parseInt($(layout.content).css("margin-bottom"));
		var newBottomMargin = footerHeight + currentBottomMargin;
		$(layout.content).css("margin-bottom", newBottomMargin);

		return layout;
	})()
});