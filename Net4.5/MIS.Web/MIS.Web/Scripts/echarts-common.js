function showReport(viewEntity) {
    var myChart = echarts.init(viewEntity.viewObj);

    var seriesData = viewEntity.series;
    var series = new Array();
    for (var i = 0; i < seriesData.length; i++) {
        var e = {
            name: seriesData[i].name,
            type: viewEntity.legendType,
            data: seriesData[i].data,
            markPoint: viewEntity.markPoint,
            markLine: viewEntity.markLine
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
            data: viewEntity.legend
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
                interval:0,
                rotate:40,
                data: viewEntity.category,
                axisLabel: {
                    formatter: '{value} ' + viewEntity.xAxisPostfix
                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value} ' + viewEntity.yAxisPostfix
                }
            }
        ],
        series: series
    };

    // 为echarts对象加载数据
    myChart.setOption(option);
}

function showReportPie(viewEntity)
{
    var myChart = echarts.init(viewEntity.viewObj);

    var seriesData = viewEntity.series;
    var series = new Array();
    for (var i = 0; i < seriesData.length; i++) {
        var e = {
            name: seriesData[i].name,
            type: viewEntity.legendType,
            radius: '55%',
            center: ['50%', '60%'],
            data: seriesData[i].data,
        }

        series.push(e);
    }

    var option = {
        title: {
            text: viewEntity.title,
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            x: 'center',
            data: viewEntity.legend
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: false },
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['pie', 'funnel'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        calculable: true,
        series: series
    };

    // 为echarts对象加载数据
    myChart.setOption(option);

}

