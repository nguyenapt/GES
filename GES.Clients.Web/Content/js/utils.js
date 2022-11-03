var utils = {
    setUpContentBlockAnimation: function()
    {
        $('.ges-content-block > .header > .title').removeClass("fadeIn animated").addClass("fadeIn animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass("fadeIn animated");
        });

        $('.ges-content-block > .ges-content').removeClass("fadeIn animated").addClass("fadeIn animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass("fadeIn animated");
        });

        $('.ges-content-read-more').removeClass("bounceInLeft animated").addClass("bounceInLeft animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass("bounceInLeft animated");
        });
    },
    quickNotification: function(msg, type, timeOut) {
        if (typeof (type) === "undefined") type = "success"; // success, info, warning, error
        if (typeof (timeOut) === "undefined") timeOut = 4000;

        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "100",
            "hideDuration": "1000",
            "timeOut": timeOut,
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",
            bodyOutputType: "trustedHtml"
        }
        toastr[type](msg);
    },
    openLinkInNewTab: function(link) {
        var win = window.open(link);
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert("Please allow popups for this website.");
        }
    },
    genCaseReportHref_Old: function (caseId, caseEngagementTypeId, companyId, caseEngagementTypeCategoriesId, forumMessageTreeId, parentForumMessageId) {
        var href = utils.genBaseLinksToOldSite() + "engagement_forum/process.aspx?I_GesCaseReports_Id=" + caseId
            + "&I_Companies_Id=" + companyId
            + "&I_EngagementTypes_Id=" + caseEngagementTypeId
            + "&I_EngagementTypeCategories_Id=" + caseEngagementTypeCategoriesId
            + "&G_ForumMessages_Tree_Id=" + forumMessageTreeId
            + "&ParentG_ForumMessages_Id=" + parentForumMessageId;
        return href;
    },
    genCompanyHref: function(companyId) {
        var href = "/Company/Profile/" + companyId;
        return href;
    },
    getPopoverConfig: function(title, content) {
        return {
            html: true,
            animation: true,
            title: title,
            content: content,
            container: "body",
            placement: function (context, source) {
                var offset = $(source).offset();
                if (offset.left > 300) {
                    return "left";
                }
                return "right";
            },
            trigger: "hover"
        }
    },
    clearArray: function(myArray) {
        if (myArray != null) {
            if (window.portfolioIds.length > 0) {
                while (myArray.length > 0) {
                    myArray.pop();
                }
            }
        }
    },
    setTooltipsOnColumnHeader: function(grid, iColumn, text) {
        var thd = $("thead:first", grid[0].grid.hDiv)[0];
        $("tr.ui-jqgrid-labels th:eq(" + iColumn + ")", thd).attr("title", text);
    },
    genMyEndorsementLink: function(status, rowObject, subGridId) {
        return "<a id=\"my-endorsement-link-" + rowObject.Id + "\" href=\"#\" onclick=\"showMyEndorsement(" + rowObject.Id + "," + subGridId + ")\">" + status + "</a>" +
            "<i id=\"my-endorsement-loading-icon-" + rowObject.Id + "\" class=\"fa fa-spin fa-circle-o-notch\" style=\"display: none \">";
    },
    getParameterByName: function(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    genStandardLink: function(id, conclusionText) {
        return "<a target='_blank' href=\"/Standard/Details/" +
            id + "\">" + conclusionText + "</a>";
    },
    genStandardLink_Old: function(id, templateId, conclusionText, companyId) {
        return "<a target='_blank' href='" +
            utils.genBaseLinksToOldSite() + "ges/case_profile.aspx?G_ReportingTemplates_Id=" +
            templateId + "&I_Companies_Id=" +
            companyId + "&I_GesCaseReports_Id=" +
            id + "'>" + conclusionText + "</a>";
    },
    createParamStr: function(obj) {
        var output = [];
        for (var prop in obj) {
            if (obj.hasOwnProperty(prop)) {
                var value = Array.isArray(obj[prop]) ? obj[prop].join(",") : obj[prop];
                output.push(prop + '=' + value);
            }
        }
        return output.join('&');
    },
    submitAsyncForm: function($formToSubmit, fnSuccess, fnError) {
        if (!$formToSubmit.valid())
            return false;

        $.ajax({
            type: $formToSubmit.attr("method"),
            url: $formToSubmit.attr("action"),
            data: $formToSubmit.serialize(),

            success: fnSuccess,
            error: fnError

        });
        return true;
    },
    initMultiSelect: function(cssSel, allDefaultText, styleClasses, size) {
        if (typeof (allDefaultText) === "undefined") allDefaultText = "All values";
        if (typeof (styleClasses) === "undefined") styleClasses = "btn-default btn-white";
        if (typeof (size) === "undefined") size = 10;

        $(cssSel).selectpicker({
            style: styleClasses,
            noneSelectedText: allDefaultText,
            deselectAllText: "Reset",
            size: size
        });
    },
    shortenIfPassLimit: function(limit, selector) {
        $.each($(selector), function (key, item) {
            if (item.innerHTML.length > limit) {
                $(item).shorten({
                    showChars: limit,
                    moreText: "<i class='fa fa-arrow-down'></i>&nbsp;Read more",
                    lessText: "<i class='fa fa-arrow-up'></i>&nbsp;Read less"
                });
                var parent = $(item).parent();
                if (parent.hasClass("dashboard-milestone-item") || parent.hasClass("dashboard-news-item")) {
                    parent.addClass("cursor-pointer");
                }
            }
        });
    },
    isValidISIN: function(isin) {
        // basic pattern
        var regex = /^([a-zA-Z_-_]{2})([0-9A-Za-z]{9})([0-9])$/;
        var match = regex.exec(isin);
        if (match === null) return false;
        if (match.length !== 4) return false;

        // if start with C_ >>> no checksum
        if (isin.indexOf("C_") === 0)
            return true;

        // validate the check digit
        var result = match[3] == utils.calcISINCheck(match[1] + match[2]);
        return result;
    },
    /**
     * Calculates a check digit for an isin
     * @param {String} code an ISIN code with country code, but without check digit
     * @return {Integer} The check digit for this code
     */
    calcISINCheck: function(code) {
        var conv = '';
        var digits = '';
        var sd = 0;
        // convert letters
        for (var i = 0; i < code.length; i++) {
            var c = code.charCodeAt(i);
            conv += (c > 57) ? (c - 55).toString() : code[i]
        }
        // group by odd and even, multiply digits from group containing rightmost character by 2
        for (var i = 0; i < conv.length; i++) {
            digits += (parseInt(conv[i]) * ((i % 2) == (conv.length % 2 != 0 ? 0 : 1) ? 2 : 1)).toString();
        }
        // sum all digits
        for (var i = 0; i < digits.length; i++) {
            sd += parseInt(digits[i]);
        }
        // subtract mod 10 of the sum from 10, return mod 10 of result 
        return (10 - (sd % 10)) % 10;
    },
    renderDevGradeVisual: function(value) {
        var classes = "", title = "";
        switch (value) {
        case 1:
            classes = "high";
            title = "High";

            break;
        case 2:
            classes = "medium";
            title = "Medium";
            break;
        case 3:
            classes = "low";
            title = "Low";
            break;
        }

        return value === 0 ? "<div title='N/A'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>"
            : "<div class='tooltip-hint dev-grade-content' data-tooltip-title='Development' data-tooltip-content='development-hint'><img src='/Content/img/elements/dev-" + classes + ".svg' title='" + title + "' /></div>";
    },
    renderConfirmedVisual: function (value) {        
        return value ? "<img src='/Content/img/Tick.png' width='24' />" : "<div class='jqgrid-cell-not-available-style'>N.A</div>";
    },
    genNorm: function(normText, keywordMatchedNorm) {
        var classes = keywordMatchedNorm ? "sr-norm matched-norm" : "sr-norm";
        return "<span class='" + classes + "'>" + normText + "</span>";
    },
    genSummaryLink: function(summaryType, rowObject) {
        var text = "";
        var watchStype = "";
        switch (summaryType) {
        case "case":
            text = "case profile";
            break;
        case "alert":
                text = "alert";
                if (rowObject != null && rowObject.Notices !== 'undefined' && rowObject.Notices != null && rowObject.Notices.length > 0) {
                    watchStype = "infomation-icon";
                }
            break;
        }
        return "<a data-toggle='modal' class='anchor-pointer' data-target='#" + summaryType + "SummaryModal-" + rowObject.Id + "' title='View summary of this " + text + "'><i class='fa fa-lg fa-info-circle " + watchStype + "'></i>&nbsp;&nbsp;</a>";
    },
    genCaseReportLink: function (caseName, rowObject) {
        return this.genCaseReportLink_New(caseName, rowObject);
    },
    genCaseReportLink_New: function (caseName, rowObject) {
        var classes = rowObject.KeywordMatched ? "sr-caseprofile matched-kw" : "sr-caseprofile";
        var showKeywords = utils.getParameterByName("showKeywords");

        var html = "";
        if (rowObject.IsUnsubscribed) {
            classes += " un-subscribed";
            html = "<a href='#' class='" + classes + "' onclick=\"Open(" +"'"+ rowObject.ServiceEngagementThemeNorm + "'" + ")\" >" + caseName + "</a>";
        } else {
            html = "<a class='" + classes + "' target='_blank' href=\"/Company/CaseReport/" + rowObject.Id + "\">" + caseName + "</a>";
        }

        if (showKeywords === "true") {
            html += "<br /><span class='case-all-keywords'>Keywords: " + rowObject.Keywords + "</span>";
        }
        return html;
    },
    genCaseReportLink_Old: function(caseName, rowObject) {
        var classes = rowObject.KeywordMatched ? "sr-caseprofile matched-kw" : "sr-caseprofile";
        var showKeywords = utils.getParameterByName("showKeywords");

        var html = "<a class='" + classes + "' target='_blank' href=\"" +
            utils.genCaseReportHref_Old(rowObject.Id, rowObject.EngagementTypeId, rowObject.CompanyId, rowObject.EngagementTypeCategoriesId, rowObject.ForumMessagesTreeId, rowObject.ParentForumMessagesId) + "\">" +
            caseName + "</a>";
        if (showKeywords == "true") {
            html += "<br /><span class='case-all-keywords'>Keywords: " + rowObject.Keywords + "</span>";
        }
        return html;
    },
    genAlertLink: function(cellvalue, rowObject ) {
        var html = "";
        var classes = rowObject.KeywordMatched ? "sr-caseprofile matched-kw" : "sr-caseprofile";
        html = "<a class='" + classes  + "' href='/Alert/AlertDetails/" + rowObject.Id +"'>" + cellvalue +  "</a>";  
        return html;
    },
    genAlertLink_Old: function(alertName, rowObject) {
        var html = "<a class='' target='_blank' href=\"" +
            utils.genBaseLinksToOldSite() + "alert/article.aspx?I_NaArticles_Id=" +
            rowObject.Id + "&I_Companies_Id=" +
            rowObject.CompanyId + "&ShowResults=true\">" + alertName + "</a>";
        return html;
    },
    genAlertResultLink: function(isin, numAlerts) {
        return numAlerts;
    },
    genAlertResultLink_Old: function(isin, numAlerts) {
        return numAlerts > 0 ? "<b><a target='_blank' class='cell-alert-result' title='' href=\"" +
            utils.genBaseLinksToOldSite() + "alert/overview.aspx?ShowResults=true&s_Isin=" +
            isin + "\">" + numAlerts + "</a></b>" :
            numAlerts;
    },
    renderResProgGradeVisual: function(value, type) {
        var classes = "", commentary = "";
        var tooltipTitle = type === "progress" ? "Progress" : "Response";
        var tootipContent = type === "progress" ? "progress-hint" : "response-hint";
        switch (value) {
        case 1:
            classes = type === "progress" ? "a-none" : "o-none";
            break;
        case 2:
            classes = type === "progress" ? "a-poor" : "o-poor";
            break;
        case 3:
            classes = type === "progress" ? "a-standard" : "o-standard";
            break;
        case 4:
            classes = type === "progress" ? "a-good" : "o-good";
            break;
        case 5:
            classes = type === "progress" ? "a-excellent" : "o-excellent";
            break;
        }

        return value === 0 ? "<div title='N/A'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div>"
            : "<img class='tooltip-hint res-pro-indicators' data-tooltip-title='" + tooltipTitle + "' data-tooltip-content='" + tootipContent + "' src='/Content/img/elements/" + classes + ".svg' />";
    },
    genSummaryModalContent: function(summaryType, rowObject, tpl, companyName) {
        var typeText = "";
        var context = {};
        switch (summaryType) {
        case "case":
            typeText = "Case Profiles";
            context = {
                id: rowObject.Id,
                title: rowObject.IssueName,
                summary: rowObject.Description,
                companyName: companyName,
                type: summaryType,
                typeText: typeText,
                date: utils.dateFormatter(rowObject.EntryDate) === "" ? "Published: N/A" : "Published: " + utils.dateFormatter(rowObject.EntryDate)
            };
            context.date += utils.dateFormatter(rowObject.LastModified) === "" ? "" : " | Last modified: " + utils.dateFormatter(rowObject.LastModified);
            break;
        case "alert":
            typeText = "Alerts";
            context = {
                id: rowObject.Id,
                title: rowObject.Heading,
                summary: rowObject.Summary,
                companyName: companyName,
                type: summaryType,
                typeText: typeText,
                date: utils.dateFormatter(rowObject.LastModified) === "" ? "Alert date: N/A" : "Alert date: " + utils.dateFormatter(rowObject.LastModified),
                source: "(" + rowObject.Source + (utils.dateFormatter(rowObject.Date) === "" ? "" : " " + utils.dateFormatter(rowObject.Date)) + ")",
                notices: rowObject.Notices,
                alertType: rowObject.AlertType
            };
            break;
        }
        var html = tpl(context);
        return html;
    },
    /*
    Must include:
    @section globalVar {
        var oldClientsSiteUrl = '@WebConfigurationManager.AppSettings["oldClientsSiteUrl"]';
    }
    into the destination view before using below functions
    */
    genBaseLinksToOldSite: function() {
        return window.oldClientsSiteUrl + "en-US/client/";
    },
    genCompanyLink_Old: function(companyId, companyName) {
        return "<b><a target='_blank' class='cell-company-name' title='' href=\"" +
            utils.genBaseLinksToOldSite() + "engagement_forum/company.aspx?I_Companies_Id=" +
            companyId + "\">" + companyName + "</a></b>";
    },
    genCompanyLink: function(companyId, companyName) {
        return "<b><a class='cell-company-name' href=\"/Company/Profile/" +
            companyId + "\">" + companyName + "</a></b>";
    },
    genNumCasesHrefAction: function(numCases) {
        return numCases > 0 ? "<b><a target='_blank' class='cell-numCases-action' title=''>" + numCases + "</a></b>" : numCases;
    },
    dateFormatter: function(cellvalue, options, rowObject) {
        if (cellvalue) {
            var result = $.format.date(utils.convertNetDate(cellvalue), "yyyy-MM-dd");
            if (rowObject != null && rowObject.IsUnsubscribed !== 'undefined' && rowObject.IsUnsubscribed) {
                result = "<div class='un-subscribed'>" + result + "</div>";
            }
            return result;
        } else {
            return "";
        }
    },
    dateWithTimeFormatter: function(cellvalue, options, rowObject) {
        if (cellvalue) {
            var result = $.format.date(utils.convertNetDateWithTimeUTCToLocalTime(cellvalue), "yyyy-MM-dd HH:mm a");
            if (rowObject != null && rowObject.IsUnsubscribed !== 'undefined' && rowObject.IsUnsubscribed) {
                result = "<div class='un-subscribed'>" + result + "</div>";
            }
            return result;
        } else {
            return "";
        }
    },
    convertNetDate: function(netDate) {
        var re = /-?\d+/;
        var m = re.exec(netDate);
        var d = new Date(parseInt(m[0]));
        return d;
    },
    convertNetDateWithTimeUTCToLocalTime: function(netDate) {
        var re = /-?\d+/;
        var m = re.exec(netDate);
        var d = new Date(parseInt(m[0]));
        var a = (d.getHours() > 12)? "PM":"AM";
        var utcDate = d.getDay() + "/" + d.getMonth() + "/" + d.getFullYear() + " " + d.getHours() + ":" + d.Milestone + " " + a + " UTC";
        var returnDate = new Date(utcDate);
        
        return moment.utc(parseInt(m[0])).local();;
    },
    goBackHistory: function() {
        if (document.referrer !== "") {
            window.location = document.referrer;
        } else {
            window.location = "/";    
        }
    },
    renderTextWithNAValue: function (value, options, rowObject) {
        var result = value ? value : "<div class='jqgrid-cell-not-available-style'>N.A</div>";
        if (rowObject != null && rowObject.IsUnsubscribed !== 'undefined' && rowObject.IsUnsubscribed) {
            result = "<div class='un-subscribed'>" + result + "</div>";
        }
        return result;
    },

    normalizeFileName: function (value) {
        if (value) {
            return value.trim().replace(/[^a-z0-9\s]/gi, '').replace(/ /g, "_");    
        }
        return "";
    }
};

