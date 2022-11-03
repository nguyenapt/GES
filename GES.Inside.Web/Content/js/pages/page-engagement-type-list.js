$(function () {
    var postUrl = "/EngagementType/GetDataForEngagementTypesJqGrid";
    var gridCaption = "Engagement Types";


    var grid = $("#tblEngagementTypes");
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
        postData: { },
        mtype: "post",
        colNames: ["EngagementTypes Id", "Catalog", "Name", "Contact", "Status","Created"],
        colModel: [
            { name: "I_EngagementTypes_Id", width: "35px", align: "right", hidden: true, search: false },
            { name: "CatalogName", width: "70px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            {
                name: "Name", searchoptions: {
                    searchOperators: true
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/EngagementType/EngagementTypeDetails/" + rowObject.I_EngagementTypes_Id + "\">" + cellvalue + "</a>";
                }
            },
            { name: "ContactFullName", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            {
                name: "Deactive", searchoptions: {
                    searchOperators: true
                },
                formatter: function (cellvalue, options, rowObject) {

                    if (rowObject.Deactive === null || rowObject.Deactive  === false) {
                        return "";
                    } else {
                        return "Closed";
                    }
                }
            },
            { name: "Created", width: "45px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } },
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
