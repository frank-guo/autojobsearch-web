
var cssLink = document.createElement("link")
cssLink.href = "/js/ContextMenu.css";
cssLink.rel = "stylesheet";
cssLink.type = "text/css";
$('head',document).append(cssLink);


var titleNo = 1;
var locationNo = 1;

$(document).ready(function () {
    //var d = $("#myframe")[0].contentDocument; // contentWindow works in IE7 and FF
    checkIframeLoaded();

    $('#startEmail').click(function () {
        //Send credential to the website to store
        var email = { "address": $("#email").val(), "password": $('#password').val() };
        var credential = JSON.stringify(email);

        $.ajax({
            type: "POST",
            url: "/Email/TurnOnsend",
            data: credential,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                alert(msg);
            }
        });
    });

    $('#containLinks').click(function () {
        //show the text box to help find job1's link
        if (this.checked) {
            $("#locateJob1Link").removeAttr("disabled");

            //Set isContainJobLink
            $.ajax({
                type: "POST",
                url: "/Browser/SetIsContainJobLink",
                data: JSON.stringify({isContainJobLink:true}),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    alert("send listOfNodes successfully!");
                }
            });
        }

        if (!this.checked) {
            $("#locateJob1Link").attr("disabled", "disabled")

            //Reset isContainJobLink
            $.ajax({
                type: "POST",
                url: "/Browser/SetIsContainJobLink",
                data: JSON.stringify({isContainJobLink:false}),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    alert("send listOfNodes successfully!");
                }
            });
        }
    });


    $('#addTitle').click(function () {
        AddOneTitleCondInput();
    });

    $('#addLocation').click(function () {
        AddOneLocationCondInput();
    });

    $('#removTitle').click(function () {
        if (titleNo == 1) {
            return;
        }

        RemoveOneTitleCondInput();
    });

    $('#removeLocation').click(function () {
        if (locationNo == 1) {
            return;
        }

        RemoveOneLocationCondInput();
    });

    $('#setCond').click(function () {
        var titleConds = [];
        var locationConds = [];

        for (var i = 1; i <= titleNo; i++) {
            id = "#inputTitle" + i;
            var condition = $(id).val();
            if(condition!=null && condition!=""){
                titleConds.push(condition);
            }
        }

        for (var i = 1; i <= locationNo; i++) {
            id = "#location" + i;
            var condition = $(id).val();
            if(condition!=null && condition!=""){
                locationConds.push(condition);
            }
        }

        siteId = $(this).attr('data-id');
        var data = JSON.stringify({
            ID: siteId,
            titleConds: titleConds,
            locationConds: locationConds
        });

        $.ajax({
            type: "POST",
            url: "/Browser/SetCondition",
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                alert(msg);
            }
        });
    });
});

