$(function () {
    var caseSummaryModals = "";
    var alertSummaryModals = "";

    // Add to Focus list button
    $(".btn-add-to-focus-list").on("click", function (e) {
        var self = $(e.target);
        var currentInFocusListValue = self.hasClass("in-focus-list");
        var data = {
            newValue: !currentInFocusListValue,
            gesCompanyId: gesCompanyId
        }

        $(".btn-add-to-focus-list").button("loading");
        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            url: "/Company/UpdateGesCompanyWatcher",
            data: JSON.stringify(data)
        })
            .done(function (response, textStatus, jqXHR) {
                //self.button("reset");
                if (response.meta.success) {
                    if (data.newValue === true) {
                        utils.quickNotification("Successfully added to your focus list");
                    } else {
                        utils.quickNotification("Successfully removed from your focus list");
                    }
                    
                    updateAddToFocusListBtn(data.newValue);
                } else {
                    utils.quickNotification(response.meta.error, "error");
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                self.button("reset");
                utils.quickNotification("Failed: Error occurred updating focus list", "error");
            })
            .always(function () {
                
            });
    });

    // Events
    $("#ei-events").eventify({
        locale: "en" //TODO(truong.pham) add limitedCharacter depend on window size
    });

    // Linkify
    $("#ei-events").linkify({
        target: "_blank"
    });
    $(".box-company-overview").linkify({
        target: "_blank"
    });

    // Tab Management
    var activeTab = null;
    $("a[data-toggle=\"tab\"]").on("shown.bs.tab", function (e) {
        $(window).resize();

        activeTab = e.target.hash;
        if (activeTab === "#events") {
            // dotdotdot: truncate the events description
            $(".ei-description").dotdotdot({
                ellipsis: "...",
                height: 18
            });
        }
    });

    /*-- Documentation --*/
    $(".btn-doc-show-all").on("click", function(e) {
        $("#documents tr.hidden-doc-row").toggle();
        var label = $(e.target).text() === "Collapse" ? "Show All" : "Collapse";
        $(e.target).text(label);
    });/*-- /Documentation --*/

    /*-- Templates (Handlebars) --*/
    //-- Summary template
    var summarySource = $("#summary-modal-template").html();
    window.summaryTpl = Handlebars.compile(summarySource);
    //-- Cases (Print version) template
    var printCasesSource = $("#print-cases-template").html();
    window.printCasesTpl = Handlebars.compile(printCasesSource);
    //-- Alerts (Print version) template
    var printAlertsSource = $("#print-alerts-template").html();
    window.printAlertsTpl = Handlebars.compile(printAlertsSource);
    /*-- /Templates (Handlebars) --*/

    /*-- Cases --*/
    var postUrl = "/company/GetCaseReportsByCompanyId";
    var gridCaption = "";
    var tblId = "tblcases";
    var grid = $("#" + tblId);
       
    $.jgrid.defaults.responsive = true;
    grid.bind("jqGridLoadComplete", function (e, items, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("#"+ tblId + " tr.jqgrow:even").addClass("jqgrid-row-even");
    });

    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "POST",
        postData: {
            'gesCompanyId': window.i_gesCompanyId,
            'companyId': window.i_companyId,
            'notshowclosecase': !$("#cb-show-closed-cases").is(":checked"),
            'orgId': window.i_orgId,
            'individualId': window.i_individualId
        },        
        colNames: ["Id", "Issue", "Location", "Norm/Theme", "Engagement status", "Entry date"],
        colModel: [
            { name: "Id", index: "I_GesCaseReports_Id", key: true, search: false, hidden: true },
            {
                name: "IssueName", index: "ReportIncident", width: 150, sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    caseSummaryModals += utils.genSummaryModalContent("case", rowObject, summaryTpl, window.i_companyName);

                    return utils.genSummaryLink("case", rowObject) + utils.genCaseReportLink(cellvalue, rowObject);
                }, title: false
            },
            { name: "Location", width: 80, sortable: true, formatter: utils.renderTextWithNAValue, title: false, align: "center" },
            { name: "ServiceEngagementThemeNorm", width: 110, sortable: true, formatter: utils.renderTextWithNAValue, title: false, align: "center" },            
            { name: "Recommendation", width: 80, sortable: true, align: "center", formatter: utils.renderTextWithNAValue, title: false },
            //{ name: "Conclusion", width: 80, sortable: true, formatter: utils.renderTextWithNAValue, title: false, align: "center" },
            //{
            //    name: "Confirmed", search: false, width: 58, title: false,
            //    formatter: function (cellvalue, options, rowObject) {
            //        return utils.renderConfirmedVisual(cellvalue);
            //    }, align: "center"
            //},
            { name: "EntryDate", width: 60, formatter: utils.dateFormatter, sortable: true, align: "center", title: false }
            
        ],
        sortname: "EngagementType",
        sortorder: "desc",
        height: "auto",
        toppager: true,
        pager: $("#tblcases-pager"),
        rowNum: 10,
        rowList: [5, 10, 20, 999],
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: gridCaption,

        beforeSelectRow: function (rowid, e) {
            return false;
        },
        loadComplete: function (data) {
            // render print version for case profiles
            genPrintCasesContent(window.printCasesTpl, data);

            // update summary modals html
            $(".caseSummaryModalsHtml").html(caseSummaryModals);
            caseSummaryModals = "";

            // no result text
            var total = $("#tblcases").jqGrid('getGridParam', 'records');
            $(".tab-cases a").html("<i class='fa fa-briefcase'></i>&nbsp;&nbsp;Cases (" + total + ")");
            if (total == 0) {                
                $("#caseProfilesTableContainer").hide();                
                $(".tab-cases .ui-paging-info").text("");
                $(".no-cases-msg").show();
            } else {
                $("#caseProfilesTableContainer").show();                
                $(".no-cases-msg").hide();
            }            

            // header tooltips
            //utils.setTooltipsOnColumnHeader($("#tblcases"), 1, "Issue Name and Link");
            //utils.setTooltipsOnColumnHeader($("#tblcases"), 2, "Location");
            utils.setTooltipsOnColumnHeader($("#tblcases"), 3, "GS = Global Standards \nS&R = Stewardship & Risk");
            utils.setTooltipsOnColumnHeader($("#tblcases"), 4, "Evaluate\nEngage\nDisengage\nResolved\nArchived");
            //utils.setTooltipsOnColumnHeader($("#tblcases"), 5, "Entry Date");
            utils.setTooltipsOnColumnHeader($("#tblcases"), 6, "The indicator describes how the company responds to GES’ inquiries.");
            utils.setTooltipsOnColumnHeader($("#tblcases"), 7, "The indicator describes whether or not the violation continues, or how the company’s work to prevent future violations is developing.");
            utils.setTooltipsOnColumnHeader($("#tblcases"), 8, "The indicator describes the combined company progress and response performance.");

            // hide pagers if there's only 1 page
            var numPages = grid.jqGrid("getGridParam", "lastpage");
            if (numPages <= 1) {
                $("#" + tblId + "_toppager").hide();
                $("#" + tblId + "-pager").hide();
            } else {
                $("#" + tblId + "_toppager").show();
                $("#" + tblId + "-pager").show();
            }

            if (typeof (clientType) !== "undefined") {
                if (clientType == 'GlobalEthicalStandardOnly') {
                    jQuery("#" + tblId).hideCol("Recommendation");
                    jQuery("#" + tblId).hideCol("ResponseGrade");
                    jQuery("#" + tblId).hideCol("ProgressGrade");
                    jQuery("#" + tblId).hideCol("DevelopmentGrade");

                } else if (clientType == 'GlobalEthicalStandardAndSR') {
                    jQuery("#" + tblId).hideCol("ResponseGrade");
                    jQuery("#" + tblId).hideCol("ProgressGrade");
                    jQuery("#" + tblId).hideCol("DevelopmentGrade");
                }
            }
        },
        gridComplete: function() {
            $(".tooltip-hint").each(function(index, tooltip) {
                var title = $(tooltip).attr("data-tooltip-title");
                var content = $("#" + $(tooltip).attr("data-tooltip-content")).html();

                $(tooltip).popover(utils.getPopoverConfig(title, content));
                $(tooltip).closest(".ui-jqgrid .ui-jqgrid-bdiv").css("overflow", "auto");
            });
        }
    });

    // checkbox: show closed cases: changed
    $("#cb-show-closed-cases").change(function () {
        // update post data
        var postData = grid.jqGrid("getGridParam", "postData");
        postData.notshowclosecase = !$("#cb-show-closed-cases").is(":checked");

        grid.jqGrid("setGridParam", {
            postData: postData,
            page: 1
        });

        // reload
        grid.trigger("reloadGrid");
    });
    /*-- /Cases --*/

    /* ----------------------------------------------- */
    /* ----------------------------------------------- */
    /* ----------------------------------------------- */
    /* ----------------------------------------------- */
    /* ----------------------------------------------- */

    /*-- Alerts --*/
    var postUrlAlerts = "/Alert/GetAlerts";
    var tblIdAlerts = "tblalerts";
    var gridAlerts = $("#" + tblIdAlerts);

    gridAlerts.bind("jqGridLoadComplete", function (e, items, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("#" + tblIdAlerts + " tr.jqgrow:even").addClass("jqgrid-row-even");
    });

    gridAlerts.jqGrid({
        url: postUrlAlerts,
        datatype: "json",
        mtype: "POST",
        postData: {
            'companyId': window.i_companyId
        },
        colNames: ["Id", "Heading", "Location", "Norm", "Entry date", "Summary"],
        colModel: [
        { name: "Id", key: true, search: false, hidden: true },
        {
            name: "Heading",
            width: 200,
            sortable: true,
            formatter: function(cellvalue, options, rowObject) {
                alertSummaryModals += utils.genSummaryModalContent("alert", rowObject, summaryTpl, window.i_companyName);
                return utils.genSummaryLink("alert", rowObject) + utils.genAlertLink(cellvalue, rowObject);//cellvalue;
            }, title: false
        },
        { name: "Location", width: 90, sortable: true, align: "center", title: false },
        { name: "Norm", width: 90, sortable: true, align: "center", title: false },
        { name: "Date", width: 40, formatter: utils.dateFormatter, sortable: true, align: "center", title: false },
        {
            name: "Summary",
            width: 400,
            sortable: true,
            hidden: true, title: false
        }
        ],
        sortname: "Date",
        sortorder: "desc",
        height: "auto",
        toppager: true,

        pager: $("#tblalerts-pager"),
        rowNum: 10,
        rowList: [5, 10, 20, 999],
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        caption: gridCaption,

        beforeSelectRow: function (rowid, e) {
            return false;
        },
        loadComplete: function (data) {
            // render print version for case profiles
            genPrintAlertsContent(window.printAlertsTpl, data);

            // update summary modals html
            $(".alertSummaryModalsHtml").html(alertSummaryModals);
            alertSummaryModals = "";

            // no result text
            var total = $("#tblalerts").jqGrid('getGridParam', 'records');
            $(".tab-alerts a").html("<i class='fa fa-bell'></i>&nbsp;&nbsp;Alerts (" + total + ")");
            if (total == 0) {
                $("#alertsTableContainer").hide();       
                //$(".tab-alerts .ui-paging-info").text("");

                $(".no-alerts-msg").show();
            } else {
                $("#alertsTableContainer").show();       

                $(".no-alerts-msg").hide();
            }

            // header tooltips
            //utils.setTooltipsOnColumnHeader($("#tblalerts"), 1, "Alert Heading and Link");
            //utils.setTooltipsOnColumnHeader($("#tblalerts"), 2, "Alert Location");
            //utils.setTooltipsOnColumnHeader($("#tblalerts"), 4, "Alert Date");

            // hide pagers if there's only 1 page
            var numPages = gridAlerts.jqGrid("getGridParam", "lastpage");
            if (numPages <= 1) {
                $("#" + tblIdAlerts + "_toppager").hide();
                $("#" + tblIdAlerts + "-pager").hide();
            } else {
                $("#" + tblIdAlerts + "_toppager").show();
                $("#" + tblIdAlerts + "-pager").show();
            }
        }
    });
    /*-- /Alerts --*/

    companyExportPdf.onChangeSelectOption();
});

