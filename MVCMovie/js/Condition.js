var titleNo = 1;
var locationNo = 1;


$(document).ready(function () {
    //Add set list in navigation bar
    var dataId = $('#setCondition').attr('data-id');
    $('#setting').show();
    $("#conditionLi").attr("href", "/Condition/Index/" + dataId);

    //Initialize conditions
    //Get and set title conditions
    var siteId = { "siteId": dataId };
    var siteIdJson = JSON.stringify(siteId);

    $.ajax({
        type: "POST",
        url: "/Condition/GetCondition",
        data: siteIdJson,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (condition) {
            if (condition == null) {
                return;
            }

            //Intialize titleConds
            for (var i = 0; i < condition.titleConds.length; i++) {
                //Set titleCond with  the value of titleNo
                $("#titleConds").append($('<option>', {
                    value: titleNo,
                    text: condition.titleConds[i].titleCond
                }));
                titleNo++;
            }

            //Intialize locationConds
            for (i = 0; i < condition.locationConds.length; i++) {
                //Set locationCond with  the value of locationNo
                $("#locationConds").append($('<option>', {
                    value: locationNo,
                    text: condition.locationConds[i].locationCond
                }));
                locationNo++;
            }
        }
    });

    $('#setCondForm').submit(function (event) {
        setCond();
        event.preventDefault();
    });

    $('#addTitle').click(function (e) {
        e.preventDefault();
        if ($("#title").val() == "") {
            return;
        }
        if (!isExist($("#title").val(), "titleConds")) {
            $("#titleConds").append($('<option>', {
                value: titleNo,
                text: $("#title").val().capFirstLetter()
            }));
            titleNo++;
        }
        else {
            $('#titleInputAlert').attr("class", "alert alert-warning");
            $('#titleInputAlert').text("Duplicate condition!");
            $('#titleInputAlert').fadeIn(1000);
            $('#titleInputAlert').fadeOut(3000);
        }
    });

    String.prototype.capFirstLetter = function() {
        return this.charAt(0).toUpperCase() + this.substr(1);
    }

    function isExist(input, selectBox) {
        if (input == null) {
            return false;
        }

        var exist = false;
        $('#' + selectBox +' option').each(function () {
            var condition = $(this).text();
            if (condition.match(new RegExp(input, "i")) && condition.length == input.length) {
                exist = true;
                return false;
            }
            return true;
        });

        return exist;
    }

    $('#removTitle').click(function (e) {
        if ($('#titleConds :selected').length) {
            $('#titleConds :selected').remove();
            $('#title').text('');
        }
    });

    $('#titleConds').change(function () {
        $('#title').val($('#titleConds :selected').text());
    });

    $('#addLocation').click(function (e) {
        if ($("#location").val() == "") {
            return;
        }

        if (!isExist($("#location").val(), "locationConds")) {
            $("#locationConds").append($('<option>', {
                value: locationNo,
                text: $("#location").val().capFirstLetter()
            }));
            locationNo++;
        } else {
            $('#locationInputAlert').attr("class", "alert alert-warning");
            $('#locationInputAlert').text("Duplicate condition!");
            $('#locationInputAlert').fadeIn(1000);
            $('#locationInputAlert').fadeOut(3000);
        }
    });

    $('#removLocation').click(function (e) {
        if ($('#locationConds :selected').length) {
            $('#locationConds :selected').remove();
            $('#location').text('');
        }
    });

    $('#locationConds').change(function () {
        $('#location').val($('#locationConds :selected').text());
    });

    /*
    //Prevent submit event being triggerred.
    $('[name="timeRange"]').click(function(event) {
        this.click();
    });
    */
    


    function setCond() {
        var titleConds = [];
        var locationConds = [];

        $('#titleConds option').each( function(){
            var condition = $(this).text();
            if(condition!=null && condition!=""){
                titleConds.push(condition);
            }
        });

        $('#locationConds option').each( function(){
            var condition = $(this).text();
            if(condition!=null && condition!=""){
                locationConds.push(condition);
            }
        });

        var data = JSON.stringify({
            ID: dataId,
            titleConds: titleConds,
            locationConds: locationConds
        });

        $.ajax({
            type: "POST",
            url: "/Condition/SetCondition",
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (error) {
                switch (error.ErrorCode) {
                    case 0:
                        $('#alertMsg').attr("class", "alert alert-success");
                        $('#alertMsg').text(error.ErrorMsg);
                        break;
                    case -1:
                        $('#alertMsg').attr("class", "alert alert-danger");
                        $('#alertMsg').text(error.ErrorMsg);
                        break;
                    case -2:
                        $('#alertMsg').attr("class", "alert alert-danger");
                        $('#alertMsg').text(error.ErrorMsg);
                        break;
                }
                
                $('#alertMsg').fadeIn(1000);
                $('#alertMsg').fadeOut(3000);
            }
        });
    }
})