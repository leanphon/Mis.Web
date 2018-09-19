

function showReport(viewEntity) {
    var myChart = echarts.init(viewEntity.viewObj);
    
    var seriesData = viewEntity.option.series;
    var series = new Array();
    for (var i=0; i < seriesData.length; i++)
    {
        var e = {
            name: viewEntity.option.series[i].name,
            type: 'line',
            data: viewEntity.option.series[i].data,
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        }

        series.push(e);
    }

    var option = {
        title: {
            text: viewEntity.title,
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: viewEntity.option.legend
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: false },
                dataView: { show: false, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                boundaryGap: false,
                data: viewEntity.option.category
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value} ' + viewEntity.yAxisDesc
                }
            }
        ],
        series: series,
    };

    // 为echarts对象加载数据
    myChart.setOption(option);
}
