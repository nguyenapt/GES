$(function () {
    if (typeof (_initialHeading) !== "undefined")
        $("#initialHeading").html(_initialHeading);

    var tblId = "tblcases";
    var grid = $("#" + tblId);

    var postUrl = "/company/GetAdditionalIncident";
    var colNames = ["Id", "Entry date", "Issue", "Location", "Norm/Theme", "Engagement status"];

    var gridColModel = [
        { name: "Id", hidden: true, sortable: false, key: true },
        { name: "EntryDate", search: false, formatter: utils.dateFormatter, align: "center", width: 65, title: false },
        {
            name: "IssueName", search: false, title: false,
            formatter: function (cellvalue, options, rowObject) {
                return utils.genCaseReportLink(cellvalue, rowObject);
            }
        },
        { name: "Location", search: false, formatter: utils.renderTextWithNAValue, width: 90, title: false, align: "center" },
        { name: "ServiceEngagementThemeNorm", search: false, formatter: utils.renderTextWithNAValue, width: 92, title: false },
        { name: "Recommendation", search: false, align: "center", formatter: utils.renderTextWithNAValue, width: 105, title: false },
        //{ name: "Conclusion", search: false, width: 95, formatter: utils.renderTextWithNAValue, title: false, align: "center" },
        //{
        //    name: "Confirmed", search: false, width: 58, title: false,
        //    formatter: function (cellvalue, options, rowObject) {
        //        return utils.renderConfirmedVisual(cellvalue);
        //    }, align: "center"
        //}
    ];

    $("a[data-toggle=\"tab\"]").on("shown.bs.tab", function () {
        $(window).resize();
    });

    $.jgrid.defaults.responsive = true;
    grid.bind("jqGridLoadComplete",
        function (e, items, orgClickEvent) {
            $(window).resize();

            // odd, even row
            $("#tblcases tbody>tr.jqgrow:even").addClass("jqgrid-row-even");
            $("#tblcases tbody>tr.jqgrow:odd").removeClass("jqgrid-row-even");
        });

    grid.jqGrid({
        url: postUrl,
        postData: { caseProfileId: typeof (_caseReportId) !== "undefined" ? _caseReportId : 0 },
        height: "100%",
        toppager: true,
        pager: $("#tblcases-pager"),
        rowNum: 10,
        rowList: [5, 10, 20, 999],
        datatype: "json",
        mtype: "post",
        colNames: colNames,
        colModel: gridColModel,
        sortname: "none",
        sortorder: "asc",
        loadComplete: function () {
            $(window).resize();
            var total = grid.jqGrid('getGridParam', 'records');
            if (total == 0) {
                $("#companyRelatedItemsTableContainer").hide();
            } else {
                $(".no-company-related-items-msg").hide();
            }

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
                }
            }
        }
    });

    var summaryModalTemplate = $("#summary-modal-template");
    if (summaryModalTemplate.length) {
        var summarySource = summaryModalTemplate.html();
        window.summaryTpl = Handlebars.compile(summarySource);

        /*-- Alerts --*/
        var postUrlAlerts = "/Alert/GetAlerts";
        var tblIdAlerts = "tblalerts";
        var gridAlerts = $("#" + tblIdAlerts);
        var alertSummaryModals = "";

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
                    formatter: function (cellvalue, options, rowObject) {
                        alertSummaryModals += utils.genSummaryModalContent("alert", rowObject, summaryTpl, window.i_companyName);
                        return utils.genSummaryLink("alert", rowObject) + utils.genAlertLink(cellvalue, rowObject); //cellvalue;
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

            beforeSelectRow: function (rowid, e) {
                return false;
            },
            loadComplete: function (data) {

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
    }

    var eiEvents = $("#ei-events");

    if (eiEvents.length) {
        // Events
        eiEvents.eventify({
            locale: "en",
            limitedCharacter: 60
        });

        // Linkify
        eiEvents.linkify({
            target: "_blank"
        });
    }

    $(".go-back-unsubscribed").on("click", function () {
        utils.goBackHistory();
    });

    $(window).resize(function () {
        setTimeout(
            function () {
                var containerWidth = $(".sdg-thumbnail").parent().width();
                var itemSize = containerWidth / 4 - 3;
                $(".sdg-thumbnail").css("width", itemSize);
                $(".sdg-thumbnail img").each(function () {
                    $(this).css({
                        "width": "auto",
                        "maxWidth": itemSize,
                        "maxHeight": itemSize
                    });
                });
            },
            500
        );
    });

    if (window._pieColors !== undefined && window._pieData !== undefined) {
        chartCreator.createPieChart("kpi-chart-container", window._pieColors, window._pieData);

        window.onresize = function () {
            chartCreator.createPieChart("kpi-chart-container", window._pieColors, window._pieData);
        };
    }

    caseProfileExportPdf.onChangeSelectOption();

    utils.shortenIfPassLimit(0, ".ungp-methodology-desc");
});