function checkIframeLoaded() {
    var d = window.frames["myframename"].document;
    //d.open(); d.close(); // must open and close document object to start using it!

    // now start doing normal jQuery:
    if (d.readyState == 'complete') {
        $("#demo").html($(d).contents().find("title").html());
        var scriptTag = document.createElement('script');
        scriptTag.type = 'text/javascript';
        scriptTag.src = "/js/ContextMenu.js";
        $("head", d).append(scriptTag);

        //Get the values for all the input textarea from the website and set their values
        //First set job1 and job2
        var test = $("#myframe");
        var doc1 = $("#myframe")[0].contenDocument || $("#myframe")[0].contentWindow.document;
        var listOfNodes = [];
        var pathNode = new Object();
        var listPostions = [];

        var siteId = { "siteId": $('#selectPanel').attr('data-id') };
        var siteIdJson = JSON.stringify(siteId);

        $.ajax({
            type: "POST",
            url: "/Browser/GetJobs",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (lp) {
                listOfNodes = lp;

                if ( listOfNodes!=null ) {
                    //Get the job1 node
                    var i;
                    var parent1 = doc1;
                    var node1;

                    //Get to the job1
                    for (i = listOfNodes.length-1; i>=0 ; i--) {
                        node1 = $(parent1).children().eq(listOfNodes[i].position);
                        parent1 = node1;
                    }

                    $("#node1").text($(node1).prop('outerHTML'));

                    //Get the link elment of job1
                    $.ajax({
                        type: "POST",
                        url: "/Browser/GetLevelNo",
                        data: siteIdJson,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (result) {
                            if(result!=null){
                                var levelNo = result;
                            }

                            var linkOfJob1 = node1;
                            for(i = 0; i<levelNo;i++){
                                linkOfJob1 = $(linkOfJob1).parent();
                                //In case i is too large, linkOfJob1 has reached at HTML element
                                //and parent() will return null 
                                if($(linkOfJob1).prop("tagName") == "HTML"){
                                    break;
                                }
                            }
                            $("#job1link").text($(linkOfJob1).prop('outerHTML'));
                        }

                    });
                }
            }
        });

        //Get and set Job2 input area
        $.ajax({
            type: "POST",
            url: "/Browser/GetJob2",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (lp) {
                listOfNodes = lp;
                var node1 = getNode(lp);
                $("#node2").text($(node1).prop('outerHTML'));
            }
        });

        //Get and set Next input area
        $.ajax({
            type: "POST",
            url: "/Browser/GetNext",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (lp) {
                listOfNodes = lp;
                var node = getNode(lp);
                $("#nextPage").text($(node).prop('outerHTML'));
            }
        });

        //Get and set Company input area
        $.ajax({
            type: "POST",
            url: "/Browser/GetCompany",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (lp) {
                listOfNodes = lp;
                var node = getNode(lp);
                $("#company").text($(node).prop('outerHTML'));
            }
        });

        //Get and set Company input area
        $.ajax({
            type: "POST",
            url: "/Browser/GetOthers",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (lp) {
                listOfNodes = lp;
                var node = getNode(lp);
                $("#others").text($(node).prop('outerHTML'));
            }
        });

        //Get and set the Check box for whether it contains job links
        $.ajax({
            type: "POST",
            url: "/Browser/GetIsContainJobLink",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (retValue) {
                var isContainJobLink = retValue;
                if (isContainJobLink){
                    $("#containLinks")[0].checked = true;
                    $("#locateJob1Link").removeAttr("disabled");
                }
                else{
                    $("#containLinks")[0].checked = false;
                    $("#locateJob1Link").attr("disabled", "disabled")
                }
            }
        });

        //Get and set title conditions
        $.ajax({
            type: "POST",
            url: "/Browser/GetCondition",
            data: siteIdJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (condition) {
                if(condition==null){
                    return;
                }

                //Set titleConds
                for(i=0;i<condition.titleConds.length;i++){
                    //Set titleCond with titleNo
                    titleId = "#inputTitle" + titleNo;
                    var title = condition.titleConds[i].titleCond;
                    $(titleId).val(title);

                    if(i==condition.titleConds.length - 1){
                        break;
                    }

                    AddOneTitleCondInput();
                }

                //Set locationConds
                for(i=0;i<condition.locationConds.length;i++){
                    //Set locationConds with locationNo
                    locationId = "#location" + locationNo;
                    var location = condition.locationConds[i].locationCond;
                    $(locationId).val(location);

                    if(i==condition.locationConds.length - 1){
                        break;
                    }

                    AddOneLocationCondInput();
                }
            }
        });

        //Get and set 

        return;
    }

    window.setTimeout('checkIframeLoaded();', 100);

}

function RemoveOneTitleCondInput() {
    var divId = "#inputTitleDiv" + titleNo;
    $(divId).remove();

    titleNo--;
    var lastOrId = "#titleOr" + titleNo;
    $(lastOrId).hide();

}

function RemoveOneLocationCondInput() {
    var divId = "#locationDiv" + locationNo;
    $(divId).remove();

    locationNo--;
    var lastOrId = "#locationOr" + locationNo;
    $(lastOrId).hide();
}

function AddOneLocationCondInput(){
    var lastOrId = "#locationOr" + locationNo;
    locationNo++;

    var div1 = '<div class="col-md-3 myrow btmAlgn" id="locationDiv' + locationNo + '">';
    var locationDiv = '<div class="col-md-8 left0"><input type="text" class="form-control top5" id="location'
        + locationNo + '" placeholder="City Name" /></div>';
    var orDiv = '<div class="col-md-4" id="locationOr' + locationNo + '" style="display:none">OR</div>';
    var endDiv1 = '</div>';
    var locationDiv = div1 + locationDiv + orDiv + endDiv1;
    $(lastOrId).show();
    $("#locationRow").append(locationDiv);
}

function AddOneTitleCondInput() {
    var lastOrId = "#titleOr" + titleNo;
    titleNo++;
    var div1 = '<div class="col-md-3 myrow btmAlgn" id="inputTitleDiv' + titleNo + '">';
    var titleDiv = '<div class="col-md-8 left0"><input type="text" class="form-control top5" id="inputTitle'
        + titleNo + '" placeholder="Keyword" /></div>';
    var orDiv = '<div class="col-md-4" id="titleOr' + titleNo + '" style="display:none">OR</div>';
    var endDiv1 = '</div>';
    var condDiv = div1 + titleDiv + orDiv + endDiv1;
    $(lastOrId).show();
    $("#condRow").append(condDiv);
}

function getNode(listOfNodes){
    var i;
    var parent1 = $("#myframe")[0].contenDocument || $("#myframe")[0].contentWindow.document;
    var node1;

    if ( listOfNodes!=null ) {
        for (i = listOfNodes.length-1; i>=0 ; i--) {
            node1 = $(parent1).children().eq(listOfNodes[i]);
            parent1 = node1;
        }
    }

    return node1;
}

