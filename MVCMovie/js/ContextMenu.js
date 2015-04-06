﻿var doc1 = $("#myframe")[0].contenDocument || $("#myframe")[0].contentWindow.document;
var targetE;
var node1, node2, job1Link, parent1, parent2, child, next;
var nodeListOfJob1 = [];
var levelNoLinkHigherJob1 = 0;
var company, others;
var count = 0;
$(doc1).bind("contextmenu", function (e) {
    e.preventDefault();// To prevent the default context menu.
    //getBoundingClientRect() also works and  will return an object containing the coordinates of the element top-left and bottom-right
    //Then offset top-left could be got by object.left and object.top 
    targetE = e.target;
    var offsetLeft = $("#myframe")[0].offsetLeft;
    var offsetTop = $("#myframe")[0].offsetTop;
    var absLeft = e.pageX + offsetLeft;
    //IE gets scrollTop from documentElement, ie<html> when DOCTYPE is set, or from body when DOCTYPE not set
    //Other browser use window.pageYOffset
    var scrollTop = $("#myframe")[0].contentWindow.pageYOffset || doc1.documentElement.scrollTop || doc1.body.scrollTop
    var absTop = e.pageY + offsetTop - scrollTop;
    $("#cntnr").css("left", absLeft);   // For updating the menu position.
    $("#cntnr").css("top", absTop);    // 
    $("#cntnr").fadeIn(500, startFocusOut()); //  For bringing the context menu in picture.
});
function startFocusOut() {
    $(document).on("click", function () {
        $("#cntnr").hide(500);              // To hide the context menu
        $(document).off("click");
    });
}
$("#itemNode1").click(function (e) {
    //$("#op").text("You have selected " + $(targetE).prop('outerHTML'));
    $("#node1").text($(targetE).prop('outerHTML'));
    $("#job1link").text($(targetE).prop('outerHTML'));
    node1 = targetE;
    job1Link = node1;
    levelNoLinkHigherJob1 = 0;
});

$("#itemNode2").click(function (e) {
    $("#node2").text($(targetE).prop('outerHTML'));
    node2 = targetE;
});

$("#itemCompany").click(function (e) {
    $("#company").text($(targetE).prop('outerHTML'));
    company = targetE;

    //Caculate the path for the company node
    var listPositions = getNodePath(company);

    //Highlight the company node with green using the caculated path
    hightLightNode(listPositions, 'green');

    //Send listPositions to the website to store
    $.ajax({
        type: "POST",
        url: "/Browser/SetCompany",
        data: JSON.stringify(listPositions),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            alert("send listPositions successfully!");
        }
    });

    //Highlight the company node, the path of which is got from the website, with red
    var listPostions = [];
    $.ajax({
        type: "GET",
        url: "/Browser/GetCompany",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (lp) {
            listPostions = lp;
        }
    });

    hightLightNode(listPositions, 'red');
});

$("#itemOthers").click(function (e) {
    $("#others").text($(targetE).prop('outerHTML'));
    others = targetE;

    //Caculate the path for the others node
    var listPositions = getNodePath(others);

    //Highlight the company node with green using the caculated path
    hightLightNode(listPositions, 'green');

    //Send listPositions to the website to store
    $.ajax({
        type: "POST",
        url: "/Browser/SetOthers",
        data: JSON.stringify(listPositions),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            alert("send listPositions successfully!");
        }
    });

    //Highlight the company node, the path of which is got from the website, with red
    var listPostions = [];
    $.ajax({
        type: "GET",
        url: "/Browser/GetOthers",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (lp) {
            listPostions = lp;
        }
    });

    hightLightNode(listPositions, 'red');
});

$("#itemNext").click(function (e) {
    $("#nextPage").text($(targetE).prop('outerHTML'));
    next = targetE;

    //Caculate the path for the next node
    var listPostions = getNodePath(next);

    //Highlight the next nodes with color green
    hightLightNode(listPostions, 'green');
    

    //Send listPositions to the website to store
    $.ajax({
        type: "POST",
        url: "/Browser/SetNext",
        data: JSON.stringify(listPostions),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            alert("send listPositions successfully!");
        }
    });

    //Highlight the next nodes, the path of which is got from the website, with red
    var listPositions = [];
    $.ajax({
        type: "GET",
        url: "/Browser/GetNext",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (lp) {
            listPostions = lp;
        }
    });

    parent1 = doc1;

    //Get to the common ancestor
    for (i = listPostions.length - 1; i >= 0 ; i--) {
        next = $(parent1).children().eq(listPostions[i])
        parent1 = next;
    }

    $(next).css('background-color', 'red');

});

$("#goUp").click(function (e) {
    if (job1Link != null) {
        nodeListOfJob1.push(job1Link);
        job1Link = $(job1Link).parent();
        $("#job1link").text($(job1Link).prop('outerHTML'));

        levelNoLinkHigherJob1++;

        var levelNo = { "levelNoLinkHigherJob1": levelNoLinkHigherJob1 };
        var levelNoJson = JSON.stringify(levelNo);

        $.ajax({
            type: "POST",
            url: "/Browser/SetLevelNo",
            data: levelNoJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (json) {
                alert("send listPositions successfully!");
            }
        });

    }
});

