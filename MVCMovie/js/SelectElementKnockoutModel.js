var doc1 = $("#myframe")[0].contenDocument || $("#myframe")[0].contentWindow.document;
var Job1 = function (pathNodes) {
    this.jobPath = ko.observable(pathNodes);

    this.text = ko.computed(function () {
        if (this.jobPath != null) {
            //Get the job1 node
            var i;
            var parent1 = doc1;
            var node1;

            //Get to the job1
            for (i = listOfNodes.length - 1; i >= 0 ; i--) {
                node1 = $(parent1).children().eq(listOfNodes[i].position);
                parent1 = node1;
            }
        }
        return $(node1).prop('outerHTML');
    }, this);

}
$.ajax({
    type: "POST",
    url: "/Browser/GetJobs",
    data: siteIdJson,
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (jobPath) {
        var job1 = new Job1(jobPath);
        ko.applyBindings(job1, document.getElementById('node1'));
    }
})