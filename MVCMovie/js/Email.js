


$(document).ready(function () {
    $('#setting').show();

    $(".dropdown-menu li a").click(function () {
        var frequency = $(this).text();
        $("#frequency").val(frequency);
    });

    var Email = function () {
        this.ID = $("#siteId").html();
        this.address = ko.observable("");
        this.password = ko.observable("");
        this.frequency = ko.observable("Daily");
        this.sendingOn = ko.observable(false);
    }

    var email = new Email();
    ko.applyBindings(email, document.getElementById("emailForm"));

    $('#emailForm').submit(function (event) {
        setEmail();
        event.preventDefault();
    });

    function setEmail() {
        var emailJson = ko.toJSON(email);

        $.ajax({
            type: "POST",
            url: "/Email/SaveEmail",
            data: emailJson,
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
});