$("#goDown").click(function (e) {
    if (nodeListOfJob1.length != 0) {
        job1Link = nodeListOfJob1.pop();
        $("#job1link").text($(job1Link).prop('outerHTML'));

        levelNoLinkHigherJob1--;

        var levelNo = { "levelNoLinkHigherJob1": levelNoLinkHigherJob1 };
        var levelNoJson = JSON.stringify(levelNo);

        $.ajax({
            type: "POST",
            url: "/Browser/SetLevelNo",
            data: levelNoJson,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (json) {
                alert("send listPositions successfully!");
            }
        });

    }
});

$("#itemHLight").click(function (e) {
    //In general, the function first caculate the job1's path with the mark of the common parent of job1 and job2
    //Next, use the path to find all job nodes to highlight
    //At last, it putshes the path, i.e.listOfNodes, to the website for store. 

    var listOfNodes = [];

    parent1 = $(node1).parent();
    parent2 = $(node2).parent();

    //First get to the common parent
    while ($(parent1)[0] != $(parent2)[0]) {
        if (parent1 == null || parent2 == null) {
            alert("Can not determin all nodes!");
            return false;
        }

        children = $(parent1).children();
        $(children).each(function () {
            if (this == $(node1)[0]) {
                return false;
            }
            count++;
        });

        pathNode = new Object();
        pathNode.position = count;
        pathNode.hasCommonParent = false;
        listOfNodes.push(pathNode);

        //$('#op').append("<p></p>");
        //$('#op p').last().text(pathNode.hasCommonParent.toLocaleString() + "   " + pathNode.position.toString() + $(node1).prop('tagName'));
        count = 0;
        node1 = parent1;
        node2 = parent2;
        parent1 = $(node1).parent();
        parent2 = $(node2).parent();
    }

    //All the way get to the root from the common parent
    while ($(parent1)[0] != null) {
        children = $(parent1).children();
        $(children).each(function () {
            if (this == $(node1)[0]) {
                return false;
            }
            count++;
        });

        pathNode = new Object();
        pathNode.position = count;
        if ($(parent1)[0] != $(parent2)[0]) {
            pathNode.hasCommonParent = false;
        }
        else {
            pathNode.hasCommonParent = true;
        }

        listOfNodes.push(pathNode);

        //$('#op').append("<p></p>");
        //$('#op p').last().text(pathNode.hasCommonParent.toLocaleString() + "   " + pathNode.position.toString() + "    " + $(node1).prop('tagName'));

        //Go up one level

        count = 0;
        node1 = parent1;
        parent1 = $(node1).parent();
    }

    //$('#op').append("<br>");

    //Highlight all the job title nodes
    var i;
    parent1 = doc1;

    //Get to the common ancestor
    for (i = listOfNodes.length-1; i>=0 ; i--) {
        if (!listOfNodes[i].hasCommonParent) {
            node1 = $(parent1).children().eq(listOfNodes[i].position);
            parent1 = node1;
        }
        else {
            break;
        }
    }

    //$('#op').append("<br>");
    //parent1 is currently at th level of the common ancestor
    //i is currently at the level one lower than the common ancestor
    //Get all the job title nodes
    startI = --i;
    children = $(parent1).children();
    $(children).each(function () {
        node1 = $(this);
        for (; i >= 0; i--) {
            //$('#op').append("<p></p>");
            //$('#op p').last().text(listOfNodes[i].hasCommonParent.toLocaleString() + "   "
            //    + listOfNodes[i].position.toString() + "    " + $(node1).prop('tagName'));

            node1 = $(node1).children().eq(listOfNodes[i].position);
        }
        i = startI;
        $(node1).css('background-color', 'green');
    });

    //Send ListPathNodes to the website to store
    $.ajax({
        type: "POST",
        url: "/Browser/SetJobs",
        data: JSON.stringify(listOfNodes),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            alert("send listOfNodes successfully!");
        }
    });
});

function getNodePath(node) {
    parent1 = $(node).parent();
    count = 0;
    var listPostions = [];

    //All the way get to the root
    while ($(parent1)[0] != null) {
        children = $(parent1).children();
        $(children).each(function () {
            if (this == $(node)[0]) {
                return false;
            }
            count++;
        });

        listPostions.push(count);

        //Go up one level
        count = 0;
        node = parent1;
        parent1 = $(node).parent();
    }

    return listPostions;
}

function hightLightNode(nodePath, color) {
    //Highlight the node with color green
    parent1 = doc1;
    var node;

    for (i = nodePath.length - 1; i >= 0 ; i--) {
        node = $(parent1).children().eq(nodePath[i]);
        parent1 = node;
    }

    $(node).css('background-color', color);
}