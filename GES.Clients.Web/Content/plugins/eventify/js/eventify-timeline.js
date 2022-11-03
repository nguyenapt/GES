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

function mainEventify(settings) {
    // load user settings
    moment.locale(settings.locale);
    if (settings.theme != "") {
        settings.theme = '-' + settings.theme;
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
            eventdaytype: $item.data("eventdaytype"),
            parts: $item.data("parts")
        };
    }).get();
    
    var currentDate = moment();

    // theme 
    $(settings.div.selector).attr("id", "ei-events" + settings.theme + "");

    // get actual events for month and year
    function getEvents(data, date) {
        var out = ['<div class="ei-events-container">'];
        var counter = 0;
        for (var i = 0; i < data.length; i++) {
                counter++;

                var eventClass = "";
                var tooltip = "";
            out.push('<div title="' + tooltip + '" class="ei-event-timeline ' + eventClass + '"><div class="ei-preview desc_trig" style="border-left-color: #fcaf17;">');
                out.push('<div class="ei-date">');
                out.push('<div class="timeline-month">' + data[i].start.format('MMM') + '</div>');
                out.push('<div class="timeline-day">' + data[i].start.date() + '</div>');
                out.push('<div class="timeline-year">' + data[i].start.year() + '</div>');
                out.push('</div>');
                out.push('<div class="ei-content-timeline">');
                //out.push('<div class="ei-event-type ei-event-type-' + data[i].eventtype + '"></div>');

                var displayName = cutToLimit(data[i].name, settings.limitedCharacter);
                out.push('<div class="ei-name-timeline" data-parts="' + data[i].parts + '">' + displayName + '</div>');   
                
                out.push('</div>');
                out.push('</div>');
                out.push('</div>');
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