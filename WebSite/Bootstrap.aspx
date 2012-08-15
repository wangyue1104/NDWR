<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bootstrap.aspx.cs" Inherits="Bootstrap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>

    <link rel="stylesheet" type="text/css" href="Scripts/bootstrap/css/bootstrap.css" />
    <link href="http://twitter.github.com/bootstrap/assets/css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="Scripts/datatable/plugins/DT_bootstrap.css" />
    <link href="Scripts/bootstrap/plugins/select2/css/select2-2.1.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Scripts/datatable/js/jquery.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.metadata.js"></script>
    <script type="text/javascript" src="Scripts/datatable/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap/js/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/datatable/plugins/DT_bootstrap.js?dd"></script>
    <script src="Scripts/bootstrap/plugins/select2/js/select2-2.1.js" type="text/javascript"></script>
    <script src="<%=BasePath %>ndwr/ndwrcore.js" type="text/javascript"></script>
    <script src="<%=BasePath %>ndwr/RemoteDemo.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/datatable-ndwr.js"></script>

    <style type="text/css">
      body {
        padding-top: 60px;
        padding-bottom: 40px;
      }
      .sidebar-nav {
        padding: 9px 0;
      }
    </style>
    <script type="text/javascript" charset="utf-8">
        var oTable;
        var formArg = null;
        $(document).ready(function () {

            oTable = $('#example').dataTable({
                "bServerSide": true,
                "iDisplayLength": 5,
                "bPaginate": true,
                'bFilter': false,
                'bLengthChange': false,
                "oLanguage": { "sUrl": "Scripts/datatable/language/dt_zh.txt" },
                "sPaginationType": "bootstrap",
                "iFullNumbersShowPages": 10,
                "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                    if (formArg == null) {
                        fnCallback({ "sEcho": 0, "iTotalRecords": 0, "iTotalDisplayRecords": 0, "aaData": null });
                        return;
                    }
                    RemoteDemo.DataTable(convert(aoData), function (data) {
                        fnCallback(data);
                        location.href = "#listSection"; // 书签跳转
                    });
                },
                "aoColumns": [
                      {
                          "sTitle": "索引", "mData": "Id", "bSortable": false, "bSearchable": false,
                          "fnRender": function (obj) {
                              return '<a href="javascript:void(0);">' + obj.aData.Id + '</a>';
                          }
                      },
                      { "sTitle": "账号", "mData": "Name" },
                      { "sTitle": "密码", "mData": "Pswd" }
                ]
            });

            $("#smt").click(function () {
                formArg = new Array();
                var oSettings = oTable.fnSettings();
                oTable.fnClearTable(0);
                oTable.fnDraw();
                //return false;
            });

            $("#selectError").select2({ placeholder: "请选择一个数字" });

        });


    </script>
</head>
<body>
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container-fluid">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span><span class="icon-bar"></span>
                </a>
                <a class="brand" href="#">Project name</a>
                <div class="btn-group pull-right">
                    <a class="btn dropdown-toggle" data-toggle="dropdown" href="#"><i class="icon-user">
                        </i>Username <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a href="#">Modify Password</a></li>
                        <li class="divider"></li>
                        <li><a href="#">Sign Out</a></li>
                    </ul>
                </div>
                <div class="nav-collapse">
                    <ul class="nav">
                        <li class="active"><a href="Index.aspx">Home</a></li>
                        <li><a href="#about">About</a></li>
                        <li><a href="#contact">Contact</a></li>
                    </ul>
                </div>
                <!--/.nav-collapse -->
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span2">
                <div class="well sidebar-nav">
                    <ul class="nav nav-list">
                        <li class="nav-header">Sidebar</li>
                        <li class="active"><a href="#listSection">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li class="nav-header">Sidebar</li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li class="nav-header">Sidebar</li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                        <li><a href="#">Link</a></li>
                    </ul>
                </div>
                <!--/.well -->
            </div>

            <!--/span-->
            <div class="span10">

                <form class="form-horizontal">
                    <fieldset>
                        <legend>Legend text</legend>
                        <div class="control-group">
                            <label class="control-label" for="input01">Text input</label>
                            <div class="controls">
                                <input type="text" class="input-xlarge" id="input01">
                                <input type="text" class="input-xlarge" id="input02">
                                <span class="help-inline">Supporting help text</span>
                                <%--<p class="help-block">Supporting help text</p>--%>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Uneditable input</label>
                            <div class="controls">
                                <span class="input-xlarge uneditable-input">Some value here</span>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="disabledInput">Disabled input</label>
                            <div class="controls">
                                <input class="input-xlarge disabled" id="disabledInput" type="text" placeholder="Disabled input here…" disabled>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label" for="optionsCheckbox2">Disabled checkbox</label>
                            <div class="controls">
                                <label class="checkbox">
                                    <input type="checkbox" id="optionsCheckbox2" value="option1" disabled>This is a disabled checkbox
                                </label>
                            </div>
                        </div>
                        <div class="control-group warning">
                            <label class="control-label" for="inputWarning">Input with warning</label>
                            <div class="controls">
                                <input type="text" id="inputWarning">
                                <span class="help-inline">Something may have gone wrong</span>
                            </div>
                        </div>
                        <div class="control-group error">
                            <label class="control-label" for="inputError">Input with error</label>
                            <div class="controls">
                                <input type="text" id="inputError">
                                <span class="help-inline">Please correct the error</span>
                            </div>
                        </div>
                        <div class="control-group success">
                            <label class="control-label" for="inputSuccess">Input with success</label>
                            <div class="controls">
                                <input type="text" id="inputSuccess">
                                <span class="help-inline">Woohoo!</span>
                            </div>
                        </div>
                        <div class="control-group success">
                            <label class="control-label" for="selectError">Select with success</label>
                            <div class="controls">
                                <select id="selectError">
                                    <option>1</option>
                                    <option>2</option>
                                    <option>3</option>
                                    <option>4</option>
                                    <option>5</option>
                                </select>
                                <span class="help-inline">Woohoo!</span>
                            </div>
                        </div>
                        <div class="control-group success">
                            <label class="control-label" for="selectError">Select with success</label>
                            <div class="controls">
                                <div class="btn-group" data-toggle="buttons-radio">
                                  <button class="btn" type="button">Left</button>
                                  <button class="btn" type="button">Middle</button>
                                  <button class="btn" type="button">Right</button>
                                </div>
                                <span class="help-inline">Woohoo!</span>
                            </div>
                        </div>
                        <div class="form-actions">
                            <button type="button" class="btn btn-primary" id="smt">Save changes</button>
                        </div>
                        <%--<div class="control-group">
                            <label class="control-label" for="smt"></label>
                            <div class="controls">
                                <button class="btn" id="smt">Cancel</button>
                            </div>
                        </div>--%>
                    </fieldset>
                </form>

                <a name="listSection"></a><%--列表书签--%>
                <table id="example" class="table table-striped table-bordered" style="width: 100%;"></table>
                
            </div>
            <!--/span-->
        </div>
        <!--/row-->
        <hr>
        <footer>
            <p>&copy; Company 2012</p>
        </footer>
    </div>
    <!--/.fluid-container-->

</body>
</html>
