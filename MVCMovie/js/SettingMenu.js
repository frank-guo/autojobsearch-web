﻿var SettingMenu = function (siteId) {
    $('[data-toggle="tooltip"]').tooltip();

    this.siteId = ko.observable(siteId);

    this.jobFeaturesUrl = ko.computed(function () {
        return "/SeletElement/Index/" + this.siteId();
    }, this);

    this.conditionUrl = ko.computed(function () {
        return "/Condition/Index/" + this.siteId();
    }, this);

    this.searchRuleUrl = ko.computed(function () {
        return "/angularapp/search-rule/" + this.siteId();
    }, this);

    this.emailUrl = ko.computed(function () {
        return "/Email/Index/" + this.siteId();
    }, this);
}

var settingMenu = new SettingMenu($("#siteId").html());
ko.applyBindings(settingMenu, document.getElementById('settingMenu'));
