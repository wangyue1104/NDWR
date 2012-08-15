<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTable-BootStrap.aspx.cs"
    Inherits="DataTable_BootStrap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Scripts/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="Scripts/datatable/plugins/DT_bootstrap.css" />

    <script type="text/javascript" src="Scripts/jquery/jquery-1.7.2.js" ></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.metadata.js"></script>
    <script type="text/javascript" src="Scripts/jquery/validate/jquery.validate.js" ></script>
    <script type="text/javascript" src="Scripts/jquery/validate/jquery.validate.bootstrap.js" ></script>

    <script type="text/javascript" src="Scripts/datatable/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap/js/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/datatable/plugins/DT_bootstrap.js"></script>

    <script type="text/javascript" src="<%=BasePath %>ndwr/ndwrcore.js" ></script>
    <script type="text/javascript" src="<%=BasePath %>ndwr/RemoteDemo.js" ></script>
    <script type="text/javascript" src="Scripts/datatable-ndwr.js"></script>


    <script type="text/javascript" charset="utf-8">
        var oTable;
        var formArg = null;
        $(document).ready(function () {

            oTable = $('#example').dataTable({
                //"bJQueryUI": true,
                //"sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
                //"bProcessing": true,
                "bServerSide": true,
                //"sScrollX": "100%",
                "iDisplayLength": 5,
                "bPaginate": true,
                'bFilter': false,
                'bLengthChange': false,
                "oLanguage": { "sUrl": "Scripts/datatable/language/dt_zh.txt" },
                //"sPaginationType": "full_numbers",
                "sPaginationType": "bootstrap",
                "iFullNumbersShowPages": 10,
                "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                    if (formArg == null) {
                        fnCallback({ "sEcho": 0, "iTotalRecords": 0, "iTotalDisplayRecords": 0, "aaData": null });
                        return;
                    }
                    RemoteDemo.DataTable(convert(aoData), function (data) {
                        fnCallback(data);
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



            //Ajax重新load控件数据。（server端）
            $("#btn").click(function () {
                var oSettings = oTable.fnSettings();
                //oSettings.bProcessing = true;
                oSettings.fnServerData = function (sSource, aoData, fnCallback, oSettings) {
                    RemoteDemo.DataTable(convert(aoData), function (data) {
                        fnCallback(data);
                    });
                };
                oTable.fnClearTable(0);
                oTable.fnDraw();

            });

            $("#btn2").click(function () {
                formArg = new Array();
                var oSettings = oTable.fnSettings();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            });

            // 验证
            $("#signupForm").validate({
                rules: {
                    firstname: "required",
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true,
                        minlength: 5
                    },
                    confirm_password: {
                        required: true,
                        minlength: 5,
                        equalTo: "#password"
                    },
                    birthday: {
                        required: function (ele) { return $('#input01').val().length > 0 ? false : true; },
                        date: true
                    }
                },
                messages: {
                    firstname: "请输入姓名",
                    email: {
                        required: "请输入Email地址",
                        email: "请输入正确的email地址"
                    },
                    password: {
                        required: "请输入密码",
                        minlength: jQuery.format("密码不能小于{0}个字符")
                    },
                    confirm_password: {
                        required: "请输入确认密码",
                        minlength: "确认密码不能小于5个字符",
                        equalTo: "两次输入密码不一致不一致"
                    },
                    birthday: {
                        required : "只有在不选订单情况下才可空",
                        date : "日期格式不正确"
                    }
                },
                submitHandler: function (form) {
                    alert("submitted");
                    form.submit();
                }
            });


            $("#validateBtn").click(function () {
            });
        });


    </script>
</head>
<body>
    <%--<div style="width: 1000px; height: 50px;">
    </div>--%>
    <div class="container-fluid" style="margin-top: 10px">
        <div class="row-fluid">
        <form class="form-horizontal"  id="signupForm">
        <fieldset>
            <legend>Legend text</legend>
            <div class="control-group">
                <label class="control-label" for="input01">Text input</label>
                <div class="controls">
                    <input type="text" class="input-xlarge" id="input01">
                    <p class="help-block">Supporting help text</p>
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


            <div class="control-group">
                <label class="control-label" for="firstname">First Name</label>
                <div class="controls">
                    <input class="input-xlarge" id="firstname" name="firstname" type="text" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="email">E-Mail</label>
                <div class="controls">
                    <input class="input-xlarge" id="email" name="email" type="text" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="password">Password</label>
                <div class="controls">
                    <input class="input-xlarge" id="password" name="password" type="password" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="confirm_password">RePassword</label>
                <div class="controls">
                    <input class="input-xlarge" id="confirm_password" name="confirm_password" type="password" />
                </div>
            </div>
            <div class="control-group">
                <label class="control-label" for="birthday">Birthday</label>
                <div class="controls">
                    <input class="input-xlarge" id="birthday" name="birthday" type="text" />
                </div>
            </div>


            <div class="form-actions">
                <button type="submit" class="btn btn-primary">Save changes</button>
                <button class="btn">Cancel</button>
                <button type="button" class="btn" id="validateBtn">validateBtn</button>
            </div>
        </fieldset>
        </form>


        <table id="example" class="table table-striped table-bordered" style="width: 100%;">
        </table>


        <div style="clear: both;">
            <button id="btn2" class="btn">提交2</button>
        </div>
        </div>
    </div>
</body>
</html>
