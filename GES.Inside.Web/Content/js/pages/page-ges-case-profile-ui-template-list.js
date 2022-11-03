$(function () {
    var postUrl = "/CaseProfileUIConfig/GetDataForCaseProfileUIsJqGrid";
    var gridCaption = "Case Profile templates";


    var grid = $("#tblGesCaseProfileUiTemplate");
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
        colNames: ["GesCaseProfileTemplatesId", "Name", "Description", "Engagement Type","Recomendation"],
        colModel: [
            { name: "GesCaseProfileTemplates_Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "TemplateName", searchoptions: {
                    searchOperators: true
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/CaseProfileUIConfig/Details/" + rowObject.GesCaseProfileTemplatesId + "\">" + cellvalue + "</a>";
                }
            },
            { name: "TemplateDescription", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "EngagementType", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Recomendation", width: "200px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } }
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
