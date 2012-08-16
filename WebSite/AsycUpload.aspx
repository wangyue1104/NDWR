<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AsycUpload.aspx.cs" Inherits="AsycUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="Scripts/jquery/jquery-1.7.2.js" type="text/javascript"></script>

    <script type="text/javascript">
        var idname = 'uploadifm'; // iframe 名字
        $(document).ready(function () {
            (function () {
                var div = document.createElement("div");
                // Add the div to the document first, otherwise IE 6 will ignore onload handler.
                document.body.appendChild(div);
                div.innerHTML = "<iframe src='javascript:void(0)' frameborder='0' style='width: 0px;height: 0px; border: 0;' id='" + idname + "' name='" + idname + "' onload='loadingComplete();'></iframe>";
            })();

            $('#smt').click(function () {
                var formHtml = '<form id="dwr-form" action="AsycUpload.aspx?action=upload" target="' + idname + '" style="display:none;" method="post" enctype="multipart/form-data"></form>';
                var div = document.createElement("div");
                div.innerHTML = formHtml;
                var form = div.firstChild;

                var iptFileEle = document.getElementById('iptFile'); // 上传文件控件
                var clone = iptFileEle.cloneNode(true); // 上传文件空间副本

                iptFileEle.removeAttribute('id'); // 移除id属性
                iptFileEle.setAttribute('name', 'demoName'); // 添加name属性 ，post提交是用到

                iptFileEle.parentNode.insertBefore(clone, iptFileEle); // 在控件前插入副本控件
                iptFileEle.parentNode.removeChild(iptFileEle); // 移除控件

                form.appendChild(iptFileEle); // 把控件附加到表单上

                document.body.appendChild(div); // 最后附加div到html节点上
                document.getElementById('dwr-form').submit(); // 提交表单
            });
        });


        function loadingComplete() {
            $('#msg').html('完成');
        }
    </script>
</head>
<body>


    <input id="iptFile" type="file" />
    <button id="smt">上传</button>

    <div id="msg"></div>
</body>
</html>
