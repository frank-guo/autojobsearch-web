

$(document).ready(function () {
    $('#setting').show();
    /*
    var data = [
    { text: "Black", value: "1" },
    { text: "Orange", value: "2" },
    { text: "Grey", value: "3" }
    ];

    // create DropDownList from input HTML element
    $("#frequent").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: data,
        index: 0,
        change: onChange
    });

    var frequent = $("#frequent").data("kendoDropDownList");

    function onChange() {
        var value = $("#frequent").val();
    };
    */

    $(".dropdown-menu li a").click(function () {
        var frequency = $(this).text();
        $("#frequency").val(frequency);
    });
});