//! eventify.js
//! version : 1.0.1
//! author: www.github.com/dellax/
//! license : MIT
//! https://github.com/dellax/eventify

/*----- Utility function -----*/

/* Function: trim str, cut by words */
function trimToLength(str, charLimit) {
    var words = jQuery.trim(str).substring(0, charLimit).split(" ");
    if (words.length > 1)
        return words.slice(0, -1).join(" ") + "...";
    return words.slice(0, 1).join(" ");
}

function cutToLimit(str, n) {
    if (str.length <= n)
        return str;

    var cutat = str.lastIndexOf(' ', n - 1);
    if (cutat !== -1)
        str = str.substring(0, cutat) + '...';
    return str;
}

/* Function: random color hex generator */
function genRandomColor() {
    return '#' + (Math.random() * 0xFFFFFF << 0).toString(16);
}

//////////////////////////////////////////////////////////////////////
//! eventify.js //////////////////////////////////////////////////////

function mainEventify(settings) {
    // load user settings
    moment.locale(settings.locale);
    if (settings.theme != "") {
        settings.theme = '-' + settings.theme;
    }

    function sortByStartTimes(a, b) {
        if (a.start < b.start) return -1;
        if (a.start > b.start) return 1;
        return 0;
    }
    
	var data = $(""+settings.div.selector+" .ei-event").map(function() {
        var $item = $(this);
        //	collect data from html
        return {
            start: moment($item.data("start")),
            end: moment($item.data("end")),
            name: $item.find(".ei-name").text(),
            description: $item.find(".ei-description").text(),
            loc: $item.data("loc"),
            eventtype: $item.data("eventtype"),
            eventdaytype: $item.find(".eventdaytype").text(),
            parts: $item.data("parts"),
            eventid: $item.find(".ei-event-download-link").text()
        };
    }).get();
    
    // sort collected data by start date and time
    data.sort(sortByStartTimes);

    var currentDate = moment();

    var isTodayEventClass = "is-today-event";

    var availableYears = getAvailableYears(data);

    // theme 
    $(settings.div.selector).attr("id", "ei-events" + settings.theme + "");

    // get actual events for month and year
    function getEvents(data, date) {
        var out = [
            '<div class="ei-events-wrapper' + (settings.hasStickyNavigator ? " has-nav-container-sticky" : "") + '" id="ei-events' + settings.theme + '"><div class="ei-nav-container">',
            //'<h2>' + date.format('MMMM') + ' ' + date.year() + '</h2>', '<i class="fa fa-chevron-circle-left ei-arrow-left"></i><i class="fa fa-chevron-circle-right ei-arrow-right"></i>',
            '<h2>' + date.year() + '</h2>',
            '<i class="fa fa-chevron-circle-left ei-arrow-control ei-arrow-left' + (availableYears.indexOf(date.year()) === 0 ? " disable-calendar" : "") + '"></i>' +
            '<i class="fa fa-chevron-circle-right ei-arrow-control ei-arrow-right' + (availableYears.indexOf(date.year()) === (availableYears.length - 1) ? " disable-calendar" : "") + '"></i>',
            '</div>',
            '<div class="ei-events-container">'];

        var currentHost = window.location.hostname;
        var oldClientSiteBaseUrl = "http://clients3.ges-invest.com";
        if (currentHost.indexOf("test") === -1 && currentHost.indexOf("local") === -1) {
            oldClientSiteBaseUrl = "https://clients.ges-invest.com";
        }

        var counter = 0;
        var hasTodayEvent = false;
        var currentMoment = moment();

        for (var i = 0; i < data.length; i++) {
            if (date.year() === data[i].start.year()) {
                counter++;

                var eventClass = "";
                if (new Date(data[i].start).getTime() < new Date(currentMoment).getTime()) {
                    eventClass += " event-past";
                }

                var tooltip = "";
                var hasMore = data[i].description.length > 45;

                //Add class to scroll to today event
                if (!hasTodayEvent && currentMoment.isSame(data[i].start, "day")) {
                    hasTodayEvent = true;
                    eventClass += " " + isTodayEventClass;    
                }

                if (hasMore) {
                    eventClass += " event-has-more";
                    tooltip = "Send this calendar to me";
                }

                //out.push('<div class="ei-event ' + eventClass + '"><div class="ei-preview desc_trig" style="border-left-color: ' + genRandomColor() + ';">');
                out.push('<div title="' + tooltip + '" class="ei-event ' + eventClass + '"><div class="ei-preview desc_trig" style="border-left-color: #fcaf17;">');
                out.push('<div class="ei-date">');
                out.push('<div class="ei-day">' + data[i].start.date() + '</div>');
                //out.push('<div class="ei-day_end">- '+data[i].end.date()+'</div>');
                out.push('<div class="ei-month">' + data[i].start.format('MMM') + '</div>');
                out.push('</div>');
                out.push('<div class="ei-content">');
                out.push('<div class="ei-event-type ei-event-type-' + data[i].eventtype + '"></div>');

                var displayName = cutToLimit(data[i].name, settings.limitedCharacter);
                var descriptionLower = data[i].description.toLowerCase();
                var isSimpleAgm = displayName.toLowerCase() === "agm" && descriptionLower === "agm";
                var isEngagementevent = true;
                
                if (data[i].parts) {
                    var parts = data[i].parts.toString().split("-");
                    var success = false;
                    if (parts.length === 2) {
                        if (parts[0] !== "" && parts[0] !== "0") { // company link
                            out.push('<div class="ei-name"><a href="/Company/Profile/' + parts[0] + '">' + displayName + '</a></div>');
                            isEngagementevent = false;
                            success = true;
                        } else if (parts[1] !== "") { // engagement type link > to old site
                            out.push('<div class="ei-name"><a target="_blank" href="' + oldClientSiteBaseUrl + '/en-US/client/engagement_forum/engagement_type.aspx?I_EngagementTypes_Id=' + parts[1] + '">' + displayName + '</a></div>');
                            success = true;
                        }
                    }

                    if (!success) {
                        out.push('<div class="ei-name" data-parts="' + data[i].parts + '">' + displayName + '</div>');
                    }
                } else {
                    out.push('<div class="ei-name" data-parts="' + data[i].parts + '">' + displayName + '</div>');   
                }                
                
                if (descriptionLower.indexOf("agm") < 0 && descriptionLower.indexOf("annual general meeting") < 0) {

                    var timeStr = data[i].start.format('HH:mm');
                    timeStr = timeStr === "00:00" ? "Day Event" : timeStr;
                    
                    if ((data[i].eventdaytype === "Day Event")) {
                        timeStr = "Day Event";
                    }

                    out.push('<div class="ei-time"><i class="fa fa-clock-o"></i> ' + timeStr);
                    if (data[i].loc !== "") {
                        out.push(' <i class="fa fa-location-arrow"></i> ');
                    }
                    out.push('</div>');
                    out.push('<div class="ei-description">' + data[i].description + '</div>');

                } else if (!isSimpleAgm) {
                    out.push('<div class="ei-description hide-time">' + data[i].description + "</div>");   
                }
                
                out.push('</div>');                
                
                if (!isEngagementevent && data[i].start.isValid() && data[i].end.isValid()){
                  
                    out.push('<div class="ei-link" style="height: 20px; padding-right: 10px; "><div class="ei-event-send-mail-link ">' + '<button  type="button"  title="Email me event details" style="padding: 0px; background-color: transparent; padding-bottom: 20px;"  class="btn btn-sm btn-send-email-calendar ei-event-send-mail-link-loding'  + data[i].eventid + '" value="' + data[i].eventid + '" data-loading-text="<i class=\'fa fa-circle-o-notch fa-spin\'></i> Sending email..." ><i class="fa fa-paper-plane"></i></button>' + "</div></div>"); 
                }                
 
                out.push('</div>');
                
                if (hasMore) {
                    out.push('<div class="event_description evcal_eventcard" style="display: none;"><div class="evo_metarow_details evorow evcal_evdata_row bordb evcal_event_details"><span class="evcal_evdata_icons"><i class="fa fa-align-justify"></i></span><div class="evcal_evdata_cell "><div class="eventon_full_description"><h3 class="padb5 evo_h3">Event Details</h3><div class="eventon_desc_in" itemprop="description"><p>' + data[i].description + '</p></div><div class="clear"></div></div></div></div></div><div class="clear end"></div>');
                }
                
                out.push('</div>');                
            }
        }

        if (counter === 0) {
            out.push("<p>There are no events scheduled for this period.</p>");
        }

        out.push('</div>', '</div>');
        out = out.join('\n');
        $("div" + settings.div.selector + settings.theme + "").replaceWith(out);

        $(".ei-event").click(function () {
            $(this).find(".event_description").slideToggle();
        });

        $(".ei-arrow-left").click(function () {
            changeTime($(this), false);
        });

        $(".ei-arrow-right").click(function () {
            changeTime($(this), true);
        });

        //Scroll to today event
        var eventWrapper = $("div" + settings.div.selector + settings.theme + "");
        if (eventWrapper.length > 0) {
            var scrollElment = eventWrapper.parent();
            var offset = 6; //Additional offset for parent container
            if (hasTodayEvent) {
                scrollElment.animate({ scrollTop: eventWrapper.find("." + isTodayEventClass).position().top - eventWrapper.position().top + offset });
            } else if (currentMoment.isSame(date, "year")) {
                var futureEvents = eventWrapper.find(".ei-event:not(.event-past)");
                if (futureEvents.length > 0) {
                    scrollElment.animate({ scrollTop: futureEvents.position().top - eventWrapper.position().top + offset });
                } else {
                    scrollElment.animate({ scrollTop: eventWrapper[0].scrollHeight });
                }
            } else {
                scrollElment.animate({ scrollTop: 0 });
            }    
        }

        // Dot Dot Dot plugin init
        $(".ei-description").dotdotdot({
            //	settings
            ellipsis: '...'
        });

        $(".btn-send-email-calendar").on("click", function (e) {
            var self = $(e.target);
            var value = this.value;
            var currentInFocusListValue = self.hasClass("in-focus-list");
            var data = {
                eventId: value
            };

            $(".ei-event-send-mail-link-loding" + this.value).button("loading");
            $.ajax({
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                url: "/Company/EventExportSendMail2",
                data: JSON.stringify(data)
            })
                .done(function (response, textStatus, jqXHR) {
                    //self.button("reset");
                    if (response.meta.success) {
                        utils.quickNotification(response.meta.message);

                    } else {
                        utils.quickNotification(response.meta.message, "error");
                    }
                    $(".ei-event-send-mail-link-loding" + value).button("reset");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {                    
                    utils.quickNotification("Failed: Error occurred sending email", "error");
                })
                .always(function () {

                });
        });

        function changeTime(element, isMoveForward) {
            var currentYear = currentDate.year();
            var currentYearIndex = availableYears.indexOf(currentYear);

            if ((currentYearIndex >= (availableYears.length - 1) && isMoveForward)
                || (currentYearIndex <= 0 && !isMoveForward)) {
                element.addClass("disable-calendar");
                return;
            }

            var step = isMoveForward ? 1 : -1;
            
            getEvents(data, currentDate.add({ years: availableYears[currentYearIndex + step] - currentYear }));

            $(".ei-events-wrapper").linkify({
                target: "_blank"
            });
        }
    }

    function getAvailableYears(dateData) {
        var years = [];
        for (var i = 0; i < dateData.length; i++) {
            var year = dateData[i].start.year();
            if (years.indexOf(year) < 0)
                years.push(year);
        }
        var currentYear = currentDate.year();
        if (years.indexOf(currentYear) < 0) {
            years.push(currentYear);
            years.sort(function (a, b) {
                return a - b;
            });
        }
        return years;
    }
    
    // write initial data on page load
    getEvents(data, currentDate);
}

(function ($) {

    $.fn.eventify = function (options) {
        var settings = $.extend({
            // Default settings
            div: "#ei-events",
            locale: "en",
            theme: "",
            limitedCharacter: 80,
            hasStickyNavigator: false
        }, options);

        return mainEventify({
            div: $(this),
            locale: settings.locale,
            theme: settings.theme,
            limitedCharacter: settings.limitedCharacter,
            hasStickyNavigator: settings.hasStickyNavigator
        });

    };

    $.fn.extend({
        getPath: function () {
            var pathes = [];

            this.each(function (index, element) {
                var path, $node = jQuery(element);

                while ($node.length) {
                    var realNode = $node.get(0), name = realNode.localName;
                    if (!name) { break; }

                    name = name.toLowerCase();
                    var parent = $node.parent();
                    var sameTagSiblings = parent.children(name);

                    if (sameTagSiblings.length > 1) {
                        allSiblings = parent.children();
                        var index = allSiblings.index(realNode) + 1;
                        if (index > 0) {
                            name += ':nth-child(' + index + ')';
                        }
                    }

                    path = name + (path ? ' > ' + path : '');
                    $node = parent;
                }

                pathes.push(path);
            });

            return pathes.join(',');
        }
    });

}(jQuery));