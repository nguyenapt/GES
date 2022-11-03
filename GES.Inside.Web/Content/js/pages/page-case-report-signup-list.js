$(function () {
    var postUrl = "/CaseProfile/GetDataForSignedUpCaseProfileListJqGrid";
    var gridCaption = "";

    var grid = $("#tblcasereportsignuplist");
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
        colNames: ["Company", "Case", "Id", "Organization", "Individual", "Email", "Endorsement"],
        colModel: [            
            { name: "CompanyName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "CaseName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }, minwidth: "300px" },
            {
                name: "Id",
                width: "50px",
                align: "right",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix +
                        "<a target='_blank' href=\"/CaseProfile/Edit/" +
                        rowObject.CaseProfileId +
                        "\">" +
                        rowObject.CaseProfileId +
                        "</a>";
                },
                searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge", "nu", "nn"] }
            },
            { name: "OrganizationName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "FullName", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }, width:"110px" },
            { name: "Email", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Endorsement", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] }, width:"70px", align:"center" }
            //{
            //    name: "CaseName", searchoptions: {
            //        searchOperators: true,
            //        sopt: ["cn", "ew", "en", "bw", "bn"]
            //    },
            //    formatter: function (cellvalue, options, rowObject) {
            //        var cellPrefix = "";
            //        return cellPrefix + "<a href=\"/CaseProfile/SignedUpUsers?id=" + rowObject.Id + "\">" + cellvalue + "</a>";
            //    }
            //},
            
            //{ name: "NumberOfSignUp", search:false }
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
    //setIndustriesSelect.call(grid, "Industry");
    //setStatusesSelect.call(grid, "Status");    //setProgressStatusesSelect.call(grid, "ProgressStatus");    //setCountriesSelect.call(grid, "Country");    //setFilterDate.call(grid, "Created");    //setFilterDate.call(grid, "Modified");    
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });


    $(".export-endorsements-btn").on("click", function (e) {
        window.location.href = "/CaseProfile/ExportEndorsements";
        return true;
    });

});
