var titleNo = 1;


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

            //Set titleConds
            for (i = 0; i < condition.titleConds.length; i++) {
                //Set titleCond with  the value of titleNo
                $("#titles").append($('<option>', {
                    value: titleNo,
                    text: condition.titleConds[i].titleCond
                }));
                titleNo++;
            }
        }
    });

    $('#addTitle').click(function (e) {
        if ($("#title").val() != "") {
            $("#titles").append($('<option>', {
                value: titleNo,
                text: $("#title").val()
            }));
            titleNo++;
        }
    });

    $('#removTitle').click(function (e) {
        if ($('#titles :selected').length) {
            $('#titles :selected').remove();
            $('#title').text('');
        }
    });

    $('#titles').change(function () {
        $('#title').text($('#titles :selected').text());
    });

    function addOneTitle(){
        if ($("#title").val() != null) {
            $("#titles").append($('<option>', {
                value: titleNo,
                text: $("#title").val()
            }));
            titleNo++;
        }
    }
})