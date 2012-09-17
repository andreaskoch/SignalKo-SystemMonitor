if (typeof (jQuery) === 'undefined') {
    throw new Error("jQuery is required to run this component");
}

if (typeof (ko) === 'undefined') {
    throw new Error("Knockout is required to run this component");
}

if (typeof (Highcharts) === 'undefined') {
    throw new Error("Highcharts is required to run this component");
}

if (typeof (SystemMonitor) === 'undefined') {
    throw new Error("SystemMonitor is required to run this component");
}

$.extend(SystemMonitor, {

    "Overview": (function () {

        function machineStateViewModel(machineName) {
            var self = this;
            self.MachineName = machineName;

            self.chart = null;
            self.ChartTitle = machineName;
            self.ChartContainerId = "chart-" + machineName;
            self.ChartContainer = ko.observable("<div id='" + self.ChartContainerId + "'></div>");

            self.ChartSizeMax = 60;
            self.CaptureStartTime = new Date();

            self.AddData = function(timestamp, dataPoints)
            {
                if (self.chart === null)
                {
                    initializeChart(dataPoints);
                }

                for (var i = 0; i < dataPoints.length; i++)
                {
                    var name = dataPoints[i].Name;
                    var value = dataPoints[i].Value;

                    var series = getOrAddSeries(name);

                    var secondsSinceMidnight = SystemMonitor.Utilities.getSecondsSinceMidnight(new Date(timestamp));
                    var secondsBetweenMidnightAndCaptureStart = SystemMonitor.Utilities.getSecondsSinceMidnight(self.CaptureStartTime);

                    var x = secondsSinceMidnight - secondsBetweenMidnightAndCaptureStart;
                    var y = parseFloat(value);

                    var shiftChart = !(series.data.length < self.ChartSizeMax);
                    var redrawChart = false;

                    if (x >= 0 && x <= 86400)
                    {
                        series.addPoint([x, y], redrawChart, shiftChart);
                    }
                }

                self.chart.redraw();
            };

            var initializeChart = function(initialDataPoints)
            {
                var getInitialSeries = function(dataSeries) {
                    var initialSeries = [];

                    for(var i = 0; i < dataSeries.length; i++)
                    {
                        var seriesName = dataSeries[i].Name;
                        var value = dataSeries[i].Value
                        var seriesData = [value];

                        initialSeries.push({ "name": seriesName, "data": seriesData });
                    }

                    return initialSeries;
                };

                var getAbsoluteTimestampFromRelativeChartPosition = function (chartPosition) {
                    var secondsSinceCaptureStart = chartPosition;
                    var millisecondsSinceCaputureStart = secondsSinceCaptureStart * 1000;
                    var millisecondsAtCaptureStart = self.CaptureStartTime.getTime();

                    var timestamp = new Date(millisecondsAtCaptureStart + millisecondsSinceCaputureStart);
                    return timestamp;
                };

                self.chart = new Highcharts.Chart({
                    chart: {
                        renderTo: self.ChartContainerId,
                        type: 'line',
                        zoomType: 'x',
                        marginRight: 250,
                        marginBottom: 25
                    },
                    title: {
                        text: self.ChartTitle,
                        x: -20 //center
                    },
                    xAxis: {
                        type: 'linear',
                        labels: {
                            formatter: function () {
                                var timestamp = getAbsoluteTimestampFromRelativeChartPosition(this.value);
                                return SystemMonitor.Utilities.getFormattedTime(timestamp);
                            }
                        }
                    },
                    yAxis: {
                        title: {
                            text: '%'
                        },
                        min: 0,
                        max: 100
                    },
                    tooltip: {
                        formatter: function () {
                            var timestamp = getAbsoluteTimestampFromRelativeChartPosition(this.x);
                            var value = Highcharts.numberFormat(this.y, 2);

                            return '<b>' + this.series.name + '</b><br/>' +
                            SystemMonitor.Utilities.getFormattedTime(timestamp) + '<br/>' +
                            value;
                        }
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'top',
                        x: -10,
                        y: 100,
                        borderWidth: 0
                    },
                    plotOptions: {
                        line: {
                            lineWidth: 2,
                            marker: {
                                enabled: false,
                                states: {
                                    hover: {
                                        enabled: false
                                    }
                                }
                            },
                            shadow: false,
                            states: {
                                hover: {
                                    lineWidth: 4
                                }
                            }
                        }
                    },
                    series: getInitialSeries(initialDataPoints)
                });
            };

            var getOrAddSeries = function (seriesName) {
                var series = self.chart.series;

                for (var i = 0; i < series.length; i++) {
                    var entry = series[i];
                    if (entry.name === seriesName) {
                        return entry;
                    }
                }

                self.chart.addSeries({
                    "name": seriesName,
                    "data": []
                });
            };            
        }

        function machineStatesViewModel()
        {
            var self = this;
            self.machineStateViewModels = ko.observableArray();

            var getMachineStateViewModel = function (MachineName) {
                var machineStateViewModels = self.machineStateViewModels();

                for (var i = 0; i < machineStateViewModels.length; i++)
                {
                    var machineStateViewModel = machineStateViewModels[i];
                    if (machineStateViewModel.MachineName === MachineName)
                    {
                        return machineStateViewModel;
                    }
                }

                return null;
            };

            var systemInformationHub = $.connection.systemInformationHub;
            systemInformationHub.displaySystemStatus = function(systemStatus)
            {
                var machineStateModel = getMachineStateViewModel(systemStatus.MachineName);

                if (machineStateModel === null)
                {
                    machineStateModel = new machineStateViewModel(systemStatus.MachineName);
                    self.machineStateViewModels.push(machineStateModel);
                }

                machineStateModel.AddData(systemStatus.Timestamp, systemStatus.DataPoints);
            };

            $.connection.hub.start();
        }

        var viewModel = new machineStatesViewModel();
        ko.applyBindings(viewModel);

        return viewModel;

    })()
});