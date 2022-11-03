var oldGrid = "";
$(function () {
    window.initializedJqgrid = false;

    var postData = {};

    if (typeof isOnlyForEngagementType !== 'undefined' && typeof engagementTypeId !== 'undefined' && isOnlyForEngagementType !== null && isOnlyForEngagementType === true) {
       // $(".sna-theme-page-img").css('background-image', 'url("/Content/img/engagement-type-' + engagementTypeId + '.jpg")');
        SearchCompany();
        // Events
        $("#ei-events").eventify({
            locale: "en"
        });
    } else {

        window.searchStr = $('#textbox-company-name').val().toLowerCase();
        window.onlyShowFocusList = $('#hiddenOnlyShowFocusList').val();

        utils.initMultiSelect("#combobox-portfolio", "All Portfolios/Indices");
        utils.initMultiSelect("#combobox-recommendation");
        //utils.initMultiSelect("#combobox-conclusion");
        utils.initMultiSelect("#combobox-location");
        utils.initMultiSelect("#combobox-response");
        utils.initMultiSelect("#combobox-progress");
        utils.initMultiSelect("#combobox-industry");
        utils.initMultiSelect("#combobox-homeCountries");

        postData = preparePostData();
        if (toBool(postData.onlyShowFocusList) === true // accessing focus list
            ||
            postData.name.trim() !== "" // might be: from: simple search

            // might be: from: dashboard
            ||
            postData.engagementAreaIds ||
            postData.portfolioIds ||
            postData.recommendationId ||
            postData.locationId ||
            postData.homeCountryIds ||
            postData.industryId
        ) {
            SearchCompany();
        }

    }

});

$(".btn-signup-type").on("click", function (e) {
    var self = $(this);
    if (self.hasClass("disabled")) {
        return;
    }

    var activelySignUp = $(this).hasClass("btn-signup-active");
    var data = {
        isSignUp: true,
        isActive: activelySignUp,
        gesCaseReportId: window.SignedUpGesCaseReportId
    }

    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Company/UpdateGesCaseReportSignUp",
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            if (response.meta.success) {
                var selector = ".btn-case-signup#GesCaseReportsId_" + window.SignedUpGesCaseReportId;
                $(selector).removeClass("signup-none");
                $(selector).removeClass("btn-default");
                if (activelySignUp) {
                    $(selector).addClass("signup-active");
                    $(selector).addClass("btn-success");
                    $(selector).text("Disclose");
                } else {
                    $(selector).addClass("signup-passive");
                    $(selector).addClass("btn-warning");
                    $(selector).text("Non-disclose");
                }
                utils.quickNotification("Endorsed successfully");
            } else {
                utils.quickNotification(response.meta.error, "error");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            // revert action
            //self.prop("checked", self.val());

            utils.quickNotification("Failed: Error occurred updating sign up", "error");
            //grid.trigger("reloadGrid");
        });
});

$(".btn-reset-search-company").on("click", function (e) {
    $('#textbox-company-name').val("");
    window.searchStr = "";
    $('#textbox-company-isin').val("");
    $('#combobox-portfolio').selectpicker('deselectAll');

    $('#combobox-recommendation').selectpicker('deselectAll');
    //$('#combobox-conclusion').selectpicker('deselectAll');
    $('#combobox-engagementarea').val("");
    $('#combobox-location').selectpicker('deselectAll');
    $('#combobox-response').selectpicker('deselectAll');
    $('#combobox-progress').selectpicker('deselectAll');
    $('#combobox-industry').selectpicker('deselectAll');
    $('#combobox-homeCountries').selectpicker('deselectAll');

    // reset checkbox values
    $('#checkbox-not-show-close-case').prop("checked", false);
    $('#checkbox-only-companies-with-active-case').prop("checked", false);

    SearchCompany();
});

$(".btn-search-company").on("click", function (e) {
    SearchCompany();
});

$("#btn-advanced-search").on("click", function (e) {
    var classList = $('#fa-advanced-search').prop('className').split(' ')

    if (classList.indexOf("fa-angle-double-down") > -1) {
        $('#fa-advanced-search').removeClass("fa-angle-double-down");
        $('#fa-advanced-search').addClass("fa-angle-double-up");
        $('#lbl-advanced-search').text("");
    } else {        
        $('#combobox-portfolio').selectpicker('deselectAll');
        $('#combobox-homeCountries').selectpicker('deselectAll');
        $('#combobox-progress').selectpicker('deselectAll');
        $('#combobox-industry').selectpicker('deselectAll');
        $('#combobox-location').selectpicker('deselectAll');
        $('#fa-advanced-search').removeClass("fa-angle-double-up");
        $('#fa-advanced-search').addClass("fa-angle-double-down");
        $('#lbl-advanced-search').text("Advanced search");
    }
});

