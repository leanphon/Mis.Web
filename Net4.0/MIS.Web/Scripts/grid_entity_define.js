
function EmployeeGrid(toolbarFun){

	this.forzenCols = [[
                { field: 'id', title: 'id', hidden: true },
                { field: 'number', title: '工号', width: 80 },
                { field: 'name', title: '姓名', width: 80 },
                { field: 'departmentName', title: '所在部门', width: 100, },
            ]];

	this.normalCols = [[
                { field: 'id', title: 'id', width: 80, hidden: true },
                { field: 'number', title: '工号', width: 80 },
                { field: 'name', title: '姓名', width: 80 },
                { field: 'departmentName', title: '所在部门', width: 100, },
                { field: 'sex', title: '性别', width: 50 },
                { field: 'idCard', title: '身份证', width: 160 },
                { field: 'phone', title: '联系电话', width: 100 },
                { field: 'birthday', title: '出生日期', width: 100, formatter: formatDate },
                { field: 'status', title: '在职状态', width: 100 },
                { field: 'bankCard', title: '工资卡', width: 160 },
                { field: 'emergencyContact', title: '紧急联系人', width: 80 },
                { field: 'emergencyPhone', title: '紧急联系人电话', width: 100 },
                { field: 'entryDate', title: '入职日期', width: 100, formatter: formatDate },
                { field: 'formalDate', title: '转正日期', width: 100, formatter: formatDate },
                { field: 'leaveDate', title: '离职日期', width: 100, formatter: formatDate },
				{ field: 'bankCard', title: '工资卡', width: 160 },
                { field: 'emergencyContact', title: '紧急联系人', width: 80 },
                { field: 'emergencyPhone', title: '紧急联系人电话', width: 100 },
                { field: 'entryDate', title: '入职日期', width: 100, formatter: formatDate },
                { field: 'formalDate', title: '转正日期', width: 100, formatter: formatDate },
                { field: 'leaveDate', title: '离职日期', width: 100, formatter: formatDate },
            ]];


	this.toolbar = [
			{ text: '增加', iconCls: 'icon-add', handler: toolbarFun[0] },
			{ text: '修改', iconCls: 'icon-edit', handler: toolbarFun[1] },
			{ text: '删除', iconCls: 'icon-remove', handler: toolbarFun[2] },
			{ text: '薪资设定', iconCls: 'icon-money', handler: toolbarFun[3] },

		];
}


function SalaryInputGrid(toolbarFun){

	this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId',  hidden: true },
                { field: 'employeeNumber', title: '工号', width: 90 },
                { field: 'employeeName', title: '姓名', width: 80 },
                { field: 'departmentName', title: '部门', width: 80 },
            ]];

	this.normalCols = [[
                { field: 'postSalary', title: '岗位工资', width: 80, },
                { field: 'shouldWorkTime', title: '应出勤', width: 80, },
                { field: 'actualWorkTime', title: '实出勤', width: 80, },
                { field: 'isFullAttendance', title: '是否全勤', width: 80, },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 80, styler: positiveStyler, },
                { field: 'performanceRewardsBase', title: '绩效基数', width: 80, },
                { field: 'performanceScore', title: '绩效得分', width: 80, },
                { field: 'performanceRewards', title: '绩效奖金', width: 80, styler: positiveStyler, },
                { field: 'benefitRewardsBase', title: '效益基数', width: 80, },
                { field: 'benefitScore', title: '效益得分', width: 80, },
                { field: 'benefitRewards', title: '效益奖金', width: 80, styler: positiveStyler, },
                { field: 'seniorityRewardsBae', title: '工龄奖基数', width: 80, },
                { field: 'seniorityRewards', title: '工龄奖', width: 80, styler: positiveStyler, },
                { field: 'normalOvertimeRewards', title: '工作日加班补贴', width: 100, styler: positiveStyler, },
                { field: 'holidayOvertimeRewards', title: '节假日加班补贴', width: 100, styler: positiveStyler, },
                {
                    field: 'subsidy', title: '其他补贴', width: 80, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'reissue', title: '补发', width: 80, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'socialSecurity', title: '社保', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'publicFund', title: '公积金', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'tax', title: '个人所得税', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                { field: 'shouldTotal', title: '应发工资', width: 80, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 80, styler: positiveStyler, }, 

            ]];


	this.toolbar = [
			{ text: '保存当前员工', iconCls: 'icon-save', handler: toolbarFun[0] },
            { text: '全部保存', iconCls: 'icon-save', handler: toolbarFun[1] },

		];
}


function SalaryRecordGrid(toolbarFun){

	this.forzenCols = [[
                { field: 'id', hidden: true },
                { field: 'assessmentInfoId', hidden: true },
                { field: 'employeeId',  hidden: true },
                { field: 'employeeNumber', title: '工号', width: 90 },
                { field: 'employeeName', title: '姓名', width: 80 },
                { field: 'departmentName', title: '部门', width: 80 },

            ]];

	this.normalCols = [[
                { field: 'month', title: '月份', width: 80, },
                { field: 'postSalary', title: '岗位工资', width: 80, },
                { field: 'fullAttendanceRewards', title: '全勤奖', width: 80, styler: positiveStyler, },
                { field: 'performanceRewards', title: '绩效奖金', width: 80, styler: positiveStyler, },
                { field: 'benefitRewards', title: '效益奖金', width: 80, styler: positiveStyler, },
                { field: 'seniorityRewards', title: '工龄奖', width: 80, styler: positiveStyler, },
                { field: 'normalOvertimeRewards', title: '工作日加班补贴', width: 100, styler: positiveStyler, },
                { field: 'holidayOvertimeRewards', title: '节假日加班补贴', width: 100, styler: positiveStyler, },
                { field: 'subsidy', title: '其他补贴', width: 80, styler: positiveStyler, },
                {
                    field: 'reissue', title: '补发', width: 80, styler: positiveStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'socialSecurity', title: '社保', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'publicFund', title: '公积金', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                {
                    field: 'tax', title: '个人所得税', width: 80, styler: negativeStyler,
                    editor: { type: 'numberbox', options: { min: 0, precision: 2, required: true } }
                },
                { field: 'shouldTotal', title: '应发工资', width: 80, styler: positiveStyler, },
                { field: 'actualTotal', title: '实发工资', width: 80, styler: positiveStyler, },
                { field: 'inputDate', title: '录入时间', width: 100, formatter: formatDate },

            ]];


	this.toolbar = [
			{ text: '修改', iconCls: 'icon-save', handler: toolbarFun[0]  },
			{ text: '锁定', iconCls: 'icon-lock', handler: toolbarFun[1]  },
			{ text: '解锁', iconCls: 'icon-unlock', handler: toolbarFun[2]  },
			{ text: '导出所有', iconCls: 'icon-export', handler: toolbarFun[3]  },
		];
}

