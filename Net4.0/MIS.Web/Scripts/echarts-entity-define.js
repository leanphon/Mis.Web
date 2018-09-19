
function EmployeeAgeReport(divId, title, category, legend, series)
{
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'line'
    this.xAxisPostfix = '岁';
    this.yAxisPostfix = '名';
    this.category = category;
    this.legend = legend;
    this.series = series;
    this.markPoint = {
        data: [
            { type: 'max', name: '最大值' },
            { type: 'min', name: '最小值' }
        ]
    };
    this.markLine = {
        data: [
            { type: 'average', name: '平均值' }
        ]
    };
}


function EmployeeGenderReport(divId, title, category, legend, series) {
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'bar'
    this.xAxisPostfix = '';
    this.yAxisPostfix = '名';
    this.category = category;
    this.legend = legend;
    this.series = series;

    //this.markPoint = {
    //    data: [
    //        { type: 'max', name: '最大值' },
    //        { type: 'min', name: '最小值' }
    //    ]
    //};
    //this.markLine = {
    //    data: [
    //        { type: 'average', name: '平均值' }
    //    ]
    //};
}


