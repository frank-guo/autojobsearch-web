
$(document).ready(function () {
    function openBrower() {
        var siteId = $('#url').data('siteId');

        if (siteId !== 0) {
            openSite(siteId);
        }
        else {
            if ($('#url').val() !== "") {
                createSite();
            }
        }
    }

    var openSite = function (siteId) {
        var idUrl = { "id": siteId, "url": $("#url").val() };

        var json = JSON.stringify(idUrl);

        $.ajax({
            url: '/Browser/SetURL',
            data: json,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (id) {
                // get the result and do some magic with it
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
        });
    }

    var createSite = function () {
        var url = { "url": $('#url').val() };
        var jsonData = JSON.stringify(url);

        $.ajax({
            url: '/Home/CreateSite',
            data: jsonData,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (sites) {
                //Update website text area
                $('#website').empty();
                $(sites.sites).each(function () {
                    $('#website').append($('<option>', {
                        value: this.ID,
                        text: this.url
                    }));
                });

                $("#url").data('siteId', sites.newID);

                openSite(sites.newID);
            }

        });
    }

    $('#website').change(function () {
        $('#url').text($('#website :selected').text());
        //can not set the siteId to the value of textarea becasue the value of textarea is actually changing the its text()
        $('#url').data('siteId', $(this).children(':selected').val());
    });

    $('#url').click(function () {
        $('#website').val(null);
        $(this).data('siteId', 0);
    });

    $('#sitesForm').submit(function (event) {
        event.preventDefault();
    });

    $("#openSite").click(function () {
        openBrower();
    })

    $("#deleteSite").click(function () {
        var url = '/Home/DeleteSite/' + $('#url').data('siteId');
        $.ajax({
            url: url,
            contentType: 'application/json; charset=utf-8',
            success: function (sites) {
                //Update website text area
                $('#website').empty();
                $(sites).each(function () {
                    $('#website').append($('<option>', {
                        value: this.ID,
                        text: this.url
                    }));
                });

                //Empty url input box
                $("#url").data('siteId', 0);
                $("#url").val("");
                $('#website').val(null);

            }
        });
    });

    var RecomdSite = function () {
        this.siteName = ko.observable("");
        this.url = ko.computed(function () {
            var url = "";
            switch (this.siteName()) {
                case "T-Net":
                    url = "http://www.bctechnology.com/jobs/search-results.cfm";
                    break;
                case "Monster":
                    url = "http://jobsearch.monster.ca/jobs/?cy=ca";
                    break;
            }
            return url;
        }, this);
    }

    var recomdSite = new RecomdSite();
    ko.applyBindings(recomdSite, document.getElementById("sitesForm"));

    $("#recomdSite li a").click(function () {
        recomdSite.siteName("");
        recomdSite.siteName($(this).html());
        $("#url").data('siteId', 0);
    })

});