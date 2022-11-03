$(function () {
    var postUrl = "/Roles/GetDataForRolesJqGrid";
    var gridCaption = "Roles List";

    var grid = $("#tblRoles");
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
        colNames: ["Id", "Name", "Accounts"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";                   
                    return cellPrefix + "<a href=\"/Roles/Details/" + rowObject.Id + "\">" + cellvalue + "</a>";
                }
            },
            {
                name: "Accounts", width: "35px", align: "center", search: false
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
        sortname: "Name",
        sortorder: "asc"
    });
    
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
