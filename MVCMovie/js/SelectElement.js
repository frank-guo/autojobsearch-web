
var cssLink = document.createElement("link")
cssLink.href = "/js/ContextMenu.css";
cssLink.rel = "stylesheet";
cssLink.type = "text/css";
$('head',document).append(cssLink);

$(document).ready(function () {
    $('#setting').show();

    //var d = $("#myframe")[0].contentDocument; // contentWindow works in IE7 and FF

    $('#containLinks').click(function () {
        //show the text box to help find job1's link
        if (this.checked) {
            $("#goUp").removeAttr("disabled");
            $("#goDown").removeAttr("disabled");

            //Set isContainJobLink
            $.ajax({
                type: "POST",
                url: "/Browser/SetIsContainJobLink/" + siteId,
                data: JSON.stringify({isContainJobLink:true}),
                contentType: "application/json; charset=utf-8",
                success: function () {
                    //alert("send listOfNodes successfully!");
                }
            });
        }

        if (!this.checked) {
            $("#goUp").attr("disabled", "disabled")
            $("#goDown").attr("disabled", "disabled")

            //Reset isContainJobLink
            $.ajax({
                type: "POST",
                url: "/Browser/SetIsContainJobLink/" + siteId,
                data: JSON.stringify({isContainJobLink:false}),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    alert("send listOfNodes successfully!");
                }
            });
        }
    });

    $('#clearAll').click(function () {
        //siteId is coming from knockout setting in SettingMenu.js
        var siteIdObj = { "siteId": siteId };
        $.ajax({
            url: "/Browser/DeleteAllJobSetting",
            data: JSON.stringify(siteIdObj),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function () {
                $('#node1').val(null)
                $('#node2').val(null)
                $('#nextPage').val(null)
                $('#company').val(null)
                $('#others').val(null)
                $('#clearAll').val(null)
                $('#containLinks').prop('checked', false);
                $('#job1link').val(null)

            }
        });
    })
});

var id = window.setInterval(checkIframeLoaded, 100);

function checkIframeLoaded() {
    var d = window.frames["myframename"].document;

    // now start doing normal jQuery:
    var innerHead = $("head", d).html();
    //readyState and load event work in IE but not in Chrome.
    //Therefore, add two more empty check for head element to determine if the iframe is loaded
    if (d.readyState == 'complete' && innerHead != null && innerHead !== "") {
        $("#demo").html($(d).contents().find("title").html());
        var scriptTag = document.createElement('script');
        scriptTag.type = 'text/javascript';
        scriptTag.src = "/js/ContextMenu.js";
        //$("head", d).append(scriptTag);
        var myframe = $("#myframe")
        var contents = myframe.contents();
        var head = $("head", myframe.contents());
        head.append(scriptTag);

        //Get the values for all the input textarea from the website and set their values
        //First set job1 and job2
        var test = $("#myframe");
        var doc1 = $("#myframe")[0].contenDocument || $("#myframe")[0].contentWindow.document;
        var listOfNodes = [];
        var pathNode = new Object();
        var listPostions = [];

        var siteIdObj = { "siteId": siteId };
        var siteIdJson = JSON.stringify(siteIdObj);

        $.ajax({
            type: "POST",
            url: "/Browser/GetJob1",
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

        //Get and set others input area
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
                    $("#findJobLink").removeAttr("disabled");
                }
                else{
                    $("#containLinks")[0].checked = false;
                    $("#findJobLink").attr("disabled", "disabled")
                }
            }
        });

        window.clearInterval(id);
        return;
    }
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