function preparePostData() {
    $('#hiddenOnlySearchCompany').val(false);
    window.onlySearchCompanyName = false;

    window.isin = $('#textbox-company-isin').val();
    window.searchStr = $('#textbox-company-name').val().toLowerCase();
    if (window.engagementAreaIds != null && typeof window.engagementAreaIds !== "undefined") {
        if (window.engagementAreaIds.length > 0) {
            window.engagementAreaIds = Array();
        }
    }
    window.engagementAreaIds = $('#combobox-engagementarea').val();
    if (window.portfolioIds != null && typeof window.portfolioIds !== "undefined") {
        if (window.portfolioIds.length > 0) {
            window.portfolioIds = Array();
        }
    }
    window.portfolioIds = $('#combobox-portfolio').selectpicker('val');
    if (window.recommendationId != null && typeof window.recommendationId !== "undefined") {
        if (window.recommendationId.length > 0) {
            window.recommendationId = Array();
        }
    }
    window.recommendationId = $('#combobox-recommendation').selectpicker('val');
    if (window.conclusionId != null && typeof window.conclusionId !== "undefined") {
        if (window.conclusionId.length > 0) {
            window.conclusionId = Array();
        }
    }
    //window.conclusionId = $('#combobox-conclusion').selectpicker('val');
    //if (window.responseId != null && typeof window.responseId !== "undefined") {
    //    if (window.responseId.length > 0) {
    //        window.responseId = Array();
    //    }
    //}
    window.responseId = $('#combobox-response').selectpicker('val');
    if (window.progressId != null && typeof window.progressId !== "undefined") {
        if (window.progressId.length > 0) {
            window.progressId = Array();
        }
    }
    window.progressId = $('#combobox-progress').selectpicker('val');
    if (window.locationId != null && typeof window.locationId !== "undefined") {
        if (window.locationId.length > 0) {
            window.locationId = Array();
        }
    }
    window.locationId = $('#combobox-location').selectpicker('val');
    if (window.homeCountryIds != null && typeof window.homeCountryIds !== "undefined") {
        if (window.homeCountryIds.length > 0) {
            window.homeCountryIds = Array();
        }
    }
    window.homeCountryIds = $('#combobox-homeCountries').selectpicker('val');
    if (window.industryId != null && typeof window.industryId !== "undefined") {
        if (window.industryId.length > 0) {
            window.industryId = Array();
        }
    }
    window.industryId = $('#combobox-industry').selectpicker('val');
    window.notShowClosecase = !$('#checkbox-not-show-close-case').is(":checked");

    window.sustainalyticsId = $('#textbox-sustainalytics-id').val();

    var postData = {
        'notshowclosecase': window.notShowClosecase,
        'onlycompanieswithactivecase': $('#checkbox-only-companies-with-active-case').is(":checked"),
        'onlySearchCompanyName': window.onlySearchCompanyName,
        'onlyShowFocusList': window.onlyShowFocusList,

        'isin': window.isin,
        'name': window.searchStr,
        'engagementAreaIds': window.engagementAreaIds,

        'portfolioIds': window.portfolioIds,
        'recommendationId': window.recommendationId,
        'conclusionId': window.conclusionId,
        'responseId': window.responseId,
        'progressId': window.progressId,
        'locationId': window.locationId,
        'homeCountryIds': window.homeCountryIds,
        'industryId': window.industryId,
        'companyId': $('#hiddenCompanyIdSelected').val(),
        'sustainalyticsId' : window.sustainalyticsId
};

    return postData;
}

function preparePostDataForEngagementType() {
    var normareasId = serviceId + '--';
    window.searchStr = '';
    window.recommendationId = Array();
    window.conclusionId = Array();
    window.engagementAreaIds = normareasId;
    window.locationId = Array();
    window.responseId = Array();
    window.progressId = Array();
    window.industryId = Array();
    window.onlySearchCompanyName = false;
    window.onlyShowFocusList = false;
    window.notShowClosecase = engagementTypeId === 9;

    var postData = {
        'notshowclosecase': window.notShowClosecase,
        'onlycompanieswithactivecase': window.notShowClosecase,
        'onlySearchCompanyName': false,
        'onlyShowFocusList': false,
        'isin': '',
        'name': '',
        'engagementAreaIds': normareasId, 
        'portfolioIds': Array(),
        'recommendationId': Array(),
        'conclusionId': Array(),
        'responseId': Array(),
        'progressId': Array(),
        'locationId': Array(),
        'homeCountryIds': Array(),
        'industryId': Array(),
        'companyId': ''
    };

    return postData;
}

function SearchCompany() {
    //hide div from home page
    $(".box-home-page").hide();

    // clear selectedGridRowIds array
    window.selectedGridRowIds = [];

    var grid = $("#tblcompanies");

    if (typeof isOnlyForEngagementType !== 'undefined' && isOnlyForEngagementType !== null && isOnlyForEngagementType === true){
        postData = preparePostDataForEngagementType();
    } else {
        var postData = preparePostData();
    }

    if (window.initializedJqgrid === true) {
        grid.setGridParam({ postData: null });
        grid.jqGrid("setGridParam",
            {
                postData: postData,
                page: 1
            });

        if (oldGrid !== "") {
            $("#tblcompanies tbody").html(oldGrid);
        }

        $("#tblcompanies").trigger("reloadGrid");
    } else {
        initGrid(grid, postData);
    }
}

