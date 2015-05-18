var titleNo = 1;

$(document).ready(function () {
    $('#addTitle').click(function (e) {
        if ($("#title").val() != null) {
            $("#titles").append($('<option>', {
                value: titleNo,
                text: $("#title").val()
            }));
            titleNo++;
        }
    });
})