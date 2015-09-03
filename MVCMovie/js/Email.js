

$(document).ready(function () {
    $('#setting').show();

    $("#frequencyDropdown li a").click(function () {
        var frequency = $(this).text();
        $("#frequency").val(frequency);
        $("#frequency").change();
    });

    var Email = function () {
        this.ID = $("#siteId").html();
        this.address = ko.observable();
        this.password = ko.observable();
        this.frequencyText = ko.observable("Daily");
        this.sendingOn = ko.observable();
        this.smtpAddress = ko.observable();
        this.smtpPort = ko.observable();
        this.sendingTime = ko.observable("00:05");

        this.frequency = ko.computed(function () {
            var freq = 0;
            switch (this.frequencyText()) {
                case "Daily":
                    freq = 1;
                    break;
                case "Every Other Day":
                    freq = 2;
                    break;
                case "Weekly":
                    freq = 3;
                    break;
            }
            return freq;
        }, this);
    }


    var email = new Email();
    $.ajax({
        type: "GET",
        url: "/Email/GetEmail/" + $("#siteId").html(),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (retEmail) {
            if (retEmail == null) {
                return;
            }
            
            email.address(retEmail.address);
            email.password(retEmail.password);
            switch (retEmail.frequency) {
                case 1:
                    email.frequencyText("Daily");
                    break;
                case 2:
                    email.frequencyText("Every Other Day ");
                    break;
                case 3:
                    email.frequencyText("Weekly");
                    break;
            }
            email.sendingOn(retEmail.sendingOn);
            email.smtpAddress(retEmail.smtpAddress);
            email.smtpPort(retEmail.smtpPort);

            return;
        }
    });
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
            success: function (msg) {
                switch (msg.msgCode) {
                    case 0:
                        $('#alertMsg').attr("class", "alert alert-success");
                        $('#alertMsg').text(msg.message);
                        break;
                    case 1:
                        $('#alertMsg').attr("class", "alert alert-danger");
                        $('#alertMsg').text(msg.message);
                        break;
                    case 2:
                        $('#alertMsg').attr("class", "alert alert-danger");
                        $('#alertMsg').text(msg.message);
                        break;
                }

                $('#alertMsg').fadeIn(1000);
                $('#alertMsg').fadeOut(3000);
            }
        });
    }
});