var dateDiff = {
    inDays: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },
    inWeeks: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },
    inMonths: function (d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();

        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },
    inYears: function (d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
}

$(function () {
    $(".btn-print").on("click", function(e) {
        window.print();
    });
});

var toBool = function (str) {
    if (typeof str === "boolean") return str;
    var strLower = str.toLowerCase();
    if (strLower === "yes" || strLower === "true")
        return true;
    return false;
}

//Takes css classes assigned to each column in the jqGrid colModel 
//and applies them to the associated header.
var applyClassesToSubgridHeaders = function (grid) {
    // how to use: Applies the classes to the headers once the grid configuration is complete.
    //applyClassesToSubgridHeaders(grid);

    // Use the passed in grid as context, 
    // in case we have more than one table on the page.
    var trHead = jQuery(".ui-subgrid thead:first tr", grid.hdiv);
    var colModel = grid.getGridParam("colModel");

    for (var iCol = 0; iCol < colModel.length; iCol++) {
        var columnInfo = colModel[iCol];
        if (columnInfo.classes) {
            var headDiv = jQuery("th:eq(" + iCol + ")", trHead);
            headDiv.addClass(columnInfo.classes);
        }
    }
};

//function genAlertLink_Old(companyId, companyName) {
//    return "<b><a target='_blank' class='cell-company-name' title='' href=\"" +
//        genBaseLinksToOldSite() + "engagement_forum/company.aspx?I_Companies_Id=" +
//        companyId + "\">" + companyName + "</a></b>";
//}
 

$.widget("custom.catcomplete", $.ui.autocomplete, {
    _create: function () {
        this._super();
        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
    },
    _renderMenu: function (ul, items) {
        var that = this,
          currentCategory = "";
        $.each(items, function (index, item) {
            var li;
            if (item.category != currentCategory) {
                ul.append("<li class='ui-autocomplete-category'><span>" + item.category + "</span></li>");
                currentCategory = item.category;
            }
            li = that._renderItemData(ul, item);
            li.addClass("li-autocomplete-company-issue");
            if (item.category) {
                li.attr("aria-label", item.category + " : " + item.label);
            }
        });

        $(".li-autocomplete-company-issue").highlight(window.searchStr);
    }
});
