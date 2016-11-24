
$(document).ready(function () {
    var openSite = function () {
        var siteId = jobHuntingSite.existingSites()[jobHuntingSite.selectedSiteIdx()].ID
        if (siteId == null) {
            return
        }
        $.ajax({
            url: '/SeletElement/Index/' + siteId,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                // get the result and do some magic with it
                //SelectElment will return a view which contains the iframe that will request Browser/Index/siteId.
                //Browser/Index/siteId acutally returns the content of job searching sites.
                //So the content inside the iframe will be thought of as safety
                var w = window.open();
                w.document.open(data);
                w.document.write(data);
                w.document.close();
            }
        });
    }

    var createSite = function () {
        var site = {"siteName": $('#siteName').val(), "url": $('#url').val() };
        var jsonData = JSON.stringify(site);

        $.ajax({
            url: '/Home/CreateSite',
            data: jsonData,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function () {
                getSites();
            }

        });
    }

    var upateSite = function () {
        var site = {
            "siteName": $('#siteName').val(), "url": $('#url').val()
        };
        var jsonData = JSON.stringify(site);

        $.ajax({
            url: '/Home/UpdateSite/' + jobHuntingSite.existingSites()[jobHuntingSite.selectedSiteIdx()].ID,
            data: jsonData,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function () {
                getSites();
            }

        });
    }

    var getSites = function () {
        $.ajax({
            type: "GET",
            url: "/Home/GetSites/",
            cache: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (existingSites) {
                if (existingSites == null) {
                    return;
                }

                jobHuntingSite.existingSites(existingSites);
                return;
            }
        });
    }

    $('#sitesForm').submit(function (event) {
        event.preventDefault();
    });

    $('#openSite').click(function () {
        $('#sitesForm').validate()
        if ($('#sitesForm').valid()) {
            openSite();
        }
    })

    var JobHuntingSite = function () {
        this.existingSites = ko.observableArray([]);
        this.disableOpenSite = ko.observable(false);
        this.selectedSiteIdx = ko.observable(0);
        this.selectedSiteName = ko.computed(function () {
            var selecteIdx = this.selectedSiteIdx()
            var selectedSite = this.existingSites()[this.selectedSiteIdx()];
            return selectedSite == null ? null : selectedSite.siteName;
        }, this);
        this.selectedSiteUrl = ko.computed(function () {
            var selectedSite = this.existingSites()[this.selectedSiteIdx()];
            return selectedSite == null ? null : selectedSite.url;
        }, this);
        this.dropdownOptions = ko.computed(function () {
            var options = []
            this.existingSites().map(function (site, index) {
                var siteName = site.siteName;
                var option = {
                    id: site.ID,
                    index: index,
                    text: siteName != null && siteName !== "" ? siteName + ": " + site.url : site.url
                }
                options.push(option);
            })
            return options
        }, this);
    }

    var jobHuntingSite = new JobHuntingSite();
    getSites();
    ko.applyBindings(jobHuntingSite, document.getElementById("sitesForm"));

    $('#website').change(function () {
        //$('#url').text($('#website :selected').text());
        ////can not set the siteId to the value of textarea becasue the value of textarea is actually changing the its text()
        //$('#url').data('siteId', $(this).children(':selected').val());
        var siteIdx = $(this).children(':selected').val()
        if (siteIdx != null) {
            jobHuntingSite.selectedSiteIdx(parseInt(siteIdx))
        }
        jobHuntingSite.disableOpenSite(false)
    });

    $('#website').click(function () {
        //jobHuntingSite.selectedSiteIdx.valueHasMutated()
        var siteIdx = $(this).children(':selected').val()
        if (siteIdx != null) {
            var url = jobHuntingSite.selectedSiteUrl()
            $('#url').val(url)
            $('#siteName').val(jobHuntingSite.selectedSiteName());
        }
        
    })

    $('#addSite').click(function () {
        $('#sitesForm').validate()
        if ($('#sitesForm').valid()) {
            createSite()
            jobHuntingSite.disableOpenSite(false)
        }
    })

    $('#updateSite').click(function () {
        $('#sitesForm').validate()
        if ($('#sitesForm').valid()) {
            upateSite()
            jobHuntingSite.disableOpenSite(false)
        }
    })

    $("#siteName").on('change keyup paste', function () {
        jobHuntingSite.disableOpenSite(true)
    })

    $("#url").on('change keyup paste', function () {
        jobHuntingSite.disableOpenSite(true)
    })

    $("#url").bind('input propertychange', function () {
        if (this.value === "") {
            jobHuntingSite.disableOpenSite(true)
        }
    });

    $("#deleteSite").click(function () {
        var selectedSiteIdx = jobHuntingSite.selectedSiteIdx();
        if (selectedSiteIdx == null) {
            return
        }
        var url = '/Home/DeleteSite/' + jobHuntingSite.existingSites()[selectedSiteIdx].ID;

        $.ajax({
            url: url,
            contentType: 'application/json; charset=utf-8',
            success: function () {
                //Update website text area
                getSites();
                $('#website option[value="0"]').attr('selected', 'selected')
                jobHuntingSite.selectedSiteIdx(0)
            }
        });
        jobHuntingSite.disableOpenSite(false)
    });

});