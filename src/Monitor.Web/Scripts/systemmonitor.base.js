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
    self.DataSeries = ko.observableArray();

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
        }
    });

    $.connection.hub.start(); 
   
}

ko.applyBindings(new MachineStatesViewModel());