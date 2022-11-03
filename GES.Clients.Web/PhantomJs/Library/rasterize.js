// Render Multiple URLs to file

"use strict";
var RenderUrlsToFile, arrayOfUrls, system, outputs, hasCoverPage = false;
var separatorChars = ';#$';

system = require("system");

/*
Render given urls
@param array of URLs to render
@param callbackPerUrl Function called after finishing each URL, including the last URL
@param callbackFinal Function called after finishing everything
*/
RenderUrlsToFile = function(urls, filePaths, callbackPerUrl, callbackFinal) {
    var getFilename, next, page, retrieve, urlIndex = 0, webpage = require("webpage");
    page = null;

    getFilename = function(index) {
        return filePaths[index];
    };
    next = function(status, url, file) {
        page.close();
        callbackPerUrl(status, url, file);
        return retrieve();
    };
    retrieve = function() {
        var url;
        if (urls.length > 0) {
            url = urls.shift();
            urlIndex++;
            page = webpage.create();
            page.viewportSize = {
                width: 1920,
                height: 1080
            };
            if(urlIndex === 1 && urls.length > 0) {
                page.paperSize = {
                    format: 'A4',
                    orientation: 'portrait'
                };
            } else {
                page.paperSize = {
                    format: 'A4',
                    orientation: 'portrait',
                    margin: {
                        left: '1.5cm',
                        bottom: '2cm',
                        right: '1.5cm'
                    },
                    header: {
                        height: "1.5cm",
                        contents: phantom.callback(function (pageNum) {
                            if (pageNum === 1) {
                                return "<span style='float:right;margin-right:-8px'><img src=\"~/Content/img/logo.png\" style=\"width: 85px; height: auto;margin-top:10px\"/></span>";
                            }
                            return "";
                        })
                    },
                    footer: {
                        height: "1cm",
                        contents: phantom.callback(function (pageNum, numPages) {
                            var adjustNumberPage = hasCoverPage ? 1 : 0;
                            var footerStyle = 'font-size: 11px; font-weight: 300; color: #40484b;';
                            var programNameStype = 'float: left; max-width: calc(100% - 60px);text-align:justify;';
                            return '<div style="' + footerStyle + '"><span style="' + programNameStype + '">© ' + (new Date()).getFullYear() + ' Sustainalytics B.V. This content may not be reproduced, transmitted, redistributed, translated, sold,  exploited commercially or otherwise reused in any way whatsoever without Sustainalytics B.V.\'s prior written consent. All copyright and other proprietary rights remain the property of Sustainalytics B.V.</span><span style="float:right; padding-top:10px; margin-right:-8px">' + (pageNum + adjustNumberPage) + ' of ' + (numPages + adjustNumberPage) + '</span></div>';
                        })
                    }
                };
            }
            
            page.open(url);

            return page.onConsoleMessage = function (message) {
                function renderWhenPageFullyLoaded() {
                    var file = getFilename(urlIndex - 1);

                    setTimeout(function () {
                        var readyState = page.evaluate(function () {
                            return document.readyState;
                        });

                        if (readyState === 'complete') {
                            window.setTimeout(function () {
                                page.render(file);
                                return next(status, url, file);
                            }, urlIndex === 1 ? 200 : 1000);
                        } else {
                            window.setTimeout(function () {
                                renderWhenPageFullyLoaded();
                            });
                        }
                    }, 200);
                }

                if (message === 'Page loaded successfully') {
                    renderWhenPageFullyLoaded();
                } else if (message === 'Has cover page'){
                    hasCoverPage = true;
                }
                else {
                    phantom.exit();
                }
            };
        } else {
            return callbackFinal();
        }
    };
    return retrieve();
};

arrayOfUrls = null;
outputs = null;

if (system.args.length > 2) {
    arrayOfUrls = system.args[1].split(separatorChars);
    outputs = system.args[2].split(separatorChars);
} else {
    console.log("Using wrong arguments.");
}

RenderUrlsToFile(arrayOfUrls, outputs, (function(status, url, file) {
    if (status !== "success") {
        return console.log("Unable to render '" + url + "'");
    } else {
        return console.log("Rendered '" + url + "' at '" + file + "'");
    }
}), function() {
    console.log("Export success");
    return phantom.exit();
});
