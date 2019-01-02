function displayDesc(spanId, showFlag) {
    if (showFlag) {
        document.getElementById(spanId).style.display = "normal";
    }
    else {
        document.getElementById(spanId).style.display = "none";
    }
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, // month
        "d+": this.getDate(), // day
        "h+": this.getHours(), // hour
        "m+": this.getMinutes(), // minute
        "s+": this.getSeconds(), // second
        "q+": Math.floor((this.getMonth() + 3) / 3), // quarter
        "S": this.getMilliseconds()
        // millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "")
            .substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
function formatDate(value, format) {
    if (value == null || value == '') {
        return '';
    }
    var dateStr = value.replace(/\/Date\(/gi, '');
    dateStr = dateStr.replace(/\)\//gi, '');

    var dt = new Date(parseInt(dateStr, 10));

    return dt.format(format); //扩展的Date的format方法(上述插件实现)
}
function formatDateTime(value) {
    if (value == null || value == '') {
        return '';
    }
    var dateStr = value.replace(/\/Date\(/gi, '');
    dateStr = dateStr.replace(/\)\//gi, '');

    var dt = new Date(parseInt(dateStr, 10));

    return dt.format("yyyy-MM-dd hh:mm:ss"); //扩展的Date的format方法(上述插件实现)
}

function formatDate(value) {
    if (value == null || value == '') {
        return '';
    }
    var dateStr = value.replace(/\/Date\(/gi, '');
    dateStr = dateStr.replace(/\)\//gi, '');

    var dt = new Date(parseInt(dateStr, 10));

    return dt.format("yyyy-MM-dd"); //扩展的Date的format方法(上述插件实现)
}

function convertToDate(jsonDate) {
    if (jsonDate == undefined || jsonDate == null || jsonDate == '') {
        return null;
    }
    var dateStr = jsonDate.replace(/\/Date\(/gi, '');
    dateStr = dateStr.replace(/\)\//gi, '');

    var dt = new Date(parseInt(dateStr, 10));

    return dt;
}

function compareDate(d1, d2) {
    var m1 = d1.getTime();
    var m2 = d2.getTime();

    return m1 - m2;
}

function databoxFormat(date) {
    var years = date.getFullYear();//获取年  
    var months = date.getMonth() + 1;//获取日  
    var dates = date.getDate();//获取月  
    if (months < 10) {//当月份不满10的时候前面补0，例如09  
        months = '0' + months;
    }

    if (dates < 10) {//当日期不满10的时候前面补0，例如09  
        dates = '0' + dates;
    }

    return years + "-" + months + "-" + dates;
}

function databoxFormatMonth(date) {
    var years = date.getFullYear();//获取年  
    var months = date.getMonth() + 1;//获取日  

    if (months < 10) {//当月份不满10的时候前面补0，例如09  
        months = '0' + months;
    }

    return years + "-" + months;
}

function databoxParser(s) {
    if (!s)
        return new Date();

    return new Date(Date.parse(s));
}

function openFrame(subTitle, url) {
    document.getElementById("frameContent").src = url;
}

function showPrompt(msg) {
    $.messager.show({"title": "操作提示", "msg":msg})
}
function showAlert(msg) {
    $.messager.alert({ "title": "操作提示", "msg": msg, "icon": "warning" })
}
function showConfirm(msg) {
    var re = false;
    $.messager.confirm({
        "title": "操作提示", "msg": msg,"fn":
        function (result) {
            if (result) {
                re = result;
            }
        }
    });
    //console.log(re)

    return re;
}