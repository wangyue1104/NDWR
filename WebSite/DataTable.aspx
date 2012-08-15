<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTable.aspx.cs" Inherits="DataTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://datatables.net/media/css/header.ccss" rel="stylesheet" type="text/css" />
    <link href="Scripts/datatable/css/demo_page.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/datatable/css/themes/smoothness/jquery-ui-1.8.4.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="Scripts/datatable/ColReorder/css/ColReorder.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/datatable/css/demo_table_jui.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery/jquery.metadata.js" type="text/javascript"></script>
    <script src="Scripts/datatable/js/jquery.dataTables.js" type="text/javascript"></script>
    <script src="Scripts/datatable/ColReorder/js/ColReorder.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var tbl;
        $(document).ready(function () {
            $('#example').dataTable({
                "aaData": [
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 4.0",
                          "platform": "Win 95+",
                          "version": 4,
                          "grade": "X"
                      },
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 5.0",
                          "platform": "Win 95+",
                          "version": 5,
                          "grade": "C"
                      }
                    ],
                "aoColumns": [
                      { "sTitle": "Engine", "mData": "engine", "bSortable": false, "bSearchable": false },
                      { "sTitle": "Browser", "mData": "browser" },
                      { "sTitle": "Platform", "mData": "platform" },
                      { "sTitle": "Version", "mData": "version" },
                      { "sTitle": "Grade", "mData": "grade" }
                    ]
            });

            //            var oTable = $('#example')
            //                .bind('sort', function (d, e) { eventFired(d, e, 'Sort'); })
            //                .bind('filter', function (d, e) { eventFired(d, e, 'Filter'); })
            //                .bind('page', function (d, e) { eventFired(d, e, 'Page'); })
            //                .dataTable({
            //                    //"sDom": 'R<"H"lfr>t<"F"ip>',
            //                    "bJQueryUI": true,
            //                    "sPaginationType": "full_numbers"
            //                });

            tbl = $('#tb').dataTable({
//                "oLanguage": {//语言国际化
//                    "sUrl": "/Scripts/Plug-in/jquery.dataTable.cn.txt"
//                },
                "bJQueryUI": true,
                //"bServerSide": true,
                //"sAjaxSource": "DataTable.aspx?action=submit",      //mvc后台ajax调用接口。
                'bPaginate': true,                      //是否分页。
                //"bProcessing": true,                    //当datatable获取数据时候是否显示正在处理提示信息。
                'bFilter': false,                       //是否使用内置的过滤功能。
                'bLengthChange': false,                  //是否允许用户自定义每页显示条数。
                'sPaginationType': 'full_numbers',      //分页样式
                'bInfo' : false,
                "aoColumns": [
                        { "sName": "engine", "sTitle": "引擎", "mData": "engine",
                            "bSearchable": false,
                            "bSortable": false,
                            "fnRender": function (oObj) { return '<a href=\"Details/' + oObj.aData[0] + '\">View</a>'; }                           //自定义列的样式
                        },
                        { "sName": "browser", "sTitle": "浏览器" ,"mData": "engine"},
                        { "sName": "platform", "sTitle": "平台" ,"mData": "engine"},
                        { "sName": "version", "sTitle": "版本" ,"mData": "engine"}
                    ],
                "aaData": [
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 4.0",
                          "platform": "Win 95+",
                          "version": '4'
                      },
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 5.0",
                          "platform": "Win 95+",
                          "version": '5'
                      }
                    ]
            });

            //Ajax重新load控件数据。（server端）
            $("#btnTest").click(function () {
                var oSettings = tbl.fnSettings();
                oSettings.sAjaxSource = "DataTable.aspx?action=submit";
                alert(oSettings.sAjaxSource);
                tbl.fnClearTable(0);
                tbl.fnDraw();

            });

            //oTable.fnSort([[0, 'asc'], [1, 'asc']]);
        });

        function eventFired(d, e, type) {
            alert(type);
        }


        var dd = [
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 4.0",
                          "platform": "Win 95+",
                          "version": '4'
                      },
                      {
                          "engine": "Trident",
                          "browser": "Internet Explorer 5.0",
                          "platform": "Win 95+",
                          "version": '5'
                      }
                    ];
    </script>
</head>
<body>
    <table id="tb"> </table>
    <table id="example">
    </table>
    <hr />
    <table id="myDataTable" class="display">
        <thead>
            <tr>
                <th style="width: 239px;" class="sorting_disabled">
                    标识
                </th>
                <th style="width: 366px;" class="sorting">
                    公司名称
                </th>
                <th style="width: 239px;" class="sorting">
                    地址
                </th>
                <th style="width: 239px;" class="sorting">
                    城市
                </th>
            </tr>
        </thead>
        <tbody>
            <tr class="odd">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="even">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="odd">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="even">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="odd">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="even">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="odd">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="even">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="odd">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
            <tr class="even">
                <td class=" sorting_1">
                    <a href="Details/1">View</a>
                </td>
                <td>
                    公司信息
                </td>
                <td>
                    地址信息
                </td>
                <td>
                    城市信息
                </td>
            </tr>
        </tbody>
    </table>
    <input type="button" id="btnTest" value="根据条件重新响应后台Ajax" />
</body>
</html>
