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

    "Overview": {
        "start": function () {

            function machineStateViewModel(machineName) {
                var self = this;
                self.MachineName = machineName;

                self.ChartTitle = machineName;
                self.ChartContainerId = "chart-" + machineName;
                self.ChartContainer = ko.observable("<div id='" + self.ChartContainerId + "'></div>");

                self.ChartSizeCurrent = 0;
                self.ChartSizeMax = 60;
                self.CaptureStartTime = new Date();

                self.getAbsoluteTimestampFromRelativeChartPosition = function (chartPosition) {
                    var secondsSinceCaptureStart = chartPosition;
                    var millisecondsSinceCaputureStart = secondsSinceCaptureStart * 1000;
                    var millisecondsAtCaptureStart = self.CaptureStartTime.getTime();

                    var timestamp = new Date(millisecondsAtCaptureStart + millisecondsSinceCaputureStart);
                    return timestamp;
                };

                self.chart = null;
                var initializeChart = function () {
                    self.chart = new Highcharts.Chart({
                        chart: {
                            renderTo: self.ChartContainerId,
                            type: 'line',
                            zoomType: 'x',
                            marginRight: 130,
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
                                    var timestamp = self.getAbsoluteTimestampFromRelativeChartPosition(this.value);
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
                                var timestamp = self.getAbsoluteTimestampFromRelativeChartPosition(this.x);
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
                                lineWidth: 1,
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
                                        lineWidth: 1
                                    }
                                }
                            }
                        },
                        series: [{
                            name: "CPU Utilization in %",
                            data: [SystemMonitor.Utilities.getSecondsSinceMidnight(new Date())]
                        },
                        {
                            name: "Memory Utilization in %",
                            data: [SystemMonitor.Utilities.getSecondsSinceMidnight(new Date())]
                        }                        ]
                    });
                };

                self.GetOrAddSeries = function (seriesName) {
                    var series = self.chart.series;

                    for (var i = 0; i < series.length; i++) {
                        var entry = series[i];
                        if (entry.name === seriesName) {
                            return entry;
                        }
                    }

                    self.chart.addSeries({ "name": seriesName, "data": [] });
                };

                self.AddData = function (Name, Timestamp, Value) {
                    if (self.chart === null) {
                        initializeChart();
                    }

                    var series = self.GetOrAddSeries(Name);

                    var secondsSinceMidnight = SystemMonitor.Utilities.getSecondsSinceMidnight(new Date(Timestamp));
                    var secondsBetweenMidnightAndCaptureStart = SystemMonitor.Utilities.getSecondsSinceMidnight(self.CaptureStartTime);

                    var x = secondsSinceMidnight - secondsBetweenMidnightAndCaptureStart;
                    var y = parseFloat(Value);

                    var shift = !(self.ChartSizeCurrent < self.ChartSizeMax);

                    if (x >= 0 && x <= 86400) {
                        series.addPoint([x, y], false, shift);
                        self.ChartSizeCurrent++;
                    }
                };

                self.UpdateChart = function () {
                    self.chart.redraw();
                };
            }

            function machineStatesViewModel() {
                var self = this;
                self.machineStateViewModels = ko.observableArray();

                self.GetMachineStateViewModel = function (MachineName) {
                    var vms = self.machineStateViewModels();

                    for (var i = 0; i < vms.length; i++) {
                        var vm = vms[i];
                        if (vm.MachineName === MachineName) {
                            return vm;
                        }
                    }

                    return null;
                };

                var hub = $.connection.systemInformationHub;

                $.extend(hub, {
                    displaySystemStatus: function (systemStatus) {
                        var machineStateModel = self.GetMachineStateViewModel(systemStatus.MachineName);

                        if (machineStateModel === null) {
                            machineStateModel = new machineStateViewModel(systemStatus.MachineName);
                            self.machineStateViewModels.push(machineStateModel);
                        }

                        for (var i = 0; i < systemStatus.DataPoints.length; i++) {
                            var dataPoint = systemStatus.DataPoints[i];
                            machineStateModel.AddData(dataPoint.Name, systemStatus.Timestamp, dataPoint.Value);
                        }

                        machineStateModel.UpdateChart();
                    }
                });

                $.connection.hub.start();

            }

            ko.applyBindings(new machineStatesViewModel());
        }
    }
});

SystemMonitor.Overview.start();