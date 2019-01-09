
function EmployeeAgeReport(divId, title, category, legend, series)
{
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'line'
    this.xAxisPostfix = '岁';
    this.yAxisPostfix = '人';
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


function EmployeeGenderReportPie(divId, title, category, legend, series) {
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'pie'
    //this.category = category;
    this.legend = legend;
    this.series = series;

}

function EmployeeWorkAgeReportPie(divId, title, category, legend, series) {
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'pie'
    //this.category = category;
    this.legend = legend;
    this.series = series;
}


function EmployeeSalaryReportPie(divId, title, category, legend, series) {
    this.viewObj = document.getElementById(divId),
    this.title = title
    this.legendType = 'pie'
    //this.category = category;
    this.legend = legend;
    this.series = series;
}


function EmployeePostReport(divId, title, category, legend, series) {
    this.viewObj = document.getElementById(divId),
        this.title = title
    this.legendType = 'line'
    this.xAxisPostfix = '';
    this.yAxisPostfix = '人';
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
