

function ModuleTreeGrid(gridId, toolbar) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '模块名称', width: 250 },
        { field: 'showIndex', title: '显示顺序', width: 100 },
        { field: 'onlyRoot', title: '是否为root', width: 100 },
        { field: 'baseUrl', title: 'url', width: 250 },
    ]];

    this.toolbar = toolbar;
}


function FunctionRightGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'moduleName', title: '模块名', width: 250 },
                { field: 'name', title: '功能名', width: 250 },
                { field: 'url', title: 'url路径', width: 250 },
                { field: 'icon', title: '图标', width: 250 },
                { field: 'authorize', title: '是否授权', width: 250 },
    ]];

    this.toolbar = toolbar;
}

function RoleGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '角色名', width: 250 },
                { field: 'description', title: '描述', width: 250 },
    ]];

    this.toolbar = toolbar;
}

function RoleAssignRightTreeGrid(gridId, toolbar) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';
    this.singleSelect = false;
    this.checkbox = true;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'rightId', hidden: true },
                //{field: 'checked', title: '', checkbox: true, align: 'center', width: 100},
                { field: 'name', title: '功能名', width: 250 },
    ]];

    this.toolbar = toolbar;
}

function UserGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '用户名', width: 250 },
                { field: 'roleName', title: '角色', width: 250 },
                { field: 'status', title: '状态', width: 250 },
                { field: 'lastLogin', title: '最后一次登录', width: 250, formatter: formatDateTime },
    ]];

    this.toolbar = toolbar;
}

function LogGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'time', title: '时间', width: 250, formatter: formatDateTime },
        { field: 'name', title: '用户', width: 150 },
        { field: 'content', title: '日志内容', width: 400 },
                //{ field: 'type', title: '类型', width: 250 },
                
    ]];

    this.toolbar = toolbar;
}


function CompanyRegisterGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '公司名称', width: 250 },
                { field: 'code', title: '公司简称', width: 250 },
    ]];

    this.toolbar = toolbar;
}

function CompanyGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                {
                    field: 'logo', title: 'logo图片', width: 250,
                    formatter: function (value) {
                        return "<img src='/Upload/" + value + "' width='20px' height='20px'/>";
                    }
                },
                { field: 'name', title: '公司名称', width: 250 },
                { field: 'address', title: '公司地址', width: 250 },
                { field: 'code', title: '公司简称', width: 250 },
                {
                    field: 'loginImg', title: '登录背景图片', width: 250,
                    formatter: function (value) {
                        return "<img src='/Upload/" + value + "' width='20px' height='20px'/>";
                    }
                },
                //{ field: 'mainImg', title: '主页面背景图片', width: 250 },
    ]];

    this.toolbar = toolbar;
}



function PostGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '岗位名称', width: 150 },
    ]];

    this.toolbar = toolbar;
}

function LevelGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '层级名称', width: 150 },
                { field: 'code', title: '层级代码', width: 150 },
                { field: 'levelSalary', title: '岗位工资', width: 150 },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 150 },
                { field: 'seniorityRewardsBase', title: '工龄奖基数', width: 150 },
    ]];

    this.toolbar = toolbar;
}

function PerformanceGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'code', title: '绩效代码', width: 150 },
                { field: 'performanceRewards', title: '绩效奖金', width: 150 },
    ]];

    this.toolbar = toolbar;
}

function BenefitGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'code', title: '效益代码', width: 150 },
                { field: 'benefitRewards', title: '效益奖金', width: 150 },
    ]];

    this.toolbar = toolbar;
}

function DepartmentTreeGrid(gridId, toolbar) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';
    this.pager = true;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '部门名称', width: 250 },
                { field: 'code', title: '部门编码', width: 150 },
                { field: 'persons', title: '当前人数', width: 150 },
    ]];

    this.toolbar = toolbar;
}

function DepartmentSelectTreeGrid(gridId, toolbar) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';

    this.forzenCols = [[
    ]];

    this.normalCols = [[
        { field: 'id', hidden: true },
        { field: 'name', title: '部门名称', width: 250 },
        { field: 'code', title: '部门编码', width: 150 },
    ]];

    //this.toolbar = toolbar;
}


function EmployeeGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'number', title: '工号', width: 150 },
                { field: 'name', title: '姓名', width: 150 },
                { field: 'departmentName', title: '所在部门', width: 150, },
	            { field: 'postName', title: '岗位', width: 150, },
    ]];

    this.normalCols = [[
                { field: 'phone', title: '联系电话', width: 150 },
                { field: 'sex', title: '性别', width: 100 },
                { field: 'nation', title: '民族', width: 100 },
                { field: 'idCard', title: '身份证', width: 200 },
                //{ field: 'email', title: '工作邮箱', width: 200 },
                { field: 'birthday', title: '出生日期', width: 150, formatter: formatDate },
                { field: 'state', title: '员工状态', width: 150 },
                //{ field: 'shouldTotal', title: '标准应发月薪', width: 150 },
                { field: 'bankCard', title: '工资卡', width: 200 },

                { field: 'entryDate', title: '入职日期', width: 200, formatter: formatDate },
                { field: 'formalDate', title: '转正日期', width: 200, formatter: formatDate },
                { field: 'leaveDate', title: '离职日期', width: 200, formatter: formatDate },

                { field: 'nativePlace', title: '籍贯', width: 200 },
                { field: 'residence', title: '户口所在地', width: 250 },
                { field: 'address', title: '现居住地', width: 250 },

                { field: 'political', title: '政治面貌', width: 200 },
                { field: 'marriage', title: '婚姻状况', width: 200 },
                { field: 'education', title: '学历', width: 200 },
                { field: 'experience', title: '工作年限', width: 200 },
                { field: 'source', title: '人才来源', width: 200 },

                { field: 'contractSerial', title: '合同编号', width: 200 },
                { field: 'contractBegin', title: '合同起始日', width: 200, formatter: formatDate },
                { field: 'contractEnd', title: '合同结束日', width: 200, formatter: formatDate },

                { field: 'isSocialSecurity', title: '社保', width: 100 },
                { field: 'isPension', title: '退休金', width: 100 },
                { field: 'isUrbanRuralMedical', title: '城乡医疗', width: 100 },

                { field: 'emergencyContact', title: '紧急联系人', width: 150 },
                { field: 'emergencyPhone', title: '紧急联系人电话', width: 150 },

    ]];


    this.toolbar = toolbar;
}

function EmployeeCareerAddGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'employeeId', hidden: true },
                { field: 'status', hidden: true },
    ]];

    this.normalCols = [[
                {
                    field: 'type', title: '类型', width: 150,
                    formatter: function (value, row) {
                        return row.type;
                    },
                    editor: {
                        type: 'combobox',
                        options: {
                            valueField: 'value',
                            textField: 'text',
                            required: true,
                            editable: false,
                            data: [
                                //{ value: '入职', text: '入职' },
                                //{ value: '转正', text: '转正' },
                                //{ value: '离职', text: '离职' },
                                //{ value: '停薪留职', text: '停薪留职' },
                                { value: '奖励', text: '奖励' },
                                { value: '惩罚', text: '惩罚' },
                                { value: '岗位变动', text: '岗位变动' },
                                { value: '薪酬变动', text: '薪酬变动' },
                            ]
                        }
                    }
                },
                {
                    field: 'time', title: '时间', width: 150,
                    formatter: function (date) {
                        if (typeof date == "string") {
                            if (date.indexOf("/Date") != -1) {
                                return formatDate(date);
                            } else {
                                return date;
                            }
                        }
                    },
                    editor: {
                        type: 'datebox',
                        options: {
                            required: true,
                            editable: false,
                        }
                    }
                },
                {
                    field: 'description', title: '说明', width: 350,
                    editor: {
                        type: 'textbox',
                        options: {
                            required: true
                        }
                    }
                },

    ]];


    this.toolbar = toolbar;
}

function EmployeeCareerGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'employeeId', hidden: true },
                { field: 'status', hidden: true },
    ]];

    this.normalCols = [[
        { field: 'employeeName', title: '姓名', width: 100 },
        { field: 'employeeNumber', title: '工号', width: 100 },
        { field: 'departmentName', title: '部门', width: 100 },
        { field: 'type', title: '类型', width: 100 },
        {
            field: 'time', title: '时间', width: 150,
            formatter: function (date) {
                if (typeof date == "string") {
                    if (date.indexOf("/Date") != -1) {
                        return formatDate(date);
                    } else {
                        return date;
                    }
                }
            },
        },
        { field: 'description', title: '说明', width: 300 },

    ]];


    this.toolbar = toolbar;
}


function EmployeeBirthdayGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'number', title: '工号', width: 150 },
                { field: 'name', title: '姓名', width: 150 },
                { field: 'departmentName', title: '所在部门', width: 150, },
    ]];

    this.normalCols = [[
                { field: 'phone', title: '联系电话', width: 150 },
                { field: 'sex', title: '性别', width: 100 },
                { field: 'nation', title: '民族', width: 100 },
                { field: 'idCard', title: '身份证', width: 200 },
                //{ field: 'email', title: '工作邮箱', width: 200 },
                { field: 'birthday', title: '出生日期', width: 150, formatter: formatDate },
                { field: 'state', title: '员工状态', width: 150 },

                { field: 'entryDate', title: '入职日期', width: 200, formatter: formatDate },
                { field: 'formalDate', title: '转正日期', width: 200, formatter: formatDate },
                { field: 'leaveDate', title: '离职日期', width: 200, formatter: formatDate },

    ]];


    this.toolbar = toolbar;
}
function EmployeeContractGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
        { field: 'id', hidden: true },
        { field: 'number', title: '工号', width: 150 },
        { field: 'name', title: '姓名', width: 150 },
        { field: 'departmentName', title: '所在部门', width: 150, },
    ]];

    this.normalCols = [[
        { field: 'phone', title: '联系电话', width: 150 },
        { field: 'sex', title: '性别', width: 100 },
        { field: 'nation', title: '民族', width: 100 },
        { field: 'idCard', title: '身份证', width: 200 },
        //{ field: 'email', title: '工作邮箱', width: 200 },
        { field: 'birthday', title: '出生日期', width: 150, formatter: formatDate },
        { field: 'state', title: '员工状态', width: 150 },

        { field: 'entryDate', title: '入职日期', width: 200, formatter: formatDate },
        { field: 'formalDate', title: '转正日期', width: 200, formatter: formatDate },
        { field: 'leaveDate', title: '离职日期', width: 200, formatter: formatDate },

        { field: 'contractSerial', title: '合同编号', width: 200 },
        { field: 'contractBegin', title: '合同起始日', width: 200, formatter: formatDate },
        { field: 'contractEnd', title: '合同结束日', width: 200, formatter: formatDate },

    ]];


    this.toolbar = toolbar;
}


function AssessmentInputGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
        { field: 'id', hidden: true },
        { field: 'employeeId', hidden: true },
        { field: 'billSerial', title: '单号', width: 150 },
        { field: 'employeeNumber', title: '工号', width: 150 },
        { field: 'employeeName', title: '姓名', width: 150 },
        { field: 'departmentName', title: '部门', width: 150 },
    ]];

    this.normalCols = [[
                {
                    field: 'performanceScore', title: '绩效得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'benefitScore', title: '效益得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'shouldWorkTime', title: '应出勤', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'actualWorkTime', title: '实出勤', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'normalOvertime', title: '平日加班', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'holidayOvertime', title: '法定节假日加班', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'lateTime', title: '迟到', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'earlyTime', title: '早退', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'absenteeismTime', title: '旷工', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'personalLeaveTime', title: '事假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'sickLeaveTime', title: '病假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'annualVacationTime', title: '使用年假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },

    ]];

    this.toolbar = toolbar;
}

function AssessmentRecordGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'employeeId', hidden: true },
        { field: 'billSerial', title: '单号', width: 200 },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
                { field: 'month', title: '月份', width: 150, },

    ]];

    this.normalCols = [[
                {
                    field: 'performanceScore', title: '绩效得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'benefitScore', title: '效益得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'shouldWorkTime', title: '应出勤', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'actualWorkTime', title: '实出勤', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'normalOvertime', title: '平日加班', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'holidayOvertime', title: '法定节假日加班', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'lateTime', title: '迟到', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'earlyTime', title: '早退', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'absenteeismTime', title: '旷工', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'personalLeaveTime', title: '事假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'sickLeaveTime', title: '病假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'annualVacationTime', title: '使用年假', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                { field: 'inputDate', title: '录入时间', width: 200, formatter: formatDateTime },

                { field: 'status', title: '状态', width: 200, },
    ]];

    this.toolbar = toolbar;
}

function SalaryInputGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId', hidden: true },
        { field: 'billSerial', title: '单号', width: 200 },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
    ]];

    this.normalCols = [[
                { field: 'levelSalary', title: '层级工资', width: 150, },
                { field: 'shouldWorkTime', title: '应出勤', width: 150, },
                { field: 'actualWorkTime', title: '实出勤', width: 150, },
                { field: 'isFullAttendance', title: '是否全勤', width: 150, },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 150, styler: positiveStyler, },
                { field: 'performanceRewardsBase', title: '绩效基数', width: 150, },
                { field: 'performanceScore', title: '绩效得分', width: 150, },
                { field: 'performanceRewards', title: '绩效奖金', width: 150, styler: positiveStyler, },
                { field: 'benefitRewardsBase', title: '效益基数', width: 150, },
                { field: 'benefitScore', title: '效益得分', width: 150, },
                { field: 'benefitRewards', title: '效益奖金', width: 150, styler: positiveStyler, },
                { field: 'seniorityRewardsBae', title: '工龄奖基数', width: 150, },
                { field: 'seniorityRewards', title: '工龄奖', width: 150, styler: positiveStyler, },
                { field: 'normalOvertimeRewards', title: '工作日加班补贴', width: 150, styler: positiveStyler, },
                { field: 'holidayOvertimeRewards', title: '节假日加班补贴', width: 150, styler: positiveStyler, },
                {
                    field: 'subsidy', title: '其他补贴', width: 150, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'reissue', title: '补发', width: 150, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'socialSecurity', title: '社保', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'publicFund', title: '公积金', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'tax', title: '个人所得税', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'chargeback', title: '其他扣款', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                { field: 'shouldTotal', title: '应发工资', width: 150, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 150, styler: positiveStyler, },

    ]];


    this.toolbar = toolbar;
}

function SalaryRecordGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId', hidden: true },
        { field: 'billSerial', title: '单号', width: 200 },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
                { field: 'month', title: '月份', width: 150, },

    ]];

    this.normalCols = [[
                { field: 'levelSalary', title: '层级工资', width: 150, },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 150, styler: positiveStyler, },
                { field: 'performanceRewards', title: '绩效奖金', width: 150, styler: positiveStyler, },
                { field: 'benefitRewards', title: '效益奖金', width: 150, styler: positiveStyler, },
                { field: 'seniorityRewards', title: '工龄奖', width: 150, styler: positiveStyler, },
                { field: 'normalOvertimeRewards', title: '工作日加班补贴', width: 100, styler: positiveStyler, },
                { field: 'holidayOvertimeRewards', title: '节假日加班补贴', width: 100, styler: positiveStyler, },
                { field: 'subsidy', title: '其他补贴', width: 150, styler: positiveStyler, },
                {
                    field: 'reissue', title: '补发', width: 150, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'socialSecurity', title: '社保', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'publicFund', title: '公积金', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'tax', title: '个人所得税', width: 150, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
	            {
	                field: 'chargeback', title: '其他扣款', width: 150, styler: negativeStyler,
	                editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
	            },
                { field: 'shouldTotal', title: '应发工资', width: 150, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 150, styler: positiveStyler, },
                { field: 'inputDate', title: '录入时间', width: 200, formatter: formatDateTime },
                { field: 'status', title: '状态', width: 200 },

    ]];


    this.toolbar = toolbar;
}


function LeaveWarningGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'number', title: '工号', width: 150 },
                { field: 'name', title: '姓名', width: 150 },
                { field: 'departmentName', title: '所在部门', width: 150, },
    ]];

    this.normalCols = [[
                { field: 'grade', title: '预警等级', width: 150, formatter: formatGrade },
                { field: 'resultScore', title: '综合分数', width: 150, formatter: formatScore },
                { field: 'dimensions', hidden: true },

    ]];


    this.toolbar = toolbar;
}

function LeaveWarningDetailGrid(gridId, toolbar) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'number', title: '工号', width: 150 },
                { field: 'name', title: '姓名', width: 150 },
                { field: 'departmentName', title: '所在部门', width: 150, },
    ]];

    this.normalCols = [[
                { field: 'dimension', title: '考察维度', width: 150 },
                { field: 'score', title: '维度值得分', width: 100 },
                { field: 'average', title: '维度均值', width: 100 },
                { field: 'bias', title: '偏差', width: 100 },
    ]];


    this.toolbar = toolbar;
}

