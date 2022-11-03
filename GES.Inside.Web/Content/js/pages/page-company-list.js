$(function () {
    var postUrl = "/company/GetDataForCompaniesJqGrid";
    var gridCaption = "Company List";

    var getCountries = function () {
        return countries;
    }
    var setCountriesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getCountries.call(this)),
                sopt: ["eq"]
            }
        });
    };

    var grid = $("#tblcompanies");
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
        colNames: ["Id", "Name", "ISIN", "SEDOL", "Location", "Website", "Modified", "Number Of Cases"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] },
                formatter: function (cellvalue, options, rowObject) {
                    return rowObject.parent === null ? "<b><a class='cell-company-name' href=\"/Company/Details/" + rowObject.Id + "\">" + rowObject.Name + "</a></b>" : "<a class='cell-company-name' href=\"/Company/Details/" + rowObject.Id + "\">" + rowObject.Name + "</a>";
                }
            },
            { name: "Isin", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "Sedol", width: "100px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "Location", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Website", hidden: true, searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "Modified", hidden: true, width: "85px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } },
            {
                name: "NumberOfCases",
                width: "85px",
                search: false,
                sortable: false,
                align: "center",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    if (rowObject.IsMasterCompany === true) {
                        return cellPrefix + "<a href=\"/CaseProfile/List?companyId=" + rowObject.Id + "\">" + rowObject.NumberOfCases + " case profiles" + "</a>";
                    }

                    return "";
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

        sortname: "Id",
        sortorder: "asc",
        cmTemplate: { sortable: false },
        loadError: function (jqXHR, textStatus, errorThrown) {
            alert("HTTP status code: " + jqXHR.status + "\n" +
                  "TextStatus: " + textStatus + "\n" +
                  "Error Message: " + errorThrown);
        },
        ondblClickRow: function() {
            $('#btn-contact-select').click();
        }
    });
    setCountriesSelect.call(grid, "Location");
    setFilterDate.call(grid, "Modified");

    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
