var configViewModel;

var MachineGroupingModel = function () {
    var self = this;
    self.machineGroups = ko.observableArray([]);
    self.allMonitorMachines = ko.observableArray([]);
    self.selectedMachine = ko.observable();
    self.selectMachine = function (machine) {
        this.selectedMachine(machine);
    };
    self.addMachine = function () {
        var machine = new Machine(self.newMachine());
        self.availableMashines.push(machine);
        self.newMachine('');
    };

    self.availableMashines = ko.observableArray([]);
    self.deleteAvailableMachine = function () {

        self.availableMashines.remove(this);
    };
    self.deleteMachineGroup = function () {
        var machineGroupToDelete = $(this)[0];
        ko.utils.arrayForEach(machineGroupToDelete.monitorMachines(), function (machine) {
            self.availableMashines.push(machine);
        });
        self.machineGroups.remove(machineGroupToDelete);
    };
    self.trash = ko.observableArray([]);

    self.addGroup = function () {
        var group = new MachineGroup(self.newGroup());
        self.machineGroups.push(group);
        self.newGroup('');
    };
    self.newMachine = ko.observable([]);
    self.newGroup = ko.observable('');

    self.saveConfiguration = function () {
        $.post(window.ConfigurationUrls.SaveConfig,
            { "configuration": ko.toJSON(configViewModel) },
            function () {
                toastr.success("Configuration successfuly saved");  
            }).error(function (xmlHttpRequest, textStatus, errorThrown) {
                toastr.error(errorThrown, "Error");
            })
        ;
    };
    self.loadConfigViewModel = function () {
        $.getJSON(window.ConfigurationUrls.LoadConfig,
            function (data) {
                if (data === '') {
                    ko.applyBindings(self, document.getElementById('configBody'));
                } else {
                    var model = ko.mapping.fromJSON(data);
                    for (var i = 0; i < model.availableMashines().length; i++) {
                        var m = new Machine();
                        m.machineName(model.availableMashines()[i].machineName());
                        self.availableMashines.push(m);
                        self.allMonitorMachines.push(m);
                    }
                    for (var j = 0; j < model.machineGroups().length; j++) {
                        var jsGroup = model.machineGroups()[j];
                        var mGroup = new MachineGroup();
                        for (var k = 0; k < jsGroup.monitorMachines().length; k++) {
                            var machine = jsGroup.monitorMachines()[k];
                            var ma = new Machine();
                            ma.machineName(machine.machineName());
                            ma.monitoringUrl(machine.monitoringUrl());
                            ma.IsWebserver(machine.IsWebserver());
                            mGroup.monitorMachines.push(ma);
                            self.allMonitorMachines.push(ma);
                        }
                        mGroup.groupName(jsGroup.groupName());
                        self.machineGroups.push(mGroup);
                    }
                }
                self.loadConfigViewModelComplete();
            });
        };
        self.handleFileDropIn = function (evt) {
            if (!evt.dataTransfer.files) {
                return;
            }
            evt.stopPropagation();
            evt.preventDefault();
            var files = evt.dataTransfer.files; // FileList object
            // Loop through the FileList and render image files as thumbnails.
            for (var i = 0, f; f = files[i]; i++) {
                var reader = new FileReader();
                // Closure to capture the file information.
                reader.onload = (function (theFile) {
                    return function (e) {
                        var model = ko.mapping.fromJSON(e.target.result);
                        self.availableMashines.push(model);
                        self.allMonitorMachines.push(ma);
                    };
                })(f);
                // Read in the image file as a data URL.
                reader.readAsText(f, "UTF-8");
            }
        };
        self.loadConfigViewModelComplete = function () { };
        self.iterateAutomatic = function () {
            if (!self.autoInterval()) {
                self.refreshIntervalId = setInterval(cycle, 3000);
                self.autoInterval(true);
            } else {
                clearInterval(self.refreshIntervalId);
                self.autoInterval(false);
                self.refreshIntervalId = 0;
            }
        };
        self.openMachineConfigOverlay = function () {
            var position = $("#" + this.machineName()).offset();
            $(".box-overlay").css({ position: "absolute", left: position.left, top: position.top });
            $(".box-overlay").show();
            ko.applyBindings(this, $('.box-overlay')[0]);
        };
        self.openGroupConfigOverlay = function () {
            var position = $("#" + this.groupName()).offset();
            $(".box-overlay-group").css({ position: "absolute", left: position.left, top: position.top });
            $(".box-overlay-group").show();
            ko.applyBindings(this, $('.box-overlay-group')[0]);
        };
        self.selectedGroupIndex = 0;
        self.refreshIntervalId = 0;
        self.autoInterval = ko.observable(false);
};

