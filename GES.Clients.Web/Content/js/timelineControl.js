var timelineControl = (function () {
    var timelineContainer;
    var timelineRuler;
    var timelineRulerWidth;
    var daySpace;
    var lastTimelineItem;
    var timelineData;
    var defaultDynamicBottom;
    var dynamicBottom;
    var currentTimelineElementBottom;
    var lastDayPointAlignment;
    var defaultTimelineElementWidth;
    var lastDateDrew;

    var init = function (data) {
        currentTimelineElementBottom = {
            Date: 20,
            TypeName: 70,
            Milestone: 100,
            StarIcon: 70
        };

        defaultTimelineElementWidth = 40;
        dynamicBottom = 0;
        defaultDynamicBottom = 14;
        timelineData = data;
        timelineContainer = $("#timelineContainer");
        timelineRuler = $("#timelineRuler");
        timelineRulerWidth = timelineRuler.width();

        $(window).resize(function () {
            reDraw();
        });

        draw();
    }

    var draw = function () {
        if (timelineData != null && timelineData.length < 1) {
            alert("No timeline data to display");
        }

        var firstTimelineItem = timelineData[0];
        lastTimelineItem = timelineData[timelineData.length - 1];

        chooseRuler();

        var daysCount = dateDiff.inDays(firstTimelineItem.DateTime, lastTimelineItem.DateTime);
        daySpace = timelineRulerWidth / daysCount;

        $.each(timelineData,
            function (index, timelineItem) {
                var dayIndex = dateDiff.inDays(timelineItem.DateTime, lastTimelineItem.DateTime);

                createTimelinePoint(timelineItem, dayIndex, index);
            });

        // Bind popup event
        $(".tooltip-hint, .timeline-element").each(function (index, tooltip) {
            var title = $(tooltip).attr("data-tooltip-title");
            var content = $("#" + $(tooltip).attr("data-tooltip-content")).html();
            $(tooltip).popover(utils.getPopoverConfig(title, content));
        });
    };

    var chooseRuler = function () {
        // Archived & Resolved case, so the Arrow of the Ruler is removed
        if (lastTimelineItem.PointProperty.SpecificType == 3) {
            timelineRuler.removeClass("timeline-ruler").addClass("archived-resolved-timeline-ruler");
        }
    };

    var createTimelinePoint = function (timelineItem, dayIndex, timelineItemIndex) {
        var dayPoint = timelineRulerWidth - (daySpace * dayIndex);
        var dayPointAlignment = dayPoint - defaultTimelineElementWidth;

        measureTimelineElementBottoms(timelineItemIndex, dayPointAlignment);

        // Draw a day pole
        if (timelineItem != lastTimelineItem) {
            createDayPole(dayPoint);
        }

        // Draw Tilte
        switch (timelineItem.PointProperty.CommonType) {
            case 0: // Recommendation
                createTimelineElement("type-name recommendation-type-name",
                    dayPointAlignment,
                    currentTimelineElementBottom.TypeName,
                    timelineItem.PointProperty.TypeName);
                break;
            case 1: // Conclusion
                createTimelineElement("type-name conclusion-type-name",
                    dayPointAlignment,
                    currentTimelineElementBottom.TypeName,
                    timelineItem.PointProperty.TypeName);
                break;
            case 2: // Milestone
                createTimelineElement("type-name milestone-type-name",
                    dayPointAlignment,
                    currentTimelineElementBottom.Milestone,
                    timelineItem.PointProperty.TypeName);

                createTimelineElement("star-icon",
                    dayPointAlignment,
                    currentTimelineElementBottom.StarIcon,
                    "<i class='fa fa-star' aria-hidden='true'></i>",
                    timelineItem.PointProperty.TypeName,
                    timelineItem.PointProperty.Tooltip);
                break;
            default:
                break;
        }

        // Draw Date (Month/Year)
        var timeLineDate = moment(timelineItem.DateTime).format("DD/MM/YYYY");
        if(!(lastDateDrew != null && lastDateDrew === timeLineDate) ){
            createTimelineElement("date", dayPointAlignment, currentTimelineElementBottom.Date, moment(timelineItem.DateTime).format("YYYY-MM"));
        }
        lastDateDrew = timeLineDate;

        // Set last day point
        lastDayPointAlignment = dayPointAlignment;
    };

    var setBottoms = function (bottom) {
        currentTimelineElementBottom.Date = currentTimelineElementBottom.Date - bottom;
        currentTimelineElementBottom.TypeName = currentTimelineElementBottom.TypeName + bottom;
        currentTimelineElementBottom.Milestone = currentTimelineElementBottom.Milestone + bottom;
        currentTimelineElementBottom.StarIcon = currentTimelineElementBottom.StarIcon + bottom;
    };

    var measureTimelineElementBottoms = function (timelineItemIndex, dayPointAlignment) {
        if (dayPointAlignment - lastDayPointAlignment <= defaultTimelineElementWidth) {
            if (dynamicBottom == 0) {
                dynamicBottom = defaultDynamicBottom;
                setBottoms(defaultDynamicBottom);

            } else {
                dynamicBottom = 0;
                setBottoms(-defaultDynamicBottom);
            }
        } else {
            if (dynamicBottom == defaultDynamicBottom) {
                setBottoms(-defaultDynamicBottom);
                dynamicBottom = 0;
            }
        }
    };

    var createTimelineElement = function (classes, left, bottom, content, tootTipTitle, toolTipContent) {
        if (toolTipContent == null) {
            timelineContainer.append(
                "<span class='timeline-element " +
                classes +
                "' style='left:" +
                left +
                "px;bottom:" +
                bottom +
                "px'>" +
                content +
                "</span>");
        } else {
            timelineContainer.append(
                "<span data-tooltip-title='" + tootTipTitle + "' data-tooltip-content='"+ toolTipContent +"' class='tooltip-hint timeline-element " +
                classes +
                "' style='left:" +
                left +
                "px;bottom:" +
                bottom +
                "px'>" +
                content +
                "</span>");
        }        
    };

    var createDayPole = function (dayPoint) {
        timelineContainer.append("<div class='timeline-element day-pole' style='left: " + dayPoint + "px'></div>");
    };

    var reDraw = function () {
        $(".timeline-element ").remove();
        timelineRulerWidth = timelineRuler.width();
        draw();
    };

    return {
        init: init
    };
})();

$(document).ready(function () {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Company/GetTimeLines?gesCaseReportId=" + _caseReportId
    })
        .done(function (response, textStatus, jqXHR) {
            if (response.length > 0) {
                $.each(response,
                    function (index, item) {
                        item.DateTime = new Date(moment(item.DateTime).format());
                    });
                timelineControl.init(response);                
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            utils.quickNotification("Failed: Error occurred getting data for timeline", "error");
        });
});