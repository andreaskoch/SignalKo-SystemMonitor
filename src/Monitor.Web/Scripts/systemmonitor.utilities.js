if (typeof (jQuery) === 'undefined') {
    throw new Error("jQuery is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
    throw new Error("SystemMonitor is required to run this component");
}

$.extend(SystemMonitor, {
    "Utilities": {
        getMidnight: function () {
            var midnight = new Date();

            midnight.setHours(0);
            midnight.setMinutes(0);
            midnight.setSeconds(0);
            midnight.setMilliseconds(0);

            return midnight;
        },

        getSecondsSinceMidnight: function (datetime) {
            // get todays midnight
            var midnight = this.getMidnight();

            // cut off milliseconds
            datetime.setMilliseconds(0);

            // calculate difference
            var millisecondsTillMidnight = midnight.getTime();
            var millisecondsOfSuppliedDateTime = datetime.getTime();
            var millisecondsSinceMidnight = millisecondsOfSuppliedDateTime - millisecondsTillMidnight;

            // return seconds since midnight
            return millisecondsSinceMidnight / 1000;
        },

        getFormattedTime: function (dateTime) {
            var hours = dateTime.getHours();
            var minutes = dateTime.getMinutes();
            var seconds = dateTime.getSeconds();

            var formatTimeComponent = function (number) {
                if (number < 10) {
                    return "0" + number;
                }

                return number;
            };

            return formatTimeComponent(hours) + ":" + formatTimeComponent(minutes, 2) + ":" + formatTimeComponent(seconds);
        }
    }
});