function keypressHandler(e) {
    if (e.which == 13) {
        e.preventDefault();
        $(this).blur();
        $('#submit-search').focus().click();
    }
}

$('#textbox-company-name').keypress(keypressHandler);
$('#textbox-company-isin').keypress(keypressHandler);

$('#checkbox-not-show-close-case').change(function () {
    SearchCompany();
});

$('#checkbox-only-companies-with-active-case').change(function () {
    SearchCompany();
});

$('#textbox-company-name').catcomplete({
    source: function (request, response) {
        window.searchStr = request.term;
        $.ajax({

            type: "POST",

            url: "/Company/GetCompaniesForAutocomplete",

            data: {
                term: request.term,
                limit: 30
            },

            success: function (ret, textStatus, jqXhr) {
                response($.map(ret.rows, function (item) {
                    return { label: item.Id, category: item.Name, desc: item.CompanyId };
                }));
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log("Autocomplete failed.");
            }
        });
    },
    minLength: 2,
    delay: 250,

    focus: function (event, ui) {
        $('#textbox-company-name').val(ui.item.label.split('(')[0].trim());
        $('#hiddenCompanyIdSelected').val(ui.item.desc);
        
        return false;
    },

    select: function (event, ui) {
        $('#textbox-company-name').val(ui.item.label.split('(')[0].trim());
        $('#hiddenCompanyIdSelected').val(ui.item.desc);
        $('#submit-search').focus().click();
        return false;
    }
});

$(".export-btn").on("click", function (e) {
    var postDataObj = preparePostData();

    // isNew
    var isNewParam = window.isNew == true ? true : false;
    postDataObj.isNew = isNewParam;

    // if "export selected" >>> add selectedIds to postDataObj
    if ($(e.target).hasClass("export-selected-search-list-btn")) { // export selected button clicked
        var selectedIdsStr = window.selectedGridRowIds.join(",");
        postDataObj.companyIds = selectedIdsStr;
    }

    // create param string
    var paramStr = utils.createParamStr(postDataObj);
    window.location.href = "/Company/ExportPrescreeningSearchToExcel?" + paramStr;
    return true;
});

