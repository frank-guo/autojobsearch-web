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
        if ($("#title").val() != "" && !isExist($("#title").val())) {
            $("#titleConds").append($('<option>', {
                value: titleNo,
                text: $("#title").val()
            }));
            titleNo++;
        }
    });

    function isExist(title) {
        if (title == null) {
            return false;
        }

        var exist = false;
        $('#titleConds option').each(function () {
            var condition = $(this).text();
            if (condition.match(new RegExp(title, "i"))) {
                exist = true;
                return false;
            }
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