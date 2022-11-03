$(function () {
    //------------------------------------
    //------------SETTING UP--------------
    //------------------------------------

    // set flags for content loading
    window.loadingChartRecomm = false;
    window.loadingChartNorm = false;
    window.loadingChartSector = false;
    window.loadingChartLocation = false;
    window.loadingHeatMap = false;
    window.loadingLatestNews = false;
    window.loadingLatestMilestones = false;
    window.loadingCalendar = false;

    window.selectedIndices = new Array();
    window.selectedPortfolios = new Array();

    // make some dropdown multiple-selectable
    $("#combobox-portfolio").selectpicker({
        style: "btn-default btn-white",
        noneSelectedText: "All Portfolios",
        deselectAllText: "Reset",
        size: 10
    });

    $("#combobox-index").selectpicker({
        style: "btn-default btn-white",
        noneSelectedText: "All Indices",
        deselectAllText: "Reset",
        size: 10
    });

    // bind click events on configuration buttons
    $(".btn-apply-portfolios").on("click", function (e) {
        // not allow to click if disabled
        if ($(this).prop("disabled")) {
            return;
        }

        // disable this button temporarily
        // until all ajax components have been loaded, or failed
        $(this).prop("disabled", "disabled");

        // reload boxes
        reloadChart("recommendation");
        setTimeout(function () {
            reloadChart("norm");
            setTimeout(function () {
                reloadChart("sector");
                setTimeout(function () {
                    reloadChart("location");
                    setTimeout(function () {
                        reloadCasesMap();
                        reloadCalendar();
                        setTimeout(function () {
                            reloadInfoBox();
                            setTimeout(function () {
                                reloadLatestNews();
                                reloadLatestMilestones();
                            }, 300);
                        }, 500);
                    }, 500); // others
                }, 300);
            }, 300);
        }, 1000); // charts
    });

    $(".btn-apply-indices").on("click", function (e) {
        // not allow to click if disabled
        if ($(this).prop("disabled")) {
            return;
        }

        // disable this button temporarily
        // until all ajax components have been loaded, or failed
        $(this).prop("disabled", "disabled");

        // reload boxes
        reloadChart("recommendation");
        setTimeout(function () {
            reloadChart("norm");
            setTimeout(function () {
                reloadChart("sector");
                setTimeout(function () {
                    reloadChart("location");
                    setTimeout(function () {
                        reloadCasesMap();
                        reloadCalendar();
                        setTimeout(function () {
                            reloadInfoBox();
                            setTimeout(function () {
                                reloadLatestNews();
                                reloadLatestMilestones();
                            }, 300);
                        }, 500);
                    }, 500); // others
                }, 300);
            }, 300);
        }, 1000); // charts
    });

    // make .box-config-portfolios hidden by default
    $(".box-config-portfolios").hide();

    // handle button group
    handleBtnGroup();

    // bind actions to dropdowns
    bindActionsToDropdown();

    // bind actions to things on sidebar
    $("body").on("click", ".sidebar-cell.dashboard-item", function (e) {
        if ($(e.target).hasClass("morelink"))
            return;

        var moreLink = $(this).find(".morelink");
        if (moreLink.length > 0)
            moreLink[0].click();
    });

    /*-- Templates (Handlebars) --*/
    //-- Calendar template
    var calendarSource = $("#calendar-template").html();
    window.calendarTpl = Handlebars.compile(calendarSource);
    //-- Blog Posts template
    var blogSource = $("#blog-posts-template").html();
    window.blogTpl = Handlebars.compile(blogSource);
    //-- Stats box template
    var statsSource = $("#stats-box-template").html();
    window.statsTpl = Handlebars.compile(statsSource);
    /*-- /Templates (Handlebars) --*/

    // Tab Management
    var activeTab = null;
    $(".tabs-news-milestones a[data-toggle=\"tab\"]").on("shown.bs.tab", function (e) {
        var activeTabNewsMilestones = getActiveTabNewsOrMilestones();
        //console.log("switched to: " + activeTabNewsMilestones);
        switch(activeTabNewsMilestones) {
            case "news":
                reloadLatestNews();
                break;
            case "milestones":
                reloadLatestMilestones();
                break;
        }
    });

    //------------------------------------
    //-------START DATA HANDLING----------
    //------------------------------------

    updateSelectedIndicesList();
    updateSelectedPortfoliosList();

    // trigger first tab
    $(".btn-group-dashboard .btn:first-of-type").trigger("click");
});

