/**
* JSON 解析类
*/

var JSONUtil;
if (!JSONUtil) {
    JSONUtil = {};
}
JSONUtil.decode = function (json) {
    try {
        return eval("\u0028" + json + '\u0029'); // '(' + json + ')'
    } catch (exception) {
        return eval("\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064"); //'undefine'
    }
};
JSONUtil.encode = (function () {
    var $ = !!{}.hasOwnProperty, _ = function ($) {
        return $ < 10 ? "0" + $ : $
    }, A = {
        "\b": "\\b",
        "\t": "\\t",
        "\n": "\\n",
        "\f": "\\f",
        "\r": "\\r",
        "\"": "\\\"",
        "\\": "\\\\"
    };
    return (function (C) {
        if (typeof C == "\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064" || C === null)
            return "null";
        else if (Object.prototype.toString.call(C) === "\u005b\u006f\u0062\u006a\u0065\u0063\u0074\u0020\u0041\u0072\u0072\u0061\u0079\u005d") {
            var B = ["\u005b"], G, E, D = C.length, F;
            for (E = 0; E < D; E += 1) {
                F = C[E];
                switch (typeof F) {
                    case "\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064":
                    case "\u0066\u0075\u006e\u0063\u0074\u0069\u006f\u006e":
                    case "\u0075\u006e\u006b\u006e\u006f\u0077\u006e":
                        break;
                    default:
                        if (G)
                            B.push("\u002c");
                        B.push(F === null ? "null" : this.encode(F));
                        G = true
                }
            }
            B.push("\u005d");
            return B.join("")
        } else if ((Object.prototype.toString.call(C) === "\u005b\u006f\u0062\u006a\u0065\u0063\u0074\u0020\u0044\u0061\u0074\u0065\u005d"))
            return "\"" + C.getFullYear() + "-" + _(C.getMonth() + 1) + "-" + _(C.getDate()) + "T" + _(C.getHours()) + ":" + _(C.getMinutes()) + ":" + _(C.getSeconds()) + "\"";
        else if (typeof C == "\u0073\u0074\u0072\u0069\u006e\u0067") {
            if (/["\\\x00-\x1f]/.test(C))
                return "\"" + C.replace(/([\x00-\x1f\\"])/g, function (B, _) {
                    var $ = A[_];
                    if ($)
                        return $;
                    $ = _.charCodeAt();
                    return "\\u00" + Math.floor($ / 16).toString(16) + ($ % 16).toString(16)
                }) + "\"";
            return "\"" + C + "\""
        } else if (typeof C == "\u006e\u0075\u006d\u0062\u0065\u0072")
            return isFinite(C) ? String(C) : "null";
        else if (typeof C == "\u0062\u006f\u006f\u006c\u0065\u0061\u006e")
            return String(C);
        else {
            B = ["\u007b"], G, E, F;
            for (E in C)
                if (!$ || C.hasOwnProperty(E)) {
                    F = C[E];
                    if (F === null)
                        continue;
                    switch (typeof F) {
                        case "\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064":
                        case "\u0066\u0075\u006e\u0063\u0074\u0069\u006f\u006e":
                        case "\u0075\u006e\u006b\u006e\u006f\u0077\u006e":
                            break;
                        default:
                            if (G)
                                B.push("\u002c");
                            B.push(this.encode(E), "\u003a", this.encode(F));
                            G = true
                    }
                }
            B.push("\u007d");
            return B.join("")
        }
    })
})();
window.JSONUtil = JSONUtil;




/**
 * The NDWR object .
 */

if (typeof window['ndwr'] == 'undefined') {
    ndwr = {};
}
(function(){

    // 远程方法列表，保存本地提交的数据，当服务器返回数据时进行对比回调 RemoteInvoke的集合
    var batchMethod = new Array();
    // 批量传输，保存需要传输的数据,以键值对存储
    var batchTransfer = {};
    // 批量提交标志
    var batchFlag = false;
    // 批次号,随提交次数递增
    var batchId = 0;
    // 单批次中每个方法的执行顺序索引
    var methodIndex = 0;
    // 下载标识
    var downloadFlag = false;
    // 下载iframe名称
    var dl_ifm_name = 'ndwr_download_ifm';
    // 上传iframe名称
    var ul_ifm_name = 'ndwr_upload_ifm';
    //

    // 远程方法执行前事件
    ndwr.beforeInvocationEvent = null;
    // 远程方法执行后事件
    ndwr.afterInvocationEvent = null;
    // 全局错误句柄
    ndwr.errorHandle = function (SystemErrors) { };
    // 数据过滤器 
    ndwr.dataFilter = function (data, textStatus) { return true; }
    // timeout
    ndwr.timeout = 0;

    // 远程执行实体
    function RemoteInvoke() {
        this.BatchId = ''; // 批次号
        this.MethodIndex = 0; // 方法索引
        this.Service = '';
        this.Method = '';
        this.CallBackFunc = null;
    }

    /*
     * 验证
     * args 用户调用参数集
     * remoteParamCount 服务器端方法参数个数
     */
    ndwr.Validate = function(args,remoteParamCount){
	    if(args.length < remoteParamCount ) { 
            throw new Error('参数不匹配')
        }
	    var retCallBack = {};
	    var callbackParam = args[remoteParamCount];
	    if(callbackParam != null && callbackParam != 'undefined' ){
		    if(typeof(callbackParam) == 'function'){ // 默认只有一个回调函数时默认为执行成功时回调
			    retCallBack.callBack = callbackParam;
			    retCallBack.errorHandler = null;
			    args[remoteParamCount] = retCallBack;
		    }else { // 如果最后一个回调没有指定的方法
			    if(callbackParam.callBack == null){
				    throw new Error('回调函数设置错误')
			    }
		    }
	    }else {
            //var newArgs = new Array();
            //newArgs.concat(args.slice(0,remoteParamCount));
            //newargs[remoteParamCount] = {callBack:function(data){},errorHandler : null};
//            for(var i =0; i< args.length; i++){
//                newArgs.
//            }
            //args[remoteParamCount] = {callBack:function(data){},errorHandler : null};
        }
    }

    // 开启批量提交
    ndwr.BeginBatch = function () {
        batchFlag = true;
        methodIndex = 0;
        batchTransfer = {}; // 初始化传输的数据
        batchId++;
    }

    ndwr.RemoteMethod = function(serviceName, methodName, args,isDownLoad) {
        if(batchFlag && isDownLoad){
            throw new Error('暂不支持在批量中进行文件下载');
        }
        downloadFlag = isDownLoad ? true : false;

        if (!batchFlag) { // 如果没有开启批量提交，则自动递增批次号
            batchId++;
            methodIndex = 0;
        } else { // 如果开启了批量提交，则把方法索引递增
            methodIndex++;
        }

        batchTransfer['SABatchID'] = batchId;
        batchTransfer['Method|' + methodIndex] = serviceName + '.' + methodName; //Method|[methodIndex] = [ServiceName].[MethodName]

        if (args != null) {
            for (var i = 0; i < args.length - 1; i++) {
                batchTransfer['Param|' + i + '|' + methodIndex] = args[i].toString(); // Param|[paramIndex]|[methodIndex]  = [value]
            }
            // 记录调用信息
            var invoke = new RemoteInvoke();
            invoke.BatchId = batchId;
            invoke.MethodIndex = methodIndex;
            invoke.Service = serviceName;
            invoke.Method = methodName;
            invoke.CallBackFunc = args[args.length - 1];

            batchMethod.push(invoke);
        }

        if (batchFlag != true) {
            RemoteSubmit();
        }
    }

    // 结束批量标志，刷新传输数据
    ndwr.EndBatch = function () {
        batchFlag = false;

        if (ndwr.beforeInvocationEvent != null) { // before invocation event.
            ndwr.beforeInvocationEvent();
        }

        RemoteSubmit()
    }


    // 执行远程调用
    function RemoteSubmit() {
        if(downloadFlag){
            download("RemoteServiceHandler.rs", batchTransfer);
            downloadFlag = false;
        }else{
            ajaxPostJson("RemoteServiceHandler.rs", batchTransfer);
        }
        batchTransfer = {};
    }

    ndwr.RemoteCallback = function(data, textStatus) {
        if (data == null || data.DataList == 0 || data.DataList.length == 0) {
            return;
        }
        var dataList = data.DataList; // 批次数据列表
        for (var i = 0; i < batchMethod.length; i++) {
            var clientMethod = batchMethod[i]; // 客户端缓存中的记录的远程方法

            for (var j = 0; j < dataList.length; j++) {
                var remoteMethod = dataList[j]; // 远程返回的服务信息
                // 服务，方法，批次 验证
                if (clientMethod.BatchId.toString() == data.BatchId &&
                    clientMethod.MethodIndex.toString() == remoteMethod.MethodIndex &&
                    clientMethod.Method == remoteMethod.Method &&
                    clientMethod.Service == remoteMethod.Service
                    ) {
                    var retCallBack = clientMethod.CallBackFunc;
                    // 如果服务端返回结果中带有错误信息，则不会激发回调函数
                    if(remoteMethod.SystemErrors != null && remoteMethod.SystemErrors.length > 0){
                        if(retCallBack == null || retCallBack.errorHandler == null){ // 如果没有指定错误处理，使用默认全局错误处理
                            ServiceError(remoteMethod.SystemErrors);
                        }else { // 指定了错误处理方法
                            retCallBack.errorHandler(remoteMethod.SystemErrors);
                        }
                        break;
                    }

                    if(retCallBack != null){ // 如果有回调函数
                        var data = JSONUtil.decode(remoteMethod.JsonData);
                        if (ndwr.dataFilter == null || ndwr.dataFilter(data) != false) { // 如有数据过滤函数为空  或 数据过滤返回true
                            retCallBack.callBack(data);// 执行回调函数
                        }
                    }
                    
                    batchMethod.splice(i--, 1); // 如果该项已经处理，则移除
                    break;
                }
            }
        }
    }

    function ServiceError(SystemErrors){
        if(ndwr.errorHandle != null){
            ndwr.errorHandle(SystemErrors);
        }
    }

    /**
     *  异步Post方式提交
     *  @method  ajaxPostJson
     *  @param   url 远程url
     *  @param   param post参数
     *  @returns 异步提交句柄
     */
    function ajaxPostJson(url, param) {

        var ajaxHander = $.ajax({
            url: url + "?dt=" + new Date().getTime(),
            type: "POST",
            data: param,
            dataType: 'json',
            timeout: ndwr.timeout,
            error: function (XMLHttpRequest, textStatus, errorThrown) { 
                ndwr.errorHandle({ Name:'ResultException',Message:'请求失败'});
            },
            success: function (data, textStatus) {
                ndwr.RemoteCallback(data, textStatus);
            }
        });

        return ajaxHander;
    }


    /*========================文件下载=======================*/

    function download(url, param) {
        if (window.frames[_ifm_name] == null) {
            var div = document.createElement("div");
            // Add the div to the document first, otherwise IE 6 will ignore onload handler.
            document.body.appendChild(div);
            div.innerHTML = "<iframe src='javascript:void(0)' frameborder='0' style='width: 0px;height: 0px; border: 0;' id='" + dl_ifm_name + "' name='" + dl_ifm_name + "' onload='ndwr.RemoteCallback();'></iframe>";
        }

        post(url,param);
    }

    function post(url, param) {
        var temp = document.createElement("form");
        temp.action = url;
        temp.method = "post";
        temp.target = _ifm_name;
        temp.style.display = "none";

        for (var x in param) {
            var opt = document.createElement("textarea");
            opt.name = x;
            opt.value = param[x];
            temp.appendChild(opt);
        }
        document.body.appendChild(temp);
        temp.submit();
        return temp;
    }


    
    /*========================文件上传=======================*/
     

    //    function isEntity(o) {
    //        var _t;
    //        var objType = ((_t = typeof (o)) == "object" ? Object.prototype.toString.call(o).slice(8, -1) : _t).toLowerCase()
    //        
    //    }

})();
