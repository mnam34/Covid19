jQuery(document).ready(function () {
    $.get('/sm/report/chart-json-bar', function (data) {
        // ECHARTS
        require.config({
            paths: {
                echarts: '/themes/m475/assets/global/plugins/echarts/'
            }
        });

        // DEMOS
        require(
            [
                'echarts',
                'echarts/chart/bar',
                'echarts/chart/chord',
                'echarts/chart/eventRiver',
                'echarts/chart/force',
                'echarts/chart/funnel',
                'echarts/chart/gauge',
                'echarts/chart/heatmap',
                'echarts/chart/k',
                'echarts/chart/line',
                'echarts/chart/map',
                'echarts/chart/pie',
                'echarts/chart/radar',
                'echarts/chart/scatter',
                'echarts/chart/tree',
                'echarts/chart/treemap',
                'echarts/chart/venn',
                'echarts/chart/wordCloud'
            ],
            function (ec) {
                //--- BAR ---
                var myChart = ec.init(document.getElementById('fcase-e-chart-bar1'));
                myChart.setOption({
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        /*data: ['Ca dương tính', 'Expenses']*/
                        data: ['Ca dương tính']
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            mark: {
                                show: false
                            },
                            dataView: {
                                show: false,
                                readOnly: false
                            },
                            magicType: {
                                show: true,
                                type: ['line', 'bar']
                            },
                            restore: {
                                show: true
                            },
                            saveAsImage: {
                                show: true
                            }
                        }
                    },
                    calculable: true,
                    xAxis: [{
                        type: 'category',
                        data: data.Date
                    }],
                    yAxis: [{
                        type: 'value',
                        splitArea: {
                            show: true
                        }
                    }],
                    series: [
                        {
                            name: 'Ca dương tính',
                            type: 'bar',
                            data: data.Data
                        },
                        //{
                        //    name: 'Expenses',
                        //    type: 'bar',
                        //    data: [2.6, 5.9, 9.0, 26.4, 28.7, 70.7, 175.6, 182.2, 48.7, 18.8, 6.0, 2.3]
                        //}
                    ]
                });

                // --- LINE ---
                var myChart2 = ec.init(document.getElementById('fcase-e-chart-line1'));
                myChart2.setOption({
                    title: {
                        text: 'Weekly Weather',
                        subtext: 'Lorem ipsum'
                    },
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: ['High', 'Low']
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            mark: {
                                show: true
                            },
                            dataView: {
                                show: true,
                                readOnly: false
                            },
                            magicType: {
                                show: true,
                                type: ['line', 'bar']
                            },
                            restore: {
                                show: true
                            },
                            saveAsImage: {
                                show: true
                            }
                        }
                    },
                    calculable: true,
                    xAxis: [{
                        type: 'category',
                        boundaryGap: false,
                        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
                    }],
                    yAxis: [{
                        type: 'value',
                        axisLabel: {
                            formatter: '{value} °C'
                        }
                    }],
                    series: [{
                        name: 'High',
                        type: 'line',
                        data: [11, 11, 15, 13, 12, 13, 10],
                        markPoint: {
                            data: [{
                                type: 'max',
                                name: 'Max'
                            }, {
                                type: 'min',
                                name: 'Min'
                            }]
                        },
                        markLine: {
                            data: [{
                                type: 'average',
                                name: 'Mean'
                            }]
                        }
                    }, {
                        name: 'Low',
                        type: 'line',
                        data: [1, -2, 2, 5, 3, 2, 0],
                        markPoint: {
                            data: [{
                                name: 'Lowest',
                                value: -2,
                                xAxis: 1,
                                yAxis: -1.5
                            }]
                        },
                        markLine: {
                            data: [{
                                type: 'average',
                                name: 'Mean'
                            }]
                        }
                    }]
                });

            }
        );
    });
    
});