$(".add-to-focus-list-btn").on("click", function (e) {
    var postDataObj = preparePostData();

    // create param string
    var paramStr = utils.createParamStr(postDataObj);

    // disable button
    $(".add-to-focus-list-btn").addClass("disabled");
    $.ajax({
        url: '/Company/AddSearchResultToFocusList?' + paramStr,
        datatype: 'json',
        contentType: false,
        processData: false,
        type: 'POST'
    })
        .done(function (response, textStatus, jqXHR) {
            if (response.success) {
                utils.quickNotification(response.message);
                $("#submit-search").click();
            } else {
                utils.quickNotification("You are trying to add " + response.numCompanies + " companies to your Focus list at once. The limit is 50. Kindly narrow down your search conditions.", "warning");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            utils.quickNotification("Failed: Error occurred", "error");
        })
        .always(function (jqXHR, textStatus, errorThrown) {
            // enable the button again
            $(".add-to-focus-list-btn").removeClass("disabled");
        });

    return false;
});


$(".export-all-holdings-btn").on("click", function (e) {
    var postDataObj = preparePostData();

    window.location.href = "/Company/ExportAllHoldings?portfolioIds=" + postDataObj.portfolioIds;
    return true;
});

$('.interval-selections').datepicker({ autoclose: true });
$("#textbox-startdate").attr("placeholder", "Interval Start Date");
$("#textbox-enddate").attr("placeholder", "Interval End Date");

function showMyEndorsement(caseReportId, subgrid) {
    var link = $("#my-endorsement-link-" + caseReportId);
    var icon = $("#my-endorsement-loading-icon-" + caseReportId);

    link.hide();
    icon.show();

    window.currentSubGridId = subgrid.id;

    $.ajax({
        url: '/CaseProfile/CreateMyEndorsementForm?caseReportId=' + caseReportId,
        method: 'GET',
        success: function (data) {
            $("#myEndorsementModalContainer").html(data);

            link.show();
            icon.hide();

            $("#myEndorsementModal").modal("show");
        }
    });
}

// used for onSelectRow func
function updateIdsOfSelectedRows(id, isTicked) {
    var index = $.inArray(id, window.selectedGridRowIds);
    if (!isTicked && index >= 0) {
        window.selectedGridRowIds.splice(index, 1); // remove id from the list
    } else if (index < 0) {
        window.selectedGridRowIds.push(id);
    }

    // enable/disable "Export selected" button
    if (window.selectedGridRowIds.length > 0)
        $(".export-selected-search-list-btn").removeClass("disabled");
    else
        $(".export-selected-search-list-btn").addClass("disabled");
};
// used for onSelectAll func
function selecAllRows(aRowids, isTicked) {
    var i, count, id;
    for (i = 0, count = aRowids.length; i < count; i++) {
        id = aRowids[i];
        updateIdsOfSelectedRows(id, isTicked);
    }
};

function initGrid(grid, postData) {
    var postUrl = "/company/GetDataForCompaniesJqGrid";
    var gridCaption = "";

    var kpiPieCharts = [];

    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, items, orgClickEvent) {
        // search string = ""
        if (typeof window.searchStr === "undefined")
            window.searchStr = "";

        $(window).resize();

        items.rows.forEach(function (item, index, array) {
            if (item.NumCases <= 0) {
                $("#" + item.Id + " td.ui-sgcollapsed").text("");
                $("#" + item.Id + " td.ui-sgcollapsed").unbind("click").html("");
            }
        });

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");

        // highlight: keyword matching company name
        $(".cell-company-name").highlight(window.searchStr);
    });

    function generateCheckBoxId(prefixText, id) {
        return prefixText + id;
    }

    function cboxFormatterGesCaseReportId(cellvalue, options, rowObject) {
        //return '<input type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + ' id="' + generateCheckBoxId("GesCaseReportsId_", rowObject.Id) + '"/>';
        var btnClass = "btn-default btn-case-signup signup-none ";
        var btnText = rowObject.CanSignUp === true ? "Sign up" : "None";
        switch (cellvalue) {
            case 0:
                break;
            case 1:
                btnClass = "btn-success btn-case-signup signup-active ";
                btnText = "Disclose";
                break;
            case 2:
                btnClass = "btn-warning btn-case-signup signup-passive ";
                btnText = "Non-disclose";
                break;
        }
        btnClass += rowObject.CanSignUp ? "" : "disabled";
        return '<div class="btn ' + btnClass + ' btn-xs" id="' + generateCheckBoxId("GesCaseReportsId_", rowObject.Id) + '">' + btnText + '</div>';
    }

    function drawFormatterKPIs(cellvalue, options, rowObject) {
        if (rowObject.KPIs != null && rowObject.KPIs != undefined && rowObject.KPIs.length > 0) {
            var pieChart = {};
            pieChart.Id = generateCheckBoxId("div_kpi_chart_", rowObject.Id);

            pieChart.pieColors = [];
            pieChart.pieData = [];

            rowObject.KPIs.forEach(function (item, index, array) {
                pieChart.pieColors.push(window.kpiPieColors[item.KpiPerformance]);
                var pie = {};
                pie.name = item.KpiName;
                pie.y = 1;
                pieChart.pieData.push(pie);
            });

            kpiPieCharts.push(pieChart);
            return '<div id="' + pieChart.Id + '" class="kpi-pie-chart"></div>';
        }
        return '<div class="jqgrid-cell-not-available-style">N.A</div>';
    }

    function cboxFormatterCompanyFocusList(cellvalue, options, rowObject) {
        var thirdStateCls = cellvalue === -1 ? "third-state" : "";
        return '<input class="checkbox-focuslist checkbox-focus-company ' + thirdStateCls + '" value="' + cellvalue + '" type="checkbox"' + (cellvalue === 1 ? ' checked="checked"' : '') + ' id="' + generateCheckBoxId("CompanyInFocusList_", rowObject.Id) + '"/>';
    }

    function cboxFormatterCaseReportInFocusList(cellvalue, options, rowObject) {
        return '<input class="checkbox-focuslist checkbox-focus-case" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + ' id="' + generateCheckBoxId("GesCaseReportInFocusList_", rowObject.Id) + '"/>';
    }

    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        postData: postData,

        multiselect: true,
        onSelectRow: updateIdsOfSelectedRows, // add/remove selected row to/from selectedGridRowIds array
        onSelectAll: selecAllRows, // Select All checkbox: add/remove ALL rows to/from selectedGridRowIds array
        beforeRequest: function () {
            // debug info: print stored selected rows
            //console.log("Selected rows (Saved):");
            //console.log(window.selectedGridRowIds);
        },

        colNames: ["Id", "CompanyId", "Focus", "Company", "Domicile", "Cases", "Alerts"],
        colModel: [
            { name: "Id", align: "right", hidden: true, search: false, key: true },
            { name: "CompanyId", hidden: true },
            { name: "IsInFocusList", width: 25, align: "center", classes: 'companyInFocusList inFocusList-Company', formatter: cboxFormatterCompanyFocusList },
            {
                name: "CompanyIssueName",
                width: 200,
                searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    //if (window.isNew == true) {
                    return utils.genCompanyLink(rowObject.CompanyId, cellvalue);
                    //} else {
                    //    return utils.genCompanyLink_Old(rowObject.CompanyId, cellvalue);
                    //}
                }, title: false
            },
        { name: "HomeCountry", width: 100, formatter: utils.renderTextWithNAValue, title: false },
            {
                name: "NumCases", width: 75, classes: "grid-cell-numCases", align: "center",
                formatter: function (cellvalue, options, rowObject) {
                    return utils.genNumCasesHrefAction(cellvalue);
                }, title: false
            },
            {
                name: "NumAlerts", width: 75, classes: "grid-cell-numAlerts", align: "center",
                formatter: function (cellvalue, options, rowObject) {
                    if (window.isNew == true) {
                        return utils.genAlertResultLink(rowObject.Isin, cellvalue);
                    } else {
                        return utils.genAlertResultLink_Old(rowObject.Isin, cellvalue);
                    }
                }, title: false
            }
        ],
        pager: $("#myPager"),
        rowNum: 20,
        rowList: [10, 20, 50, 100, 500, 1000],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        height: "auto",
        viewrecords: true,
        caption: gridCaption,
        sortname: "marketcap",
        sortorder: "asc",
        beforeSelectRow: function (rowid, e) {
            // not allow to select too many companies to export
            var limitNumCompanies = 50;
            if (window.selectedGridRowIds.length >= limitNumCompanies && $(e.target).prop("checked")) {
                // uncheck first
                $(e.target).attr("checked", false);
                // then show message
                //alert("You can only select maximum " + limitNumCompanies + " companies to export.");
                utils.quickNotification("You can only select a maximum of " + limitNumCompanies + " companies to export.", "error");
                return false;
            }

            // only allow select row via checkbox
            var $myGrid = $(this),
                i = $.jgrid.getCellIndex($(e.target).closest("td")[0]),
                cm = $myGrid.jqGrid("getGridParam", "colModel");
            return (cm[i].name === "cb");

            //return false;
        },
        loadComplete: function (jsondata, stat) {
            $('#hiddenCompanyIdSelected').val('');
            $('#hdCompanyIdSelected').val('');
            $("#lbl-search-message-result").text('');
            // hide Select all checkbox            
            $("#cb_" + grid[0].id).hide();
            $("#tblcompanies").jqGrid("setLabel", "cb", "<i class='fa fa-fw fa-upload'></i>");
            $("#tblcompanies").jqGrid("setLabel", "subgrid", "<i class='fa fa-fw fa-sitemap'></i>");

            // restore checkboxes state of selected rows
            for (var i = 0, count = window.selectedGridRowIds.length; i < count; i++) {
                grid.jqGrid("setSelection", window.selectedGridRowIds[i], false);
            }

            // no result text
            var total = $("#tblcompanies").jqGrid('getGridParam', 'records');

            if (total == 0) {
                oldGrid = $('#tblcompanies tbody').html();
                var msgText = "No companies were found matching your search criteria.";
                if (jsondata.message != null && jsondata.message.length > 0) {
                    msgText = jsondata.message;
                }

                //$('#tblcompanies tbody').html("<div class='jqgrid-no-row-style'>No companies were found matching your search criteria.</div>");
                $('#tblcompanies tbody').html("<div class='jqgrid-no-row-style'>" + msgText + "</div>");

                $(".ui-paging-info").text("");
            }

            if (total > 0 && jsondata.message != null && jsondata.message.length > 0) {
                $("#lbl-search-message-result").text(jsondata.message);
            }

            // if there is at least one company, but <= 50 companies >>> enable "Add all to my Focus list" button
            if (total > 0) {
                $(".add-to-focus-list-btn").removeClass("disabled");
            } else {
                $(".add-to-focus-list-btn").addClass("disabled");
            }

            // focus list checkbox
            $('input.checkbox-focus-company').iCheck({
                //checkboxClass: 'icheckbox_flat-blue',
                checkboxClass: 'iradio_flat-blue',
                radioClass: 'iradio_flat-blue'
            });
            $('input.checkbox-focus-company').on('ifChanged', function (event) {
                $(event.target).trigger('change');
            });

            // third state for checkboxes
            $.each($("input.third-state"), function (index, item) {
                $(item).closest("div").addClass("third-state");
            });

            // header tooltips
            utils.setTooltipsOnColumnHeader($("#tblcompanies"), 0, "Select items for export");
            utils.setTooltipsOnColumnHeader($("#tblcompanies"), 1, "Collapse/Expand list of issues");
            utils.setTooltipsOnColumnHeader($("#tblcompanies"), 4, "Add/Remove items to/from your Focus List");
            //utils.setTooltipsOnColumnHeader($("#tblcompanies"), 5, "Company Name and Link");
            //utils.setTooltipsOnColumnHeader($("#tblcompanies"), 6, "Home Country");
            //utils.setTooltipsOnColumnHeader($("#tblcompanies"), 7, "Number of cases");
            //utils.setTooltipsOnColumnHeader($("#tblcompanies"), 8, "Number of alerts");

            // Click "Number of Cases" cells > open subgrid
            //$.each($(".grid-cell-numCases").has(".cell-numCases-action"), function (index, item) {
            $.each($(".cell-numCases-action"), function (index, item) {
                $(item).bind("click", function () {
                    $(this).closest("td").prevAll(".ui-sgcollapsed:first").trigger("click");
                });
            });

            //setup Focus list
            $("input.checkbox-focus-company").change(function (e) {
                var gesCompanyIdMatches = this.id.match(/\d+/);
                var gesCompanyId = -1;
                if (gesCompanyIdMatches.length > 0) {
                    gesCompanyId = gesCompanyIdMatches[0];
                }

                var data = {
                    newValue: this.checked,
                    gesCompanyId: gesCompanyId
                }
                var self = $(this);

                // third-state checkpoint
                if (this.checked) { // add company to focus list >>> nothing to do here

                } else {
                    var expandedContentId = "tblcompanies_" + gesCompanyId + "_expandedContent";
                    if (gesCompanyId !== -1 && $("#" + expandedContentId).length) {
                        if ($("#" + expandedContentId + " .iradio_flat-blue.checked").length) {
                            $(this).addClass("third-state");
                            $(this).closest("div").addClass("third-state");
                        } else {
                            $(this).removeClass("third-state");
                            $(this).closest("div").removeClass("third-state");
                        }
                    }
                }

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Company/UpdateGesCompanyWatcher",
                    data: JSON.stringify(data)
                })
                    .done(function (response, textStatus, jqXHR) {
                        if (response.meta.success) {
                            utils.quickNotification("Successfully updated Focus list");
                            //jQuery("#tblcompanies_" + gesCompanyId).trigger("reloadGrid");
                        } else {
                            utils.quickNotification(response.meta.error, "error");
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        // revert action
                        //self.prop("checked", self.val()); // using self.val()??? seems to be wrong here

                        utils.quickNotification("Failed: Error occurred updating focus list", "error");
                        grid.trigger("reloadGrid");
                    });
            });
        },
        subGrid: true,
        subGridOptions: {
            "plusicon": "ui-icon-triangle-1-e",
            "minusicon": "ui-icon-triangle-1-s",
            "openicon": "ui-icon-arrowreturn-1-e",
            "reloadOnExpand": false,
            "selectOnExpand": false
        },
        subGridRowExpanded: function (subgrid_id, row_id) {
            var subgrid_table_id = subgrid_id + "_t";
            var pager_id = "p_" + subgrid_table_id;

            var classForSignUpCell = 'cellCheckboxSignUp' + subgrid_id;
            var classForFocusCell = 'inFocusList-Case cellCheckboxFocus' + subgrid_id;

            var rowData = grid.getRowData(row_id);

            $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: encodeURI('/Company/GetCaseReports' + '?gesCompanyId=' + rowData.Id
                    + '&companyId=' + rowData.CompanyId
                    + '&notshowclosecase=' + window.notShowClosecase
                    + '&name=' + window.searchStr
                    + '&recommendationIdString=' + window.recommendationId
                    + '&conclusionIdString=' + window.conclusionId
                    + '&engagementAreaIds=' + window.engagementAreaIds
                    + '&locationIdString=' + window.locationId
                    + '&responseIdString=' + window.responseId
                    + '&progressIdString=' + window.progressId
                    + '&industryIdString=' + window.industryId
                    + '&onlySearchCompanyName=' + window.onlySearchCompanyName
                    + '&onlyShowFocusList=' + window.onlyShowFocusList
                    + '&companyInfocusList=' + $("#" + rowData.Id + " .companyInFocusList input:first").is(":checked")),
                datatype: 'json',
                mtype: 'POST',
                colNames: ["Id", "Focus", "Issue", "Location", "Entry date", "Norm/Theme", "Engagement status", "KPI", "Response", "Progress", "Endorse"],
                colModel: [
                    { name: "Id", index: "I_GesCaseReports_Id", key: true, hidden: true },
                    { name: "IsInFocusList", width: 45, align: "center", classes: classForFocusCell, formatter: cboxFormatterCaseReportInFocusList },
                    {
                        name: "IssueName", index: "ReportIncident", width: 150, sortable: false, title: false,
                        formatter: function (cellvalue, options, rowObject) {
                            return utils.genCaseReportLink(cellvalue, rowObject);
                        }
                    },
                    { name: "Location", width: 100, sortable: false, formatter: utils.renderTextWithNAValue, title: false, align: "center" },
                    { name: "EntryDate", width: 100, sortable: false, formatter: utils.dateFormatter, align: "center", title: false },
                    { name: "ServiceEngagementThemeNorm", width: 125, sortable: false, formatter: utils.renderTextWithNAValue, title: false, align: "center" },
                    { name: "Recommendation", width: 80, sortable: false,
                        formatter: utils.renderTextWithNAValue, classes: "heading-italic", title: false, align: "center"
                    },
                    { name: "KPIs", width: 65, sortable: false, title: false, formatter: drawFormatterKPIs, align: "center"},                    
                    {
                        name: "ResponseGrade", width: 100, sortable: false, align: "center", formatter: function (cellvalue, options, rowObject) {
                            return utils.renderResProgGradeVisual(cellvalue, "response");
                        }, title: false, hidden: true
                    },
                    {
                        name: "ProgressGrade", width: 100, sortable: false,  align: "center",
                        formatter: function (cellvalue, options, rowObject) {
                            return utils.renderResProgGradeVisual(cellvalue, "progress");
                        }, title: false, hidden: true
                    },
                    { name: "SignUpValue", width: 50, align: "center", classes: classForSignUpCell, formatter: cboxFormatterGesCaseReportId, title: false }

                ],
                rowNum: 9999,
                sortname: "EngagementType",
                sortorder: "asc",
                height: "100%",
                toppager: false,
                beforeSelectRow: function (rowid, e) {
                    return false;
                },
                loadComplete: function () {
                    $(window).resize();

                    $("#" + subgrid_table_id + " tr.jqgrow:even").addClass("jqgrid-row-even");

                    // header tooltips
                    utils.setTooltipsOnColumnHeader($("#" + subgrid_table_id), 1, "Add/Remove items to/from your Focus List");
                    utils.setTooltipsOnColumnHeader($("#" + subgrid_table_id), 5, "GS = Global Standards \nS&R = Stewardship & Risk");
                    utils.setTooltipsOnColumnHeader($("#" + subgrid_table_id), 6, "Evaluate\nEngage\nDisengage\nResolved\nArchived");
                    utils.setTooltipsOnColumnHeader($("#" + subgrid_table_id), 8, "Endorsement means that a client supports the case and will be invited to the different engagement activities of the specific case. Disclose means that a client displays its name on Engagement Forum and in communications with the company. Non-disclose means that the client does not want to publish its name to other clients.");

                    // focus list checkbox
                    $('#' + subgrid_table_id + ' .checkbox-focus-case').iCheck({
                        checkboxClass: 'iradio_flat-blue',
                        radioClass: 'iradio_flat-blue'
                    });
                    $('#' + subgrid_table_id + ' .checkbox-focus-case').on('ifChanged', function (event) {
                        $(event.target).trigger('change');
                    });

                    // match title
                    $.each($("#" + subgrid_table_id + " tr.jqgrow .sr-caseprofile"), function (key, caseTitleNode) {
                        if (window.searchStr != "" && caseTitleNode.innerText.toLowerCase().indexOf(window.searchStr) >= 0) {
                            $(caseTitleNode).addClass("matched-title");
                        }
                    });

                    // highlight keyword in case profiles' title + norm
                    $(".sr-caseprofile").highlight(window.searchStr);
                    $(".sr-norm").highlight(window.searchStr);

                    //setup SignUp
                    $("." + classForSignUpCell + " .btn-case-signup").click(function (e) {
                        if ($(this).hasClass("disabled")) {
                            return;
                        }

                        var signedUp = !$(this).hasClass("signup-none");
                        var gesCaseReportIdMatches = this.id.match(/\d+/);
                        window.SignedUpGesCaseReportId = -1;
                        if (gesCaseReportIdMatches.length > 0) {
                            window.SignedUpGesCaseReportId = gesCaseReportIdMatches[0];
                        }

                        if (signedUp) {
                            var data = {
                                isSignUp: false,
                                isActive: false,
                                gesCaseReportId: window.SignedUpGesCaseReportId
                            }
                            var self = $(this);

                            $.ajax({
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                type: "POST",
                                url: "/Company/UpdateGesCaseReportSignUp",
                                data: JSON.stringify(data)
                            })
                                .done(function (response, textStatus, jqXHR) {
                                    if (response.meta.success) {
                                        console.log($(this));
                                        self.removeClass("signup-active");
                                        self.removeClass("signup-passive");
                                        self.removeClass("btn-success");
                                        self.removeClass("btn-warning");
                                        self.addClass("signup-none");
                                        self.addClass("btn-default");
                                        self.text("Sign up");
                                        utils.quickNotification("Cancelled endorsement successfully");
                                    } else {
                                        utils.quickNotification(response.meta.error, "error");
                                    }
                                })
                                .fail(function (jqXHR, textStatus, errorThrown) {
                                    utils.quickNotification("Failed: Error occurred updating endorsement", "error");
                                });
                        } else { // not signed up >> need to ask "active" or "passive"?
                            $("#SignUpTypeModal").modal("show");
                        }
                    });


                    //setup Focus list
                    $('#' + subgrid_table_id + ' .checkbox-focus-case').change(function (e) {
                        var gesCaseReportIdMatches = this.id.match(/\d+/);
                        var gesCaseReportId = -1;
                        if (gesCaseReportIdMatches.length > 0) {
                            gesCaseReportId = gesCaseReportIdMatches[0];
                        }

                        var data = {
                            newValue: this.checked,
                            gesCaseReportId: gesCaseReportId
                        }
                        var self = $(this);

                        // third-state checkpoint (subgrid)
                        var gesCompanyIdMatches = $(this).closest(".ui-sg-expanded").attr("id").match(/\d+/);
                        var gesCompanyId = -1;
                        if (gesCompanyIdMatches.length > 0) {
                            gesCompanyId = gesCompanyIdMatches[0];
                        }

                        if (this.checked) { // add case to focus list
                            // if company is in focus list already >>> nothing to do
                            // if company is not in focus list >>> activate third-state on company level

                            if (gesCompanyId !== -1 && $("#" + gesCompanyId + " .iradio_flat-blue.checked").length) {

                            } else if (gesCompanyId !== -1) {
                                $("#" + gesCompanyId + " .iradio_flat-blue").addClass("third-state");
                            }
                        } else {
                            var expandedContentId = "tblcompanies_" + gesCompanyId + "_expandedContent";

                            if (gesCompanyId !== -1 && $("#" + expandedContentId + " .iradio_flat-blue.checked").length > 1) {
                                // do nothing, as there's still some cases checked
                            } else if (gesCompanyId !== -1) {
                                if ($("#" + gesCompanyId + " .iradio_flat-blue.checked").length) {

                                } else {
                                    $("#" + gesCompanyId + " .iradio_flat-blue").removeClass("third-state");
                                }
                            }
                        }

                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            type: "POST",
                            url: "/Company/UpdateGesCaseReportsG_Individuals",
                            data: JSON.stringify(data)
                        })
                            .done(function (response, textStatus, jqXHR) {
                                if (response.meta.success) {
                                    utils.quickNotification("Successfully updated Focus list");
                                } else {
                                    utils.quickNotification(response.meta.error, "error");
                                }
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                // revert action
                                self.prop("checked", self.val());

                                utils.quickNotification("Failed: Error occurred updating focus list", "error");
                                grid.trigger("reloadGrid");
                            });
                    });

                    //draw pie chart
                    if (kpiPieCharts.length > 0) {
                        jQuery("#" + subgrid_table_id).showCol("KPIs");
                        kpiPieCharts.forEach(function(item, index, array) {
                            chartCreator.createPieChart(item.Id, item.pieColors, item.pieData);
                        });

                        kpiPieCharts = [];
                    } else {
                        jQuery("#" + subgrid_table_id).hideCol("KPIs");
                    }

                    if (typeof (engagementTypeId) !== "undefined" && engagementTypeId === 9) {
                        jQuery("#" + subgrid_table_id).hideCol("Recommendation");
                        //jQuery("#" + subgrid_table_id).hideCol("Confirmed");
                        jQuery("#" + subgrid_table_id).showCol("ResponseGrade");
                        jQuery("#" + subgrid_table_id).showCol("ProgressGrade");
                    }

                    if (typeof (clientType) !== "undefined") {
                        if (clientType == 'GlobalEthicalStandardOnly') {
                            jQuery("#" + subgrid_table_id).hideCol("Recommendation");
                            //jQuery("#" + subgrid_table_id).hideCol("Confirmed");
                            jQuery("#" + subgrid_table_id).hideCol("SignUpValue");
                        }
                    }

                    $(".tooltip-hint").each(function (index, tooltip) {
                        var title = $(tooltip).attr("data-tooltip-title");
                        var content = $("#" + $(tooltip).attr("data-tooltip-content")).html();
                        $(tooltip).popover(utils.getPopoverConfig(title, content));
                    });
                }
            });
            jQuery("#" + subgrid_table_id).jqGrid('navGrid', "#" + pager_id, {
                edit: false, add: false, del: false
            });
        }
    });

    window.initializedJqgrid = true;
}

$(".go-back-unsubscribed").on("click", function (evt) {
    utils.goBackHistory(evt);
});



var chartCreator = (function () {
    var createPieChart = function (containerId, pieColors, pieData) {
        Highcharts.chart(containerId,
            {
                chart: {
                    plotBackgroundColor: null,
                    backgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: "pie",
                    spacingBottom: 0,
                    spacingTop: 0,
                    spacingLeft: 0,
                    spacingRight: 0,
                    margin: -10,
                    width: 22,
                    height: 22
                },
                title: {
                    text: ""
                },
                tooltip: {
                    enabled: false
                },
                plotOptions: {
                    pie: {
                        colors: pieColors,
                        allowPointSelect: false,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        }
                    }
                },
                credits: {
                    enabled: false
                },
                series: [
                    {
                        colorByPoint: true,
                        states: { hover: { enabled: false } },
                        data: pieData
                    }
                ]
            });
    };

    return {
        createPieChart: createPieChart
    }
})();
