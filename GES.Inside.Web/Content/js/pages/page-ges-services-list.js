$(function () {
    var postUrl = "/GesServices/GetDataForGesServicesJqGrid";
    var gridCaption = "GES Services";


    var grid = $("#tblGesServices");
    $.jgrid.defaults.responsive = true;
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        postData: { },
        mtype: "post",
        colNames: ["Services Id", "Name", "Url", "Sort","ReportLetter","Engagement Type Name"],
        colModel: [
            { name: "GServicesId", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name", searchoptions: {
                    searchOperators: true
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/GesServices/Details/" + rowObject.GServicesId + "\">" + cellvalue + "</a>";
                }
            },
            { name: "Url", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Sort", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "ReportLetter", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "EngagementTypesName", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } }
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
        sortname: "CatalogName",
        sortorder: "asc",
        cmTemplate: { sortable: true },
        loadError: function (jqXHR, textStatus, errorThrown) {
            alert("HTTP status code: " + jqXHR.status + "\n" +
                  "TextStatus: " + textStatus + "\n" +
                  "Error Message: " + errorThrown);
        }
    });
    setFilterDate.call(grid, "Created");
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });
    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });
});
