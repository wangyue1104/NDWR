<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTable2.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<link href="http://datatables.net/media/css/header.ccss" rel="stylesheet" type="text/css" />--%>
    <link href="Scripts/datatable/css/demo_page.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/datatable/css/themes/smoothness/jquery-ui-1.8.4.custom.css" rel="stylesheet" type="text/css" />
    <%--<link href="Scripts/datatable/ColReorder/css/ColReorder.css" rel="stylesheet" type="text/css" />--%>
    <link href="Scripts/datatable/css/demo_table_jui.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery/jquery.metadata.js" type="text/javascript"></script>
    <script src="Scripts/datatable/js/jquery.dataTables.js" type="text/javascript"></script>
    <%--<script src="Scripts/datatable/ColReorder/js/ColReorder.js" type="text/javascript"></script>--%>

    <script src="<%=BasePath %>ndwr/ndwrcore.js?version=12dsdfdfdsfdsfsd" type="text/javascript"></script>
    <script src="<%=BasePath %>ndwr/RemoteDemo.js?version=23" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/datatable-ndwr.js"></script>

    <script type="text/javascript" charset="utf-8">
        var oTable;
        $(document).ready(function () {
            var nCloneTh = document.createElement('th');
            var nCloneTd = document.createElement('td');
            nCloneTd.innerHTML = '<img src="../examples_support/details_open.png">';
            nCloneTd.className = "center";

            $('#example thead tr').each(function () {
                this.insertBefore(nCloneTh, this.childNodes[0]);
            });

            $('#example tbody tr').each(function () {
                this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
            });


            oTable = $('#example').dataTable({
                "bJQueryUI": true,
                "bProcessing": true,
                "bServerSide": true,
                "sScrollX": "100%",
                "iDisplayLength": 10,
                "bPaginate": true,
                "oLanguage": { "sUrl": "Scripts/datatable/language/dt_zh.txt" },
                "sPaginationType": "full_numbers",
                //"fnServerData": function (sSour<a href="Scripts/datatable/ColReorder/">Scripts/datatable/ColReorder/</a>ce, aoData, fnCallback, oSettings) {
                //    RemoteDemo.DataTable(convert(aoData), function (data) {
                //        fnCallback(data);
                //    });
                //},
                "aoColumns": [
                      {
                          "sTitle": "索引", "mData": "Id", "bSortable": false, "bSearchable": false,
                          "fnRender": function (obj) {
                              return '<a href="javascript:void(0);">' +obj.aData.Id +'</a>';
                          }
                      },
                      { "sTitle": "账号", "mData": "Name" },
                      { "sTitle": "密码", "mData": "Pswd" }
                    ]
            });



            //Ajax重新load控件数据。（server端）
            $("#btn").click(function () {
                var oSettings = oTable.fnSettings();
                //oSettings.bProcessing = true;
                //oSettings.bServerSide = true;
                oSettings.fnServerData =  function (sSource, aoData, fnCallback, oSettings) {
                    RemoteDemo.DataTable(convert(aoData), function (data) {
                        fnCallback(data);
                    });
                };
                oTable.fnClearTable(0);
                oTable.fnDraw();

            });

            /*加入展开，收缩每一行详情按钮用*/
            $('#example tbody td a').live('click', function () {
                var nTr = $(this).parents('tr')[0];
                if (oTable.fnIsOpen(nTr)) {
                    this.src = "./style/datatable/images/details_open.png";
                    oTable.fnClose(nTr);
                } else {
                    this.src = "./style/datatable/images/details_close.png";
                    oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
                }
            });

        });


        /* 构造每一行详情的函数 fnFormatDetails*/
        function fnFormatDetails(oTable, nTr) {
            var aData = oTable.fnGetData(nTr);
            var sOut = '<table cellpadding="6" cellspacing="0" border="0" style="padding-left:50px;">'; //在这里定义要返回的内容
            sOut += '<tr><td>Rendering engine:</td><td>' + aData.Id + ' ' + aData.Name +  '' + aData.Pswd + '</td></tr>';
            sOut += '<tr><td>Link to source:</td><td>Could provide a link here</td></tr>';
            sOut += '<tr><td>Extra info:</td><td>And any further details here (images etc)</td></tr>';
            sOut += '</table>';
            return sOut;
        }
         
    </script>


</head>
<body>
    <div>
        <table id="example">
        </table>
    </div>

    <div>
        <button id="btn" >提交</button>
    </div>
</body>
</html>
