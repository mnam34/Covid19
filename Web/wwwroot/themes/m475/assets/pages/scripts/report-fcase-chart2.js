jQuery(document).ready(function () {
    $(function () {
        var bar = document.getElementById("FCaseChartBAR").getContext("2d");
        var line = document.getElementById("FCaseChartLINE").getContext("2d");
        var barHor = document.getElementById("FCaseChartBARHor").getContext("2d");
        var json_bar = "/sm/report/chart-json-bar2";
        var json_line = "/sm/report/chart-json-line";
        var json_bar_hor = "/sm/report/chart-json-bar-hor";
        var barChart = new Chart(bar, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [
                    {
                        label: "Ca dương tính",
                        fill: false,
                        //lineTension: 0.1,
                        backgroundColor: "#ed6b75",
                        borderColor: "#e73d4a",
                        borderCapStyle: 'butt',
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "#e73d4a",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        data: [],
                        //spanGaps: false,
                    }
                ]
            },
            options: {
                tooltips: {
                    //mode: 'index',
                    //intersect: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                        },
                        
                    }],
                    
                }
            }
        });
        var lineChart = new Chart(line, {
            type: 'line',
            data: {
                labels: [],
                datasets: [
                    {
                        label: "Tổng ca dương tính",
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "#ed6b75",
                        borderColor: "#e73d4a",
                        borderCapStyle: 'butt',
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "#e73d4a",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        data: [],
                        //spanGaps: false,
                    }
                ]
            },
            options: {
                tooltips: {
                    //mode: 'index',
                    //intersect: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }],
                    xAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
        var barHorChart = new Chart(barHor, {
            type: 'bar',
            data: {
                labels: [],
                datasets: [
                    {
                        label: "Ca dương tính",
                        fill: false,
                        //lineTension: 0.1,
                        backgroundColor: "#ed6b75",
                        borderColor: "#e73d4a",
                        borderCapStyle: 'butt',
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "#e73d4a",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        data: [],
                        //spanGaps: false,
                    }
                ]
            },
            options: {
                indexAxis: 'y',
                tooltips: {
                    //mode: 'index',
                    //intersect: false
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true,
                        },

                    }],

                }
            }
        });
        ajax_chart(barChart, json_bar);
        setTimeout(function () {
            ajax_chart2(lineChart, json_line);
        }, 1000);
        setTimeout(function () {
            ajax_chart3(barHorChart, json_bar_hor);
        }, 2000);
        function ajax_chart(chart, url, data) {
            var data = data || {};
            $.getJSON(url, data).done(function (response) {
                chart.data.labels = response.Date;
                chart.data.datasets[0].data = response.Data; // or you can iterate for multiple datasets
                chart.update();
            });
        }
        function ajax_chart2(chart, url, data) {
            var data = data || {};
            $.getJSON(url, data).done(function (response) {
                chart.data.labels = response.Date;
                chart.data.datasets[0].data = response.Cumulative; // or you can iterate for multiple datasets
                chart.update();
            });
        }
        function ajax_chart3(chart, url, data) {
            var data = data || {};
            $.getJSON(url, data).done(function (response) {
                chart.data.labels = response.Commune;
                chart.data.datasets[0].data = response.Data; // or you can iterate for multiple datasets
                chart.update();
            });
        }
    });
});