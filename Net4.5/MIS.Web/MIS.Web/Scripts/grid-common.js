/*

*/

/**************************************** begin datagrid 相关  *************************/


function initDatagrid(gridEntity, url, callbackFuns)
{
    var gridObj = $('#' + gridEntity.id)
    gridObj.datagrid({}); //初始化

    if (callbackFuns)
    {
        for (var i=0; i<callbackFuns.length; i++)
        {
            gridObj.datagrid('options')[callbackFuns[i].name] = callbackFuns[i].fun;
        }

    }
    
    gridObj.datagrid({
		width: '100%',
		method: 'POST',
		url: url,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		loadMsg: '正在加载中，请稍等... ',
		nowrap: false,//允许换行
		//fitColumns: true,//宽度自适应
		frozenColumns: gridEntity.forzenCols,
		columns: gridEntity.normalCols,
		toolbar: gridEntity.toolbar,

		onHeaderContextMenu: function (e, field) {
			e.preventDefault();

			contextMenu.menu('show', {
				left:e.pageX,
				top:e.pageY
			});
		}
    });
    

    var p = gridObj.datagrid('getPager');
	$(p).pagination({
		pageSize: 10,
		pageList: [10, 20, 50, 100],
		beforePageText: '第',
		afterPageText: '页 共{pages}页',
		displayMsg: '当前显示{from} - {to}条记录 共{total}条记录'
	});
	var contextMenu = $('#' + gridEntity.id + 'ContextMenu');
	createDatagridHeaderMenu(gridEntity.id, contextMenu);

    //用于保存编辑状态的行下标
    gridObj.attr("editIndex", -1);
}


function onDatagridAdjust(gridId) {
    $('#' + gridId).datagrid('resize');
}

function createDatagridHeaderMenu(gridId, contextMenu){
	//contextMenu = $('<div/>').appendTo('body');
	contextMenu.menu({
		onClick: function(item){
			if (item.iconCls == 'icon-ok'){
				$('#' + gridId).datagrid('hideColumn', item.name);
				contextMenu.menu('setIcon', {
					target: item.target,
					iconCls: 'icon-empty'
				});
			} else {
				$('#' + gridId).datagrid('showColumn', item.name);
				contextMenu.menu('setIcon', {
					target: item.target,
					iconCls: 'icon-ok'
				});
			}
		}
	});

	var fields = $('#' + gridId).datagrid('getColumnFields');

	for(var i=0; i<fields.length; i++){
		var field = fields[i];
		var col = $('#' + gridId).datagrid('getColumnOption', field);
		if (!col.hidden) {
		    contextMenu.menu('appendItem', {
		        text: col.title,
		        name: field,
		        iconCls: 'icon-ok'
		    });
		}
	}

}


function onDatagridRowContextMenu(e, rowIndex, rowData) { //右键时触发事件
    //三个参数：e里面的内容很多，真心不明白，rowIndex就是当前点击时所在行的索引，rowData当前行的数据
    e.preventDefault(); //阻止浏览器捕获右键事件

    $(this).datagrid("selectRow", rowIndex); //根据索引选中该行
    var obj = $("#" + $(this).attr('id') + "Menu")
    obj.menu('show', {
        //显示右键菜单
        left: e.pageX,//在鼠标点击处显示菜单
        top: e.pageY
    });
}

/************** begin 表格编辑相关  *************/

function onClickRow(index) {
    var editIndex = $(this).attr("editIndex")
     
    if (editIndex != index) {
        if (endEditing($(this))) {
            $(this).datagrid('selectRow', editIndex);

        } else {

        }
    }
}

function onDblClickRow(index) {
    var editIndex = $(this).attr("editIndex");

    if (editIndex != index) {
        if (endEditing($(this))) {
            $(this).datagrid('selectRow', index);

            var row = $(this).datagrid('getSelected');

            $.data(this, "lastRowData", row)


            if ($(this).datagrid('beginEdit', index)) {
                $(this).attr("editIndex", index);
            }

        } else {
            $(this).datagrid('selectRow', editIndex);
        }
    }
}

function endEditing(gridObj) {
    var editIndex = gridObj.attr("editIndex");

    if (editIndex == -1) {
        return true
    }

    if (gridObj.datagrid('validateRow', editIndex)) {
        gridObj.datagrid('endEdit', editIndex);


        var lastData = $.data(gridObj.get(0), "lastRowData")
        console.log(lastData)

        var row = { index: editIndex, row: lastData }
        gridObj.datagrid('updateRow', row);

        
        gridObj.attr("editIndex", -1);

        return true;
    } else {
        return false;
    }
}

