$(function () {
    var postUrl = "/client/GetDataForClientsJqGrid";
    var gridCaption = "Organization List";

    var getProgressStatuses = function () {
        return progressStatuses;
    }

    var getIndustries = function () {
        return industries;
    }

    var getCountries = function() {
        return countries;
    }
    var getClientsStatuses = function () {
        return clientStatus;
    },
    setStatusesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getClientsStatuses.call(this)),
                sopt: ["eq"]
            }
        });
    },
    setProgressStatusesSelect = function(columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getProgressStatuses.call(this)),
                sopt: ["eq"]
            }
        });
    },
    setIndustriesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getIndustries.call(this)),
                sopt: ["eq"]
            }
        });
    },
    setCountriesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getCountries.call(this)),
                sopt: ["eq"]
            }
        });
    };

    var grid = $("#tblclients");
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
        colNames: ["Id", "Name", "Industry", "Status", "ProgressStatus" , "Country", "Created", "Modified", "Employees", "TotalAssets" , "View"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";                   
                    return cellPrefix + "<a href=\"/Client/Details/" + rowObject.Id + "\">" + cellvalue + "</a>";
                }
            },
            { name: "Industry", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Status", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "ProgressStatus", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Country", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Created", width: "85px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt"] } },
            { name: "Modified", hidden: true, width: "85px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } },
            { name: "Employees", hidden: true, align: "right", width: "85px", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
            { name: "TotalAssets", hidden: true, align: "right", width: "100px", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
            {
                name: "View",
                width: "85px",
                search: false,
                sortable: false,
                align: "center",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/Portfolio/List?orgid=" + rowObject.Id + "\">" + rowObject.PortfoliosNumber + " portfolios" + "</a>";
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
        sortname: "Name",
        sortorder: "asc"
    });
    setIndustriesSelect.call(grid, "Industry");
    setStatusesSelect.call(grid, "Status");
    setProgressStatusesSelect.call(grid, "ProgressStatus");
    setCountriesSelect.call(grid, "Country");
    setFilterDate.call(grid, "Created");
    setFilterDate.call(grid, "Modified");
    
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