function getCurrentActiveTab() {
    return window.currentActiveTab;
}

function reloadChart(chartType) {
    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var canvasSelector = "#doughnutChart-by-" + chartType;
    var boxSelector = ".chart-cases-by-" + chartType;
    var legendModalSelector = "#legend-" + chartType;

    // loading state
    $(boxSelector + " .chart-responsive").html('<canvas id="doughnutChart-by-' + chartType + '" height="100"></canvas>');
    $(boxSelector + " .box-footer").html('');
    boxIsLoading(true, boxSelector);

    var legendModalTitle = "Details: Cases by ";
    switch (chartType) {
        case "recommendation":
            legendModalTitle += "Recommendation";
            break;
        case "norm":
            legendModalTitle += "Norm";
            break;
        case "sector":
            legendModalTitle += "Sector";
            break;
        case "location":
            legendModalTitle += "Location";
            break;
    }

    // set loading status
    setLoadingStatus(true, chartType);

    var postData = {
        chartType: chartType,
        tab: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetDoughnutChartData/" + window.uniqueKey,
        data: JSON.stringify(postData)
    })
        .done(function (response, textStatus, jqXHR) {
            boxIsLoading(false, boxSelector);

            // reset chart drawing area
            resetChartArea(chartType);

            // No data
            if (response.datasets[0].data.length === 0) {
                $(boxSelector).addClass("no-chart");
                $(boxSelector + " .chart-message").html('<p>No data available.<p/>');
                return;
            }

            $(boxSelector).removeClass("no-chart");
            var doughnutChartCtx = $(canvasSelector).get(0).getContext("2d");
            doughnutChartCtx.canvas.width = 200;
            doughnutChartCtx.canvas.height = 160;

            //Create douhnut charts
            var myDoughnutChart = new Chart(doughnutChartCtx, {
                type: 'doughnut',
                data: response,
                options: {
                    padding: 0,
                    cutoutPercentage: 65,
                    maintainAspectRatio: false,

                    tooltips: {
                        enabled: false,
                        mode: 'index',
                        position: 'nearest',
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var dataset = data.datasets[tooltipItem.datasetIndex];
                                var labels = data.labels;
                                var total = dataset.data.reduce(function(previousValue, currentValue, currentIndex, array) {
                                    return previousValue + currentValue;
                                });
                                var currentValue = dataset.data[tooltipItem.index];
                                var percentage = Math.round((currentValue/total) * 100 * 10) / 10;         
                                return labels[tooltipItem.index] + ": " + dataset.data[tooltipItem.index] + " (" + percentage + "%)";
                            }
                        },
                        custom: customTooltips
                    },

                    legend: {
                        labels: {
                            generateLabels: function () {
                                return "";
                            }
                        }
                    },
                    legendCallback: function (chart) {
                        //console.log(chart.data);
                        // No data
                        if (chart.data.datasets[0].data.length === 0) {
                            return '';
                        }


                        var text = [];

                        text.push('<div class="pull-right"><a class="anchor-pointer legend-content" data-target="' + legendModalSelector + '" qtip-content title="View legend of this chart"><i class="fa fa-lg fa-info-circle"></i>&nbsp;Legend</a></div>');
                        text.push('<div id="legend-' + chartType + '" class="modal fade" role="dialog" style="display: none;">' +
                                    '<div class="modal-dialog">' + 
                                        '<div class="modal-content">' + 
                                            '<div class="modal-header">' + 
                                                '<button type="button" class="close" data-dismiss="modal">×</button>' + 
                                                '<h4 class="modal-title">' + legendModalTitle + '</h4>');
                        text.push('</div><div class="modal-body">');
                        text.push('<ul class="chart-legend clearfix">');

                        var qtipContent = [];
                        qtipContent.push('<ul class="">');

                        var total = chart.data.datasets[0].data.reduce(function (previousValue, currentValue, currentIndex, array) {
                            return previousValue + currentValue;
                        });

                        for (var i = 0; i < chart.data.datasets[0].data.length; i++) {
                            text.push('<li>');
                            text.push('<i class="fa fa-circle" style="color:' + chart.data.datasets[0].backgroundColor[i] + '"></i> ');

                            qtipContent.push('<li>');
                            qtipContent.push('<i class="fa fa-circle" style="color:' + chart.data.datasets[0].backgroundColor[i] + '"></i> ');

                            var currentValue = chart.data.datasets[0].data[i];
                            var percentage = Math.round((currentValue / total) * 100 * 10) / 10;
                            if (chart.data.labels[i]) {
                                text.push(chart.data.labels[i] + ": " + currentValue + " (" + percentage + "%)");
                                qtipContent.push(chart.data.labels[i] + ": " + currentValue + " (" + percentage + "%)");
                            }
                            text.push('</li>');
                            qtipContent.push('</li>');
                        }
                        text.push('</ul>');
                        qtipContent.push('</ul>');

                        var qtipContentHtml = qtipContent.join("");

                        var legendHtml = text.join("");
                        legendHtml = legendHtml.replace("qtip-content", "qtip-content='" + qtipContentHtml + "'");

                        return legendHtml;
                    }
                }
            });

            $(boxSelector + " .box-footer").html(myDoughnutChart.generateLegend());
            $(boxSelector + " .box-footer .legend-content").qtip({
                content: {
                    text: function (event, api) {
                        return $(this).attr("qtip-content");
                    },
                    title: "Legend"
                },
                position: {
                    my: "top right",
                    at: "center left",
                    adjust: {
                        method: "shift"
                    }
                }
            });

            // bind click event
            $(canvasSelector).unbind("click");
            $(canvasSelector).unbind("touchstart");
            $(canvasSelector).bind("click touchstart", 
                function (evt) {
                    var elem = myDoughnutChart.getElementAtEvent(evt);
                    openTheChartElemLink(elem, myDoughnutChart);
                }
            );

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, boxSelector);
            utils.quickNotification("Loading chart data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, chartType);
        });
}

