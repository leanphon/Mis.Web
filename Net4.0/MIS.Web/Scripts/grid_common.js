

function fitCoulms(gridId){
	$('#' + gridId).datagrid('resize');
}


function initGrid(gridId, gridObj, url) {

	var contextMenu = $('#' + gridId + 'ContextMenu');

	$('#' + gridId).datagrid({
		width: '100%',
		method: 'POST',
		url: url,
		//data: data,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		loadMsg: '正在加载中，请稍等... ',
		nowrap: false,//允许换行
		//fitColumns: true,//宽度自适应
		//onRowContextMenu: showMenu,
		frozenColumns : gridObj.forzenCols,
		columns: gridObj.normalCols,
		toolbar: gridObj.toolbar,
		onHeaderContextMenu: function(e, field){
			e.preventDefault();

			contextMenu.menu('show', {
				left:e.pageX,
				top:e.pageY
			});
		}
	});
	createColumnMenu(gridId, contextMenu);

	var p = $('#' + gridId).datagrid('getPager');
	$(p).pagination({
		pageSize: 10,
		pageList: [10, 20, 50, 100],
		beforePageText: '第',
		afterPageText: '页 共{pages}页',
		displayMsg: '当前显示{from} - {to}条记录 共{total}条记录'
	});
}


function createColumnMenu(gridId, contextMenu){
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
		contextMenu.menu('appendItem', {
			text: col.title,
			name: field,
			iconCls: 'icon-ok'
		});
	}

}