var MachineGroup = function (groupName) {
    var self = this;
    self.groupName = ko.observable(groupName);
    self.monitorMachines = ko.observableArray([]);
    self.canDelete = false;
};

var Machine = function (name) {
    var self = this;
    self.machineName = ko.observable(name);
    self.monitoringUrl = ko.observable('');
    self.IsWebserver = ko.observable(true);
};

$(".closeConfig").click(function () {
    $(this).parent().parent().hide();
    ko.cleanNode($(this).parent().parent());
});

function cycle() {
    var tablink = $('a[href^="#tabs-"]');
    // simulate click on current tab
    $('a[href^="#tabs-"]').eq(configViewModel.selectedGroupIndex).click();
    // increment counter   
    configViewModel.selectedGroupIndex++;
    // reset if we're at the last one
    if (configViewModel.selectedGroupIndex == configViewModel.machineGroups().length) {
        configViewModel.selectedGroupIndex = 0;
    }
}
//connect items with observableArrays
ko.bindingHandlers.sortableList = {
    init: function (element, valueAccessor, allBindingsAccessor, context) {
        $(element).data("sortList", valueAccessor()); //attach meta-data
        $(element).sortable({
            update: function (event, ui) {
                var item = ko.dataFor(ui.item[0]);
                //var item = ui.item.data("sortItem");
                if (item) {
                    //identify parents
                    //var originalParent = ui.item.data("parentList");
                    var originalParent = ui.item.data("parentList");
                    var newParent = ui.item.parent().data("sortList");
                    //figure out its new position
                    var position = ko.utils.arrayIndexOf(ui.item.parent().children(), ui.item[0]);
                    if (position >= 0) {
                        originalParent.remove(item);
                        newParent.splice(position, 0, item);
                    }

                    ui.item.remove();
                }
            },
            connectWith: '.container'
        });
    }
};

ko.bindingHandlers.sortableMachineList = {
    init: function (element, valueAccessor, allBindingsAccessor, context) {
        $(element).data("sortList", valueAccessor()); //attach meta-data
        $(element).sortable({
            update: function (event, ui) {
                var item = ko.dataFor(ui.item[0]);
                //var item = ui.item.data("sortItem");
                if (item) {
                    //identify parents
                    //var originalParent = ui.item.data("parentList");
                    var originalParent = ui.item.data("parentList");
                    var newParent = ui.item.parent().data("sortList");
                    //figure out its new position
                    var position = ko.utils.arrayIndexOf(ui.item.parent().children(), ui.item[0]);
                    if (position >= 0) {
                        originalParent.remove(item);
                        newParent.splice(position, 0, item);
                    }

                    ui.item.remove();
                }
            },
            connectWith: '.machineGroup'
        });
    }
};

//attach meta-data
ko.bindingHandlers.sortableItem = {
    init: function (element, valueAccessor) {
        var options = valueAccessor();
        $(element).data("sortItem", options.item);
        $(element).data("parentList", options.parentList);
    }
};

//control visibility, give element focus, and select the contents (in order)
ko.bindingHandlers.visibleAndSelect = {
    update: function (element, valueAccessor) {
        ko.bindingHandlers.visible.update(element, valueAccessor);
        if (valueAccessor()) {
            setTimeout(function () {
                $(element).focus().select();
            }, 0); //new tasks are not in DOM yet
        }
    }
};  