function openTheChartElemLink(elem, chartObj) {
    var chartType = detectChartTypeFromElem(elem);
    switch(chartType) {
        case "recommendation":
            utils.openLinkInNewTab("/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr()
                + "&recommendationIds=" + chartObj.config.data.datasets[0].ids[elem[0]._index], "_blank");
            break;
        case "norm":
            utils.openLinkInNewTab("/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr()
                + "&recommendationIds=6,7&normAreaIds=" + chartObj.config.data.datasets[0].ids[elem[0]._index], "_blank");
            break;
        case "sector":
            utils.openLinkInNewTab("/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr()
                + "&recommendationIds=6,7&sectorIds=" + chartObj.config.data.datasets[0].ids[elem[0]._index], "_blank");
            break;
        case "location":
            utils.openLinkInNewTab("/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr()
                + "&recommendationIds=6,7&continentIds=" + chartObj.config.data.datasets[0].ids[elem[0]._index], "_blank");
            break;
        default:
            break;
    }
}

function detectChartTypeFromElem(elem) {
    var id = elem[0]._chart.canvas.id;
    if (id.indexOf("-recommendation") !== -1) {
        return "recommendation";
    } else if (id.indexOf("-norm") !== -1) {
        return "norm";
    } else if (id.indexOf("-sector") !== -1) {
        return "sector";
    } else if (id.indexOf("-location") !== -1) {
        return "location";
    } else {
        return "";
    }
}