function genPrintCasesContent(tpl, data) {
    $.each(data.rows, function(key, value) {
        value.EntryDate = utils.dateFormatter(value.EntryDate);
        value.LastModified = utils.dateFormatter(value.LastModified);
    });
    var html = tpl(data);
    $("#print-case-profiles").html(html);
}

function genPrintAlertsContent(tpl, data) {
    $.each(data.rows, function (key, value) {
        value.Date = utils.dateFormatter(value.Date);
        value.LastModified = utils.dateFormatter(value.LastModified);
    });
    var html = tpl(data);
    $("#print-alerts").html(html);
}

function updateAddToFocusListBtn(isInFocusList) {
    var selector = ".btn-add-to-focus-list";

    if (isInFocusList === true) {
        $(selector).addClass("in-focus-list").addClass("azure-background");
        $(selector).html("<i class='fa fa-dot-circle-o'></i>&nbsp;&nbsp;Already in my focus list");
        $(selector).attr("title", "Click to remove from my Focus list");
    } else {
        $(selector).removeClass("in-focus-list");
        $(selector).first().html("<i class='fa fa-circle-o'></i>&nbsp;&nbsp;Add to my focus list");
        $(selector).attr("title", "Click to add to my Focus list");
    }
    $(selector).prop("disabled", false);
    $(selector).removeClass("disabled");
}