function ExitEditing(gridObj) {
    console.log(gridObj)
    var editIndex = gridObj.attr("editIndex");

    if (editIndex == -1) {
        return true
    }

    if (gridObj.datagrid('validateRow', editIndex)) {
        gridObj.datagrid('endEdit', editIndex);


        gridObj.attr("editIndex", -1);

        return true;
    } else {
        return false;
    }
}



/************** end 表格编辑相关  *************/
/**************************************** end datagrid 相关  *************************/


/**************************************** begin treegrid 相关  *************************/

function initTreegrid(gridEntity, url, callbackFuns) {
    var gridObj = $('#' + gridEntity.id)
    gridObj.treegrid({}); //初始化

    if (callbackFuns) {
        for (var i = 0; i < callbackFuns.length; i++) {
            gridObj.treegrid('options')[callbackFuns[i].name] = callbackFuns[i].fun;
        }

    }

    gridObj.treegrid({
        width: '100%',
        method: 'POST',
        url: url,
        //data: data,
        pagination: gridEntity.pager,
        singleSelect: gridEntity.singleSelect,
        rownumbers: true,
        loadMsg: '正在加载中，请稍等... ',
        nowrap: false,//允许换行
        //fitColumns: true,//宽度自适应
        idField: gridEntity.idField,
        treeField: gridEntity.treeField,
        frozenColumns: gridEntity.forzenCols,
        columns: gridEntity.normalCols,
        toolbar: gridEntity.toolbar,
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();

            contextMenu.menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }
    });
    if (gridEntity.pager == true)
    {
        console.log(gridEntity)

        var p = gridObj.treegrid('getPager');
        $(p).pagination({
            pageSize: 10,
            pageList: [10, 20, 50, 100],
            beforePageText: '第',
            afterPageText: '页 共{pages}页',
            displayMsg: '当前显示{from} - {to}条记录 共{total}条记录'
        });
    }

    var contextMenu = $('#' + gridEntity.id + 'ContextMenu');
    createTreegridHeaderMenu(gridEntity.id, contextMenu);
}

function onTreegridAdjust(gridId) {
    $('#' + gridId).treegrid('resize');
}

function createTreegridHeaderMenu(gridId, contextMenu) {
    //contextMenu = $('<div/>').appendTo('body');
    contextMenu.menu({
        onClick: function (item) {
            if (item.iconCls == 'icon-ok') {
                $('#' + gridId).treegrid('hideColumn', item.name);
                contextMenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-empty'
                });
            } else {
                $('#' + gridId).treegrid('showColumn', item.name);
                contextMenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-ok'
                });
            }
        }
    });

    var fields = $('#' + gridId).treegrid('getColumnFields');

    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        var col = $('#' + gridId).treegrid('getColumnOption', field);
        if (!col.hidden) {
            contextMenu.menu('appendItem', {
                text: col.title,
                name: field,
                iconCls: 'icon-ok'
            });
        }
    }

}


function onTreegridonContextMenu(e, row) { //右键时触发事件
    //三个参数：e里面的内容很多，真心不明白，rowIndex就是当前点击时所在行的索引，rowData当前行的数据
    e.preventDefault(); //阻止浏览器捕获右键事件

    $(this).treegrid("selectRow", row.id); //根据索引选中该行

    $('#menu').menu('show', {
        //显示右键菜单
        left: e.pageX,//在鼠标点击处显示菜单
        top: e.pageY
    });
}

/**************************************** end treegrid 相关  *************************/


/**************************************** begin propertygrid 相关  *************************/
function initPropertygrid(gridEntity, url, callbackFuns) {
    var gridObj = $('#' + gridEntity.id)
    gridObj.propertygrid({}); //初始化

    if (callbackFuns) {
        for (var i = 0; i < callbackFuns.length; i++) {
            gridObj.propertygrid('options')[callbackFuns[i].name] = callbackFuns[i].fun;
        }

    }

    gridObj.propertygrid({
        width: '100%',
        method: 'POST',
        url: url,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        loadMsg: '正在加载中，请稍等... ',
        nowrap: false,//允许换行
        //fitColumns: true,//宽度自适应
        frozenColumns: gridEntity.forzenCols,
        columns: gridEntity.normalCols,
        toolbar: gridEntity.toolbar,

    });


    var p = gridObj.propertygrid('getPager');
    $(p).pagination({
        pageSize: 10,
        pageList: [10, 20, 50, 100],
        beforePageText: '第',
        afterPageText: '页 共{pages}页',
        displayMsg: '当前显示{from} - {to}条记录 共{total}条记录'
    });

}


function onPropertyridAdjust(gridId) {
    $('#' + gridId).propertygrid('resize');
}

/**************************************** end propertygrid 相关  *************************/

