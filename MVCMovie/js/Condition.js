var titleNo = 1;
var locationNo = 1;


$(document).ready(function () {

    //Initialize conditions
    //Get and set title conditions

    var dataId = $('#setCondition').attr('data-id');
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
            for (i = 0; i < condition.titleConds.length; i++) {
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

    $('#addTitle').click(function (e) {
        if ($("#title").val() != "") {
            $("#titleConds").append($('<option>', {
                value: titleNo,
                text: $("#title").val()
            }));
            titleNo++;
        }
    });

    $('#removTitle').click(function (e) {
        if ($('#titleConds :selected').length) {
            $('#titleConds :selected').remove();
            $('#title').text('');
        }
    });

    $('#titleConds').change(function () {
        $('#title').text($('#titleConds :selected').text());
    });

    $('#addLocation').click(function (e) {
        if ($("#location").val() != "") {
            $("#locationConds").append($('<option>', {
                value: locationNo,
                text: $("#location").val()
            }));
            locationNo++;
        }
    });

    $('#removLocation').click(function (e) {
        if ($('#locationConds :selected').length) {
            $('#locationConds :selected').remove();
            $('#location').text('');
        }
    });

    $('#locationConds').change(function () {
        $('#location').text($('#locationConds :selected').text());
    });


    $('#setCond').click(function () {
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

        //siteId = $(this).attr('data-id');
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
            success: function (msg) {
                alert(msg);
            }
        });
    });
})