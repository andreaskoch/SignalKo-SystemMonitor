function DataSeriesViewModel(Name)
{
    var self = this;
    self.Name = Name;
    self.Points = ko.observableArray();
    self.LastPoint = ko.computed(function() {
        var points = self.Points();
        if (points.length > 0) {
            var numberOfPoints = points.length;
            var indexOfLastPoint = numberOfPoints - 1;
            return points[indexOfLastPoint].Value;
        } else {
            return "";    
        }
    });

    self.AddPoint = function(Timestamp, Value) {
        self.Points.push({ "Timestamp": Timestamp, "Value": Value });
    };
}

function MachineStateViewModel(MachineName) {
    var self = this;
    self.MachineName = MachineName;
    self.ChartTitle = MachineName;
    self.ChartContainerId = "chart-" + MachineName;
    self.ChartContainer = ko.observable("<div id='" + self.ChartContainerId + "'></div>");
    self.DataSeries = ko.observableArray();

    self.Series = ko.computed(function() {
        var series = {};

        var dataSeries = self.DataSeries();
        for (var i = 0; i < dataSeries.length; i++) {
            var ds = dataSeries[i];

            var dataSeriesPoints = ds.Points();
            var values = [];
            for (var x = 0; dataSeriesPoints.length; i++)
            {
                values.push({ "x": dataSeriesPoints[x].Timestamp, "y": dataSeriesPoints[x].Value });
            }

            series[ds.Name] = values;
        }

        return series;
    });

    self.GetDataSeriesViewModel = function(Name) {
        var dataSeries = self.DataSeries();
        
        for (var i = 0; i < dataSeries.length; i++) {
            var ds = dataSeries[i];
            if (ds.Name === Name) {
                return ds;
            }
        }
        
        return null;
    };

    self.chart = null;
    self.InitializeChart = function() {
        self.chart = new Highcharts.Chart({
            chart: {
                renderTo: self.ChartContainerId,
                type: 'line',
                marginRight: 130,
                marginBottom: 25
            },

            title: {
                text: self.ChartTitle,
                x: -20 //center
            },
            xAxis: {
                categories: []
            },

            yAxis: {
                title: {
                    text: '%'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.series.name + '</b><br/>' + this.x + ': ' + this.y + '%';
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
            series: []
        });
    };

    self.AddDataSeries = function(Name, Timestamp, Value) {
        var ds = self.GetDataSeriesViewModel(Name);
        if (ds === null) {
            ds = new DataSeriesViewModel(Name);
            self.DataSeries.push(ds);
        }

        ds.AddPoint(Timestamp, Value);
    };
}

function MachineStatesViewModel() {
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
        displaySystemStatus: function(systemStatus)
        {
            var machineStateModel = self.GetMachineStateViewModel(systemStatus.MachineName);

            if (machineStateModel === null)
            {
                machineStateModel = new MachineStateViewModel(systemStatus.MachineName);
                self.machineStateViewModels.push(machineStateModel);
            }

            for (var i = 0; i < systemStatus.DataPoints.length; i++)
            {
                var dataPoint = systemStatus.DataPoints[i];
                machineStateModel.AddDataSeries(dataPoint.Name, systemStatus.Timestamp, dataPoint.Value);
            }

            machineStateModel.InitializeChart();
        }
    });

    $.connection.hub.start(); 
   
}

ko.applyBindings(new MachineStatesViewModel());