function showMyEndorsement(caseReportId) {
    window.SignedUpGesCaseReportId = caseReportId;
    $("#SignUpTypeModal").modal("show");
}

$("#btn-signup-type").on("click", function (e) {
    showMyEndorsement(_caseReportId);
});

$(".btn-signup-type").on("click", function (e) {
    var self = $(this);

    var isSignOff = self.hasClass("btn-sign-off");
    var activelySignUp = self.hasClass("btn-signup-active");
    var data = {
        isSignUp: !isSignOff,
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
                //utils.quickNotification("Endorsed successfully");
                window.location.reload(false);
            } else {
                utils.quickNotification(response.meta.error, "error");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            utils.quickNotification("Failed: Error occurred updating sign up", "error");
        });

});

var caseProfileExportPdf = function () {
    var toggleButtonName = function () {
        var isSelectAll = $("input[name='case-profile-export']").length === $("input[name='case-profile-export']:checked").length;
        if (isSelectAll) {
            $("#btn-export-select").html("Unselect all");
        } else {
            $("#btn-export-select").html("Select all");
        }
    },

        validateExportButton = function () {
            var isUnselectAll = $("input[name='case-profile-export']:checked").length === 0;
            $("#btn-case-profile-export").prop('disabled', isUnselectAll);
        },

        toogleSelect = function (groupName) {
            var isSelectAll = $("input[name='case-profile-export']").length === $("input[name='case-profile-export']:checked").length;
            $("input[name='" + groupName + "']:checkbox").each(function () {
                this.checked = !isSelectAll;
            });
            toggleButtonName();
            validateExportButton();
        },

        onChangeSelectOption = function () {
            $("input[name='case-profile-export']").change(function () {
                toggleButtonName();
                validateExportButton();
            });
        },

        goToExportPage = function (groupName) {
            $("#export-icon").hide();
            $("#export-loading").show();
            $("#export-button").prop('disabled', true);

            var exportOptions = $("input[name='" + groupName + "']:checked").map(function () {
                return this.value;
            }).get();

            var data = {
                caseProfileId: window.i_caseProfileId,
                showCoverPage: exportOptions.indexOf("showCoverPage") >= 0,
                showCompanyInfo: exportOptions.indexOf("showCompanyInfo") >= 0,
                showCaseInfoBusinessConduct: true,//exportOptions.indexOf("showCaseInfoBusinessConduct") >= 0,
                showStatistic: exportOptions.indexOf("showStatistic") >= 0,
                showCompanyEvents: exportOptions.indexOf("showCompanyEvents") >= 0,
                showSummary: exportOptions.indexOf("showSummary") >= 0,

                showSummaryMaterialRisk: exportOptions.indexOf("showSummaryMaterialRisk") >= 0,
                showClosingDetail: exportOptions.indexOf("showClosingDetail") >= 0,

                showAlerts: exportOptions.indexOf("showAlerts") >= 0,
                showDescription: exportOptions.indexOf("showDescription") >= 0,
                showConclusion: false,//exportOptions.indexOf("showConclusion") >= 0,
                showGesCommentary: exportOptions.indexOf("showGesCommentary") >= 0,
                showLatestNews: exportOptions.indexOf("showLatestNews") >= 0,
                showEngagementInformation: exportOptions.indexOf("showEngagementInformation") >= 0,
                showDiscussionPoint: exportOptions.indexOf("showDiscussionPoint") >= 0,
                showOtherStakeholder: exportOptions.indexOf("showOtherStakeholder") >= 0,
                showKPI: exportOptions.indexOf("showKPI") >= 0,
                showGuidelinesAndConventions: exportOptions.indexOf("showGuidelinesAndConventions") >= 0,
                showConfirmationDetails: false,//exportOptions.indexOf("showConfirmationDetails") >= 0,
                showReferences: exportOptions.indexOf("showReferences") >= 0,
                showCompanyDialogue: exportOptions.indexOf("showCompanyDialogue") >= 0,
                showSourceDialogue: exportOptions.indexOf("showSourceDialogue") >= 0,
                showCompanyRelatedItems: exportOptions.indexOf("showCompanyRelatedItems") >= 0,     
                
                showGesContactInformation: exportOptions.indexOf("showGesContactInformation") >= 0,                
                showAdditionalDocuments: exportOptions.indexOf("showAdditionalDocuments") >= 0                
            }

            $.ajax({
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                url: "/Company/ExportCaseProfilePdf",
                data: JSON.stringify(data)
            })
                .done(function (response, textStatus, jqXHR) {
                    $("#export-icon").show();
                    $("#export-loading").hide();
                    $("#export-button").prop('disabled', false);

                    if (response.FileName.length > 0) {
                        window.location = "/Export/Download?filename=" +
                            response.FileName +
                            "&fileDownloadName=CaseProfile_" +
                            utils.normalizeFileName(window.i_companyName) + "_" +
                            utils.normalizeFileName(window.i_caseProfileName).toLowerCase() +
                            ".pdf";
                    } else {
                        utils.quickNotification("Failed: Error occurred exporting pdf. Please try again or contact administrator.", "error");
                    }
                });
        },
        goToDownLoadDialoguePage = function () {
            $("#download-company-dialogue-icon").hide();
            $("#download-company-dialogue-loading").show();
            $("#submit-download").prop('disabled', true);
            
            var fromDate = $("#fromdate").datepicker('getDate');
            var toDate = $("#todate").datepicker('getDate');
            var addTerm = false;

            var data = {
                caseProfileId: window.i_caseProfileId,
                fromDate: fromDate,
                toDate: toDate,
                addTerm: addTerm
            }

            $.ajax({
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                url: "/Company/CompanyDialogueDownloadPdf",
                data: JSON.stringify(data)
            })
                .done(function (response, textStatus, jqXHR) {
                    $("#download-company-dialogue-icon").show();
                    $("#download-company-dialogue-loading").hide();
                    $("#submit-download").prop('disabled', false);

                    if (response.FileName.length > 0) {
                        window.location = "/Export/Download?filename=" +
                            response.FileName +
                            "&fileDownloadName=Report_" +
                            utils.normalizeFileName(window.i_companyName) +
                            "_company_dialogue_report" +
                            ".pdf";
                    } else {
                        utils.quickNotification("Failed: Error occurred exporting pdf. Please try again or contact administrator.", "error");
                    }
                });
        }

    return {
        goToExportPage: goToExportPage,
        goToDownLoadDialoguePage,
        toogleSelect: toogleSelect,
        onChangeSelectOption: onChangeSelectOption
    }
}();
var chartCreator = (function () {
    var createPieChart = function (containerId, pieColors, pieData) {
        var containerWidth = $("#" + containerId).width();

        var labelWidth = (containerWidth - 150) / 2;

        Highcharts.chart(containerId,
            {
                chart: {
                    type: "pie"
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
                        dataLabels: {
                            enabled: true,
                            format: "<b>{point.name}</b>",
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || "black",
                                width: labelWidth + "px",
                                whiteSpace: "nowrap",
                                overflow: "hidden",
                                textOverflow: "ellipsis"
                            }
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
