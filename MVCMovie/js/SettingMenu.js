var SettingMenu = function (siteId) {
    this.siteId = ko.observable(siteId);

    this.jobFeaturesUrl = ko.computed(function () {
        return "/SeletElement/Index/" + this.siteId();
    }, this);

    this.conditionUrl = ko.computed(function () {
        return "/Condition/Index/" + this.siteId();
    }, this);

    this.emailUrl = ko.computed(function () {
        return "/Email/Index/" + this.siteId();
    }, this);
}

var settingMenu = new SettingMenu($("#siteId").html());
ko.applyBindings(settingMenu);