function reloadCasesMap() {
    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var selector = ".box-figure.figure-cases-map";

    // loading state
    boxIsLoading(true, selector);
    setLoadingStatus(true, "heatmap");

    var data = {
        type: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetMapData/" + window.uniqueKey,
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            boxIsLoading(false, selector);

            //console.log("Cases Map Data...");
            //console.log(response);

            $("#world-map-cases").html("");
            $("#world-map-cases").vectorMap({
                map: "world_mill",
                backgroundColor: "transparent",
                //backgroundColor: "#ecf0f5",
                regionStyle: {
                  initial: {
                    fill: 'rgba(210, 214, 222, 1)',
                    "fill-opacity": 1,
                    stroke: 'none',
                    "stroke-width": 0,
                    "stroke-opacity": 1
                  },
                  hover: {
                    "fill-opacity": 0.7,
                    cursor: 'pointer'
                  },
                  selected: {
                    fill: 'yellow'
                  },
                  selectedHover: {
                  }
                },

                series: {
                    regions: [{
                        values: response,
                        scale: ['#C8EEFF', '#0071A4'],
                        normalizeFunction: 'polynomial'
                    }]
                },
                onRegionTipShow: function (e, el, code) {
                    var hoverVal = "No";
                    var plural = "y";
                    if (response[code]) {
                        hoverVal = response[code];

                        if (response[code] > 1) {
                            plural = "ies";
                        }
                    }

                    el.html(el.html() + ' (' + hoverVal + ' compan' + plural + ')');
                },
                onRegionClick: function (event, code) {
                    openLinkInNewTab("/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr() + "&recommendationIds=6,7&homecountries=" + code, "_blank");
                }
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, selector);
            utils.quickNotification("Loading data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, "heatmap");
        });
}

function genBlogContent(blogTpl, data) {
    var html = blogTpl(data);
    return html;
}

function formatBlogContent(data) {
    $.each(data.entries, function (key, value) {
        var post = value;

        // format date
        var format = moment.parseFormat(post.pubDate/* , options */);
        var convertedDate = moment(post.pubDate, format).format("MMM D, YYYY");
        post.publishedDate = convertedDate;

        var content = $.parseHTML(post.description);
        var selectedNodes = $(content).find("p");
        var shortenContent = "";
        for (var i = 0; i < 2; i++) {
            if (selectedNodes.length > 0) {
                if (i === 0) {
                    shortenContent += "<p>" + selectedNodes[i].innerHTML
                        .replace("&nbsp;", "")
                        .replace("by ", "(by ");
                } else {
                    shortenContent += "<p>" + selectedNodes[i].innerHTML;
                }
            }
            if (i === 1) {
                shortenContent += ".. <a target='_blank' href='" + post.link + "'>Read more</a>";
            }

            if (i === 0) {
                shortenContent += ")";
            }
            shortenContent += "</p>";
        }
        post.content = shortenContent;
    });
    return data;
}

function reloadInfoBox() {
    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var selector = ".box-dashboard-info";

    // update box title
    var boxTitle = "";
    switch(currentTab) {
        case "portfolios":
            boxTitle = "Portfolios ";
            break;
        case "indices":
            boxTitle = "Indices ";
            break;
        case "focuslist":
            boxTitle = "Focus List ";
            break;
        default:
            break;
    }
    boxTitle = boxTitle + "Info";
    $(selector + " h3").html(boxTitle);

    // loading state
    boxIsLoading(true, selector);
    setLoadingStatus(true, "dashboard-info");

    var data = {
        type: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetInfoBoxData/" + window.uniqueKey,
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            boxIsLoading(false, selector);

            // prepare template data
            var infoData = prepareInfoBoxData(response);

            $(selector + " .box-body").html(genStatsBoxContent(window.statsTpl, infoData.infoBoxData));
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, selector);
            utils.quickNotification("Loading data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, "dashboard-info");
        });
}

function prepareInfoBoxData(res) {
    res.infoBoxData.companiesLink = "/Company/List?" + mapGetCurrentSelectedPortfolioIndexStr();
    res.infoBoxData.casesLink = res.infoBoxData.companiesLink;

    if (res.infoBoxData.DashboardInfoDetails) {
        $.each(res.infoBoxData.DashboardInfoDetails, function (key, value) {
            value.LastUpdated = utils.dateFormatter(value.LastUpdated);
            value.LinkToSearchPage = "/Company/List?portfolioIds=" + value.PortfolioId;
        });
    }
    return res;
}

function getActiveTabNewsOrMilestones() {
    var activeTab = $(".tabs-news-milestones .tab-heading.active:first");
    if (activeTab.hasClass("tab-heading-news")) {
        return "news";
    }
    return "milestones";
}

function reloadLatestNews() {
    var activeTabNewsMilestones = getActiveTabNewsOrMilestones();
    if (activeTabNewsMilestones !== "news" || window.loadedLatestNews === true)
        return;

    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var selector = "#latest-news";

    // loading state
    boxIsLoading(true, selector);
    setLoadingStatus(true, "latest-news");
    
    var data = {
        type: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetLatestNews/" + window.uniqueKey,
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            window.loadedLatestNews = true;
            boxIsLoading(false, selector);

            $(selector).html(response);

            utils.shortenIfPassLimit(180, ".dashboard-news-desc");

            $(selector).linkify({
                target: "_blank"
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, selector);
            utils.quickNotification("Loading data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, "latest-news");
        });
}

function reloadLatestMilestones() {
    var activeTabNewsMilestones = getActiveTabNewsOrMilestones();
    if (activeTabNewsMilestones !== "milestones" || window.loadedLatestMilestones === true)
        return;

    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var selector = "#latest-milestones";

    // loading state
    boxIsLoading(true, selector);
    setLoadingStatus(true, "latest-milestones");

    var data = {
        type: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetLatestMilestones/" + window.uniqueKey,
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            window.loadedLatestMilestones = true;
            boxIsLoading(false, selector);

            $(selector).html(response);

            utils.shortenIfPassLimit(180, ".dashboard-milestone-desc");

            $(selector).linkify({
                target: "_blank"
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, selector);
            utils.quickNotification("Loading data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, "latest-milestones");
        });
}

function reloadCalendar() {
    var currentTab = getCurrentActiveTab();
    var selectedVals = getSelectedPortfoliosOrIndices(currentTab);
    var selector = ".box-calendar";

    // loading state
    boxIsLoading(true, selector);
    setLoadingStatus(true, "calendar");

    var data = {
        type: currentTab,
        selectedPortfoliosOrIndices: selectedVals,
    }
    $.ajax({
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/DashboardGetCalendarEvents/" + window.uniqueKey,
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            boxIsLoading(false, selector);

            //$("#latest-milestones").html(response);
            $(selector + " .box-body").html(genCalendarContent(window.calendarTpl, response));
            $("#ei-events").eventify({
                locale: "en",
                hasStickyNavigator: true
            });
            $(selector).linkify({
                target: "_blank"
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            boxIsLoading(false, selector);
            utils.quickNotification("Loading data failed!", "error");
        })
        .always(function () {
            setLoadingStatus(false, "calendar");
        });
}

function resetChartArea(chartType) {
    var selector = ".chart-cases-by-" + chartType;

    $(selector + " .chart-message").html("");
    $(selector + " .chart-responsive canvas").remove();
    $(selector + " .chart-responsive").html("");
    $(selector + " .chart-responsive").append('<canvas id="doughnutChart-by-' + chartType + '" height="100"></canvas>');
}

function getSelectedPortfoliosOrIndices(currentTab) {
    var selectedVals = [];
    switch (currentTab) {
        case "indices":
            selectedVals = window.selectedIndices;
            break;
        case "portfolios":
            selectedVals = window.selectedPortfolios;
            break;
        default:
            break;
    }
    return selectedVals;
}

function getSelectedValuesOfMultipleDropdown(multipleSelectId) {
    var selectedValues = new Array();
    $("#" + multipleSelectId + " :selected").each(function (i, selected) {
        selectedValues[i] = parseInt($(selected).val());
    });

    if (selectedValues.length === 0) {
        $("#" + multipleSelectId + " option").each(function (i, val) {
            selectedValues[i] = parseInt($(val).val());
        });
        selectedValues[selectedValues.length] = -1;
    }

    return selectedValues;
}

function updateSelectedIndicesList() {
    // enable toggle buttons
    $(".btn-apply-indices").prop("disabled", "");

    var indicesComboId = "combobox-index";
    window.selectedIndices = getSelectedValuesOfMultipleDropdown(indicesComboId);

    // reset news and milestones checkpoints
    window.loadedLatestNews = false;
    window.loadedLatestMilestones = false;

    //console.log("-=- selected indices -=-");
    //console.log(window.selectedIndices);
}

function updateSelectedPortfoliosList() {
    // enable toggle buttons
    $(".btn-apply-portfolios").prop("disabled", "");

    var portfoliosComboId = "combobox-portfolio";
    window.selectedPortfolios = getSelectedValuesOfMultipleDropdown(portfoliosComboId);

    // reset news and milestones checkpoints
    window.loadedLatestNews = false;
    window.loadedLatestMilestones = false;

    //console.log("-=- selected portfolios -=-");
    //console.log(window.selectedPortfolios);
}

function bindActionsToDropdown() {
    var indicesComboId = "combobox-index";
    $("#" + indicesComboId).on("change", function () {
        updateSelectedIndicesList();
    });

    var portfoliosComboId = "combobox-portfolio";
    $("#" + portfoliosComboId).on("change", function () {
        updateSelectedPortfoliosList();
    });
}

function genCalendarContent(calendarTpl, data) {
    var html = calendarTpl(data);
    return html;
}

function genStatsBoxContent(statsTpl, data) {
    var html = statsTpl(data);
    return html;
}

function handleBtnGroup() {
    $(".btn-group-dashboard > button.btn").on("click", function () {
        // not allow to click if disabled
        if($(this).prop("disabled")) {
            return;
        }

        // enable toggle buttons
        $(".btn-apply-portfolios").prop("disabled", "");
        $(".btn-apply-indices").prop("disabled", "");

        var currentTab = getCurrentActiveTab();
        var tabVal = "";

        // disable other toggle buttons temporarily
        // until all ajax components have been loaded, or failed
        $(this).siblings().prop("disabled", "disabled");

        // reset news and milestones checkpoints
        window.loadedLatestNews = false;
        window.loadedLatestMilestones = false;

        if ($(this).hasClass("btn-toggle-indices")) {
            tabVal = "indices";
            if (currentTab === tabVal) {
                return;
            }

            $(".box-config-indices").slideDown();
            $(".box-config-portfolios").slideUp();
            window.currentActiveTab = tabVal;

            // update unique key
            window.uniqueKey = generateUniqueKey();

            $(".btn-apply-indices").trigger("click");

        } else if ($(this).hasClass("btn-toggle-portfolios")) {
            tabVal = "portfolios";
            if (currentTab === tabVal) {
                return;
            }

            $(".box-config-indices").slideUp();
            $(".box-config-portfolios").slideDown();
            window.currentActiveTab = tabVal;

            // update unique key
            window.uniqueKey = generateUniqueKey();

            $(".btn-apply-portfolios").trigger("click");

        } else {
            tabVal = "focuslist";
            if (currentTab === tabVal) {
                return;
            }

            $(".box-config-indices").slideUp();
            $(".box-config-portfolios").slideUp();
            window.currentActiveTab = tabVal;

            // update unique key
            window.uniqueKey = generateUniqueKey();

            reloadChart("recommendation");
            reloadChart("norm");
            reloadChart("sector");
            reloadChart("location");

            reloadLatestNews();
            reloadLatestMilestones();
            reloadCalendar();

            reloadInfoBox();

            reloadCasesMap();
        }
    });
}

function boxIsLoading(loading, selector) {
    if (loading) {
        if ($(selector).hasClass("box")) {
            $(selector + " div.overlay").css("display", "block");
        } else {
            $(selector).html('<div class="box box-primary box-loading-only"><div class="box-header"></div><div class="box-body"></div><div class="overlay"><i class="fa fa-refresh fa-spin"></i></div></div>');
        }
    } else {
        if ($(selector).hasClass("box")) {
            $(selector + " div.overlay").css("display", "none");
        } else {
            $(selector).html("");
        }
    }
}

function setLoadingStatus(value, component) {
    switch (component) {
        case "recommendation":
            window.loadingChartRecomm = value;
            break;
        case "norm":
            window.loadingChartNorm = value;
            break;
        case "sector":
            window.loadingChartSector = value;
            break;
        case "location":
            window.loadingChartLocation = value;
            break;
        case "heatmap":
            window.loadingHeatMap = value;
            break;
        case "latest-news":
            window.loadingLatestNews = value;
            break;
        case "latest-milestones":
            window.loadingLatestMilestones = value;
            break;
        case "calendar":
            window.loadingCalendar = value;
            break;
        default:
            break;
    }

    if (value === false) {
        checkAndReactivateToggles();
    }
}

function mapGetCurrentSelectedPortfolioIndexStr() {
    var currentTab = getCurrentActiveTab();
    var str = "portfolioIds=";
    switch(currentTab) {
        case "portfolios":
            str += window.selectedPortfolios.join(",");
            break;
        case "indices":
            str += window.selectedIndices.join(",");
            break;
        case "focuslist":
            str = "isFocusList=True";
            break;
    }
    return str;
}

function equalHeightTwoCols() {
    //var $col9 = $(".content-wrapper > .content > .row > .col-md-9");
    //var $col3 = $(".content-wrapper > .content > .row > .col-md-3");
    //var diff = $col3.height() - $col9.height();

    //var $eiEvents = $(".col-md-3 .ei-events-container");
    //var eiEventsCurrentHeight = $eiEvents.height();
    //var eiEventsNewHeight = eiEventsCurrentHeight - diff;
    //$eiEvents.css("height", eiEventsNewHeight + "px");
    //if (diff > 0) {
    //    $eiEvents.css("overflow-y", "scroll");
    //}

    //var diff2 = $col3.height() - $col9.height();
    //$eiEvents.css("height", ($eiEvents.height() - diff2) + "px");

    $(".box-calendar .box-body").css("height", "640px");
    $(".box-calendar .box-body").css("overflow-y", "auto");
}

function checkAndReactivateToggles() {
    if (window.loadingChartRecomm === true
            || window.loadingChartNorm === true
            || window.loadingChartSector === true
            || window.loadingChartLocation === true
            || window.loadingHeatMap === true
            || window.loadingLatestNews === true
            || window.loadingLatestMilestones === true
            || window.loadingCalendar === true
    ) {
        return; // not reactivate it yet, something is still loading
    }

    // else: reactivate those toggle buttons
    //console.log("ok, all components have been loaded :)");
    $(".btn-group-dashboard > button.btn").prop("disabled", "");

    // equal height
    equalHeightTwoCols();
}

function customTooltips(tooltip) {
    // Tooltip Element
    var tooltipEl = document.getElementById('chartjs-tooltip');
    // Hide if no tooltip
    if (tooltip.opacity === 0) {
        tooltipEl.style.opacity = 0;
        $("#" + this._chart.canvas.id).css("cursor", "default"); // hack: mouse cursor: normal
        return;
    }
    // Set caret Position
    tooltipEl.classList.remove('above', 'below', 'no-transform');
    if (tooltip.yAlign) {
        tooltipEl.classList.add(tooltip.yAlign);
    } else {
        tooltipEl.classList.add('no-transform');
    }
    function getBody(bodyItem) {
        return bodyItem.lines;
    }
    // Set Text
    if (tooltip.body) {
        var titleLines = tooltip.title || [];
        var bodyLines = tooltip.body.map(getBody);
        var innerHtml = '<thead>';
        titleLines.forEach(function (title) {
            innerHtml += '<tr><th>' + title + '</th></tr>';
        });
        innerHtml += '</thead><tbody>';
        bodyLines.forEach(function (body, i) {
            var colors = tooltip.labelColors[i];
            var style = 'background:' + colors.backgroundColor;
            style += '; border-color:' + colors.borderColor;
            style += '; border-width: 2px';
            var span = '<span class="chartjs-tooltip-key" style="' + style + '"></span>';
            innerHtml += '<tr><td>' + span + body + '</td></tr>';
        });
        innerHtml += '</tbody>';
        var tableRoot = tooltipEl.querySelector('table');
        tableRoot.innerHTML = innerHtml;
    }
    var position = this._chart.canvas.getBoundingClientRect();
    // Display, position, and set styles for font
    tooltipEl.style.opacity = 1;
    tooltipEl.style.left = position.left + tooltip.caretX + 'px';
    tooltipEl.style.top = position.top + tooltip.caretY + 'px';
    tooltipEl.style.fontFamily = tooltip._fontFamily;
    tooltipEl.style.fontSize = tooltip.fontSize;
    tooltipEl.style.fontStyle = tooltip._fontStyle;
    tooltipEl.style.padding = tooltip.yPadding + 'px ' + tooltip.xPadding + 'px';

    $("#" + this._chart.canvas.id).css("cursor", "pointer"); // hack: mouse cursor: pointer
}

function generateUniqueKey() {
    var boxType = getCurrentActiveTab();
    if (boxType === "focuslist") {
        return "individual" + window.individualId;
    } else {
        return "organization" + window.orgId;
    }
}