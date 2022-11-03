$(function () {
    var postUrl = "/caseprofile/GetDataForCasesJqGrid";
    var gridCaption = "Case profiles";

    var compId = companyId;

    var grid = $("#tblCases");
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
        postData: { companyId: compId },
        mtype: "post",
        colNames: ["Issue", "Id", "Company name", "Location", "Norm/Theme", "Engagement status", "Entry date"],
        colModel: [            
            {
                name: "IssueName",
                formatter: function(cellvalue, options, rowObject) {
                    var cellPrefix = "";
                        return cellPrefix +
                            "<a href=\"/CaseProfile/Edit/" +
                            rowObject.Id +
                            "\">" +
                            rowObject.IssueName +
                            "</a>";
                },
                searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }
            },
            {
                name: "Id",
                width: "35px",
                align: "right",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix +
                        "<a target='_blank' href=\"/CaseProfile/Edit/" +
                        rowObject.Id +
                        "\">" +
                        rowObject.Id +
                        "</a>";
                },
                searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge", "nu", "nn"] }
            },
            { name: "CompanyName", width: "50px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }},
            { name: "Location", width: "50px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }},
            { name: "EngagementThemeNorm", width: "65px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }},
            { name: "Recommendation", width: "50px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "EntryDate", width: "45px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } },
        ],
        pager: $("#myPager"),
        rowNum: 20,
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
        sortname: "IssueName",
        sortorder: "asc",
        cmTemplate: { sortable: true },
        loadError: function (jqXHR, textStatus, errorThrown) {
            alert("HTTP status code: " + jqXHR.status + "\n" +
                  "TextStatus: " + textStatus + "\n" +
                  "Error Message: " + errorThrown);
        }
    });
    setFilterDate.call(grid, "EntryDate");
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });
    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });
});
