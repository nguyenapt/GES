var chartCreator = (function () {
    var createPieChart = function (containerId, pieColors, pieData) {
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
                                width: "150px",
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
if (window._pieColors !== undefined && window._pieData !== undefined)
    chartCreator.createPieChart("kpi-chart-container", window._pieColors, window._pieData);