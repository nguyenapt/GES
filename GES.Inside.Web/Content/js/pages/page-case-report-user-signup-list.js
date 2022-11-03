$(function () {
    var postUrl = "/CaseProfile/GetDataForSignedUpCaseProfileUserListJqGrid?id=";
    var gridCaption = "";

    if (caseReportId != null && caseReportId > 0) {
        postUrl = postUrl + caseReportId;
    }

    var grid = $("#tblCaseReportUserSignupList");
    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        colNames: ["Id", "User", "Email", "Organization", "Active/Passive"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            { name: "UserName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Email", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "OrganizationName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "SignUpValue", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } }
        ],
        pager: $("#myPager"),
        rowNum: 50,
        rowList: [20, 50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        //loadonce: true,
        rownumbers: false,
        //pagerpos: "left",
        gridview: true,
        //width: "auto",
        height: "auto",
        viewrecords: true,
        caption: gridCaption,
        sortname: "companyname",
        sortorder: "asc"
    });
    
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
