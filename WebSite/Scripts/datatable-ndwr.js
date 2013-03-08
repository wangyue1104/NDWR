/**
*  作  者: 王跃
*  用  途： datatable组件中table数据收集
**/

/**
  * "fnServerData": function (sSource, aoData, fnCallback, oSettings)
  * 中aoData 的数据格式
[
	{ "name": "sEcho", "value": 3 }, --传输的批次号
	{ "name": "iColumns", "value": 3 },  -- 表格列数
	{ "name": "sColumns", "value": "" },  -- 
	{ "name": "iDisplayStart", "value": 0 },  -- 起始记录索引
	{ "name": "iDisplayLength", "value": 10 },  -- 显示每页显示记录数
	{ "name": "mDataProp_0", "value": "Id" },  -- 第一列属性名
	{ "name": "mDataProp_1", "value": "Name" },  -- 第二列属性名
	{ "name": "mDataProp_2", "value": "Pswd" },  -- 第三列属性名
	{ "name": "sSearch", "value": "" },  -- 查询关键词
	{ "name": "bRegex", "value": false },  
	{ "name": "sSearch_0", "value": "" },
	{ "name": "bRegex_0", "value": false },
	{ "name": "bSearchable_0", "value": false },
	{ "name": "sSearch_1", "value": "" },
	{ "name": "bRegex_1", "value": false },
	{ "name": "bSearchable_1", "value": true },
	{ "name": "sSearch_2", "value": "" },
	{ "name": "bRegex_2", "value": false },
	{ "name": "bSearchable_2", "value": true },
	{ "name": "iSortCol_0", "value": 2 },  -- 第一个排序列的索引
	{ "name": "sSortDir_0", "value": "asc" },  -- 第一个排序列的索引
	{ "name": "iSortingCols", "value": 1 },  -- 排序列个数
	{ "name": "bSortable_0", "value": false },  -- 第一列是否可排序
	{ "name": "bSortable_1", "value": true },  -- 第二列是否可排序
	{ "name": "bSortable_2", "value": true }  -- 第三列是否可排序
]
*/


function convert(aoData) {
    if (aoData == null || aoData.length == 0) {
        return nul;
    }

    function get(key) {
        for (var i = 0; i < aoData.length; i++) {
            if (aoData[i].name == key) {
                return aoData[i].value;
            }
        }
        return null;
    }

    var ndwrTable = {};

    ndwrTable.sEcho = get('sEcho');
    ndwrTable.iDisplayStart = get('iDisplayStart');
    ndwrTable.iDisplayLength = get('iDisplayLength');
    ndwrTable.sSearch = get('sSearch');
    var iSortingCols = get('iSortingCols');
    if (iSortingCols > 0) { // 这里只针对一列排序，不能多列排序
        var nCol = get('iSortCol_0');
        ndwrTable.orderbyColName = get('mDataProp_' + nCol);
        ndwrTable.sortType = get('sSortDir_0');
    }

    return ndwrTable;
}