var companyExportPdf = function () {
    var toggleButtonName = function () {
            var isSelectAll = $("input[name='company-export']").length === $("input[name='company-export']:checked").length;
            if (isSelectAll) {
                $("#btn-export-select").html("Unselect all");
            } else {
                $("#btn-export-select").html("Select all");
            }
        },

    validateExportButton = function () {
        var isUnselectAll = $("input[name='company-export']:checked").length === 0;
        $("#btn-company-export").prop('disabled', isUnselectAll);
    },

    toogleSelect = function (groupName) {
        var isSelectAll = $("input[name='company-export']").length === $("input[name='company-export']:checked").length;
        $("input[name='" + groupName + "']:checkbox").each(function () {
            this.checked = !isSelectAll;
        });
        toggleButtonName();
        validateExportButton();
    },

    onChangeSelectOption = function () {
        $("input[name='company-export']").change(function () {
            toggleButtonName();
            validateExportButton();
        });
    },

    goToExportPage = function(groupName) {
        $("#export-icon").hide();
        $("#export-loading").show();
        $("#export-button").prop('disabled', true);

        var exportOptions = $("input[name='" + groupName + "']:checked").map(function () {
            return this.value; 
        }).get();
        
        var data = {
            companyId: window.i_companyId,
            gesCompanyId: window.i_gesCompanyId,
            showCompanyInfo: exportOptions.indexOf("showCompanyInfo") >= 0,
            showDialogue: exportOptions.indexOf("showDialogue") >= 0,
            showCompanyOverview: exportOptions.indexOf("showCompanyOverview") >= 0,
            showCaseProfiles: exportOptions.indexOf("showCaseProfiles") >= 0,
            showAlerts: exportOptions.indexOf("showAlerts") >= 0,
            showCompanyEvents: exportOptions.indexOf("showCompanyEvents") >= 0,
            showCorporateRatingInformation: exportOptions.indexOf("showCorporateRatingInformation") >= 0,
            showCoverPage: exportOptions.indexOf("showCoverPage") >= 0,
            showDocuments: exportOptions.indexOf("showDocuments") >= 0,
            showResolvedAndArchivedCases: $("#cb-show-closed-cases").is(":checked")
        }

        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            url: "/Company/ExportPdf",
            data: JSON.stringify(data)
        })
        .done(function (response, textStatus, jqXHR) {
            $("#export-icon").show();
            $("#export-loading").hide();
            $("#export-button").prop('disabled', false);

            if (response.FileName.length > 0) {
                window.location = "/Export/Download?filename=" +
                    response.FileName +
                    "&fileDownloadName=CompanyProfile_" +
                    utils.normalizeFileName(window.i_companyName) +
                    ".pdf";
            } else {
                utils.quickNotification("Failed: Error occurred exporting pdf. Please try again or contact administrator.", "error");
            }
        });
    };

    return {
        goToExportPage: goToExportPage,
        toogleSelect: toogleSelect,
        onChangeSelectOption: onChangeSelectOption
    }
}();
