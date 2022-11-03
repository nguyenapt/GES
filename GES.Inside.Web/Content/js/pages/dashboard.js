$(function () {
    var postUrl = "/Dashboard/GetUserCasesJqGrid";
    var gridCaption = "Cases";

    var grid = $("#tblUserCases");
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
        colNames: ["Id", "Company", "Issue", "Location", "Norm/Theme", "Recommendation"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "CompanyName", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/Company/Details/" + rowObject.CompanyId + "\">" + cellvalue + "</a>";
                }
            },
            {
                name: "IssueName", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/CaseProfile/Edit/" + rowObject.Id + "\">" + cellvalue + "</a>";
                }
            },
            {
                name: "Location", width: "65px", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                }
            },
            {
                name: "EngagementThemeNorm", width: "65px", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                }
            },
            {
                name: "Recommendation", width: "65px", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                }
            }
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
        sortname: "name",
        sortorder: "asc"
    });

    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
