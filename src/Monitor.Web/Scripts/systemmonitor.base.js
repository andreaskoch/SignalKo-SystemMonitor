
function StorageStatusViewModel(storageStatus) {
    this.StorageDeviceInfos = storageStatus.StorageDeviceInfos;
}

function MemoryStatusViewModel(memoryStatus) {
    this.AvailableMemoryInGB = memoryStatus.AvailableMemoryInGB;
    this.UsedMemoryInGB = memoryStatus.UsedMemoryInGB;
}

function ProcessorStatusViewModel(processorStatus) {
    this.ProcessorUtilizationInPercent = processorStatus.ProcessorUtilizationInPercent;
}

function SystemInformationItemViewModel(systemInformation) {
    this.TimeStamp = systemInformation.Timestamp;
    this.MachineName = systemInformation.MachineName;
    this.ProcessorStatus = new ProcessorStatusViewModel(systemInformation.ProcessorStatus);
    this.MemoryStatus = new MemoryStatusViewModel(systemInformation.MemoryStatus);
    this.StorageStatus = new StorageStatusViewModel(systemInformation.StorageStatus);
}

var systemInformationBuffer = ko.observableArray();

function SystemInformationViewModel() {
    this.systemInformationItems = systemInformationBuffer;
}

var hub = $.connection.systemInformationHub;

$.extend(hub, {
    displaySystemInformation: function (systemInformation) {
        var vm = new SystemInformationItemViewModel(systemInformation);
        systemInformationBuffer.push(vm);
    }
});

$.connection.hub.start();

ko.applyBindings(new SystemInformationViewModel());