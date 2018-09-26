

function ModuleTreeGrid(gridId, toolbarFun) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '模块名称', width: 250 },
    ]];

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}


function FunctionRightGrid(gridId, toolbarFun) {
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

    this.toolbar = [
            { text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}

function RoleGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '角色名', width: 250 },
                { field: 'description', title: '描述', width: 250 },
    ]];

    this.toolbar = [
            { text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
            { text: '权限分配', iconCls: 'icon-edit', handler: toolbarFun[3] },
    ];
}

function RoleAssignRightTreeGrid(gridId, toolbarFun) {
    this.id = gridId;
    this.idField = 'id';
    this.treeField = 'name';
    this.singleSelect = false;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'check', title: '', checkbox: true, align: 'center', width: 60 },
                { field: 'rightId', hidden: true},
                { field: 'name', title: '功能名', width: 250 },
    ]];

    this.toolbar = [
			//{ text: '保存', iconCls: 'icon-save', handler: toolbarFun[0] },
    ];
}



function CompanyRegisterGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '公司名称', width: 250 },
                { field: 'code', title: '公司简称', width: 250 },
    ]];

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}

function CompanyGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'logo', title: 'logo图片', width: 250 },
                { field: 'name', title: '公司名称', width: 250 },
                { field: 'code', title: '公司简称', width: 250 },
                { field: 'loginImg', title: '登录背景图片', width: 250 },
                { field: 'mainImg', title: '主页面背景图片', width: 250 },
    ]];

    this.toolbar = [
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
    ];
}



function PostGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'name', title: '层级名称', width: 150 },
                { field: 'code', title: '层级代码', width: 150 },
                { field: 'postSalary', title: '岗位工资', width: 150 },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 150 },
                { field: 'seniorityRewardsBase', title: '工龄奖基数', width: 150 },
    ]];

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}

function PerformanceGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'code', title: '绩效代码', width: 150 },
                { field: 'performanceRewards', title: '绩效奖金', width: 150 },
    ]];

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}

function BenefitGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
    ]];

    this.normalCols = [[
                { field: 'id', hidden: true },
                { field: 'code', title: '效益代码', width: 150 },
                { field: 'benefitRewards', title: '效益奖金', width: 150 },
    ]];

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}

function DepartmentTreeGrid(gridId, toolbarFun) {
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

    this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
    ];
}


function EmployeeGrid(gridId, toolbarFun){
    this.id = gridId;

	this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'number', title: '工号', width: 150 },
                { field: 'name', title: '姓名', width: 150 },
                { field: 'departmentName', title: '所在部门', width: 150, },
            ]];

	this.normalCols = [[
                { field: 'sex', title: '性别', width: 100 },
                { field: 'idCard', title: '身份证', width: 200 },
                { field: 'phone', title: '联系电话', width: 150 },
                { field: 'email', title: '工作邮箱', width: 200 },
                { field: 'birthday', title: '出生日期', width: 150, formatter: formatDate },
                { field: 'status', title: '在职状态', width: 150 },
                { field: 'shouldTotal', title: '标准应发月薪', width: 150 },
                { field: 'bankCard', title: '工资卡', width: 200 },
                { field: 'emergencyContact', title: '紧急联系人', width: 150 },
                { field: 'emergencyPhone', title: '紧急联系人电话', width: 150 },
                { field: 'entryDate', title: '入职日期', width: 200, formatter: formatDate },
                { field: 'formalDate', title: '转正日期', width: 200, formatter: formatDate },
                { field: 'leaveDate', title: '离职日期', width: 200, formatter: formatDate },
                { field: 'address', title: '居住地', width: 250 },
            ]];


	this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
			{ text: '薪资设定', iconCls: 'icon-money', handler: toolbarFun[3] },
            { text: '导入员工数据', iconCls: 'icon-upload', handler: toolbarFun[4] },
			{ text: '导出所有', iconCls: 'icon-export', handler: toolbarFun[5] },
		];
}



function AssessmentInputGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'employeeId', hidden: true },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
    ]];

    this.normalCols = [[
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
                {
                    field: 'performanceScore', title: '绩效得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'benefitScore', title: '效益得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
    ]];

    this.toolbar = [
			{ text: '保存当前', iconCls: 'icon-save', handler: toolbarFun[0] },
            { text: '保存全部', iconCls: 'icon-save', handler: toolbarFun[1] },

    ];
}

function AssessmentRecordGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'employeeId', hidden: true },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
                { field: 'month', title: '月份', width: 150, },

    ]];

    this.normalCols = [[
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
                {
                    field: 'performanceScore', title: '绩效得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                {
                    field: 'benefitScore', title: '效益得分', width: 150,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2 } }
                },
                { field: 'inputDate', title: '录入时间', width: 200, formatter: formatDate },
    ]];

    this.toolbar = [
			{ text: '保存当前', iconCls: 'icon-save', handler: toolbarFun[0] },
            { text: '保存全部', iconCls: 'icon-save', handler: toolbarFun[1] },

    ];
}

function SalaryInputGrid(gridId, toolbarFun) {
    this.id = gridId;

    this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId', hidden: true },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
    ]];

    this.normalCols = [[
                { field: 'postSalary', title: '岗位工资', width: 150, },
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
                { field: 'shouldTotal', title: '应发工资', width: 150, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 150, styler: positiveStyler, },

    ]];


    this.toolbar = [
			{ text: '保存当前员工', iconCls: 'icon-save', handler: toolbarFun[0] },
            { text: '全部保存', iconCls: 'icon-save', handler: toolbarFun[1] },

    ];
}

function SalaryRecordGrid(gridId, toolbarFun) {
    this.id = gridId;

	this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId',  hidden: true },
                { field: 'employeeNumber', title: '工号', width: 150 },
                { field: 'employeeName', title: '姓名', width: 150 },
                { field: 'departmentName', title: '部门', width: 150 },
                { field: 'month', title: '月份', width: 150, },

            ]];

	this.normalCols = [[
                { field: 'postSalary', title: '岗位工资', width: 150, },
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
                { field: 'shouldTotal', title: '应发工资', width: 150, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 150, styler: positiveStyler, },
                { field: 'inputDate', title: '录入时间', width: 200, formatter: formatDate },

            ]];


	this.toolbar = [
			{ text: '修改', iconCls: 'icon-save', handler: toolbarFun[0]  },
			{ text: '锁定', iconCls: 'icon-lock', handler: toolbarFun[1]  },
			{ text: '解锁', iconCls: 'icon-unlock', handler: toolbarFun[2]  },
			{ text: '导出所有', iconCls: 'icon-export', handler: toolbarFun[3]  },
		];
}


