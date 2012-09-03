
/**
 * The NDWR object .
 */

if (typeof window['ndwr'] == 'undefined') {
    ndwr = {};
}
(function(){
    
    // 远程方法副本列表，保存本地提交的数据，当服务器返回数据时进行对比回调 RemoteInvoke的集合
    var batches = {}; // [{batchId : batch}]
    // 批量传输，保存需要传输的数据,以键值对存储
	var batchTransfers = {
		transferData : {}, // 传输的数据
		batchs: {} // 本次批量中的客户端副本 ，与上个区别是 当前存储的是一个批次，而上面存储所有批次
	};
    // 批量提交标志
    var batchFlag = false;
    // 批次号,随提交次数递增
    var batchId = 0;
    // 单批次中每个方法的执行顺序索引,标识一次远程调用
    var methodIndex = 0;
    // 传输模式
    var transferMode = 'xhr'; // xhr,iframe,scriptTag三种
    // 如果输入参数为节点
    var uploadCtrls = new Array();



    ndwr.remoteURL = '';
    // 远程方法执行前事件
    ndwr.beforeTransferEvent = null;
    // 远程方法执行后事件
    ndwr.afterTransferEvent = null;
    // 全局错误句柄
    ndwr.errorHandle = function (SystemErrors) { };
    // 数据过滤器 
    ndwr.dataFilter = function (data, textStatus) { return true; }
    // timeout
    ndwr.timeout = 0;

    // 远程执行实体结构
    //function Batch() {
    //    this.BatchId = ''; // 批次号
    //    this.methodIndex = 0; // 方法索引
    //    this.Service = ''; // 服务
    //    this.Method = ''; // 方法
    //    this.CallBackFunc = null; // 回调
    //}

    function clearTransfer() {
        batchTransfers.transferData = {}; // 传输的数据
        batchTransfers.batchs = {}; // 不要使用清空数组
    }
    // 初始化批次
    function resetBatch() {
        clearTransfer(); // 清空上一次传输记录数据
        batchId++; // 批次号递增
        batchFlag = false; // 关闭批量
    }
    /*
     * 生成用户调用的参数，对参数进行统一格式
     * args 用户调用参数集
     * argLen 服务器端方法参数个数
     * 方法:function(arg1,arg2,...,argx,callback)
     * callback : 回调参数类型
        1.{ callBack : function(data),errorHandler : function(errors) } 可以自定义错误回调函数
        2.function(data) 直接是回调函数
     */
    function bulidParams(args,argLen){

	    if(args.length < argLen ) { 
            throw new Error('参数不匹配')
        }
        if(batchFlag && transferMode == 'download'){
            throw new Error('暂不支持在批量中进行文件下载');
        }

        uploadCtrls.length = 0; // 清空上传节点元素数组
        paramList = new Array(argLen + 1);// 新的参数集合
        for (var i = 0; i < argLen; i++) {
            // 取得参数，并验证
            var argItem = args[i]; 
            if(typeof(argItem) == 'function'){
                throw new Error('参数不能为函数类型');
            }
            // 文件上传传参
            if (ndwr.util.getType(argItem) == 'htmlinputelement') { // 如果为上传控件节点元素
				// 验证控件是否存在name属性
                if (argItem.name == '' || argItem.name == 'undefined') {
                    throw new Error('请为上传控件设置name属性');
                }
                uploadCtrls.push(argItem); // 把控件添加到集合中
                argItem = argItem.name; // 如果为上传控件，则把控件name当作参数
                transferMode = 'iframe'; // 设置传输方式
            }
            // 记录参数
            paramList[i] = ndwr.util.json.encode(argItem); //argItem;
        }

	    var retCallBack = {};
	    var callbackArg = args[argLen]; // 回调函数
	    if(callbackArg != null && callbackArg != 'undefined' ){
		    if(typeof(callbackArg) == 'function'){ // 默认只有一个回调函数时默认为执行成功时回调
			    retCallBack.callBack = callbackArg;
			    retCallBack.errorHandler = null;
		    }else { // 如果最后一个回调没有指定的方法
		        if (typeof(callbackArg.callBack) != 'function') {
				    throw new Error('回调函数设置错误')
		        }
		        retCallBack = callbackArg;
		    }
	    }else {
            retCallBack = {callBack:null,errorHandler : null};
        }
        
		paramList[argLen] = retCallBack;
        return paramList;
    }


    /* 
     * 生成传输的参数
    */
    function buildTransferParams(serviceName, methodName, argList) {
        batchTransfers.transferData['BatchID'] = batchId; // 批次号
        batchTransfers.transferData['Method|' + methodIndex] = serviceName + '.' + methodName; //Method|[methodIndex] = [ServiceName].[MethodName]

        for (var i = 0; i < argList.length - 1; i++) { // 顺序添加参数
            batchTransfers.transferData['Param|' + i + '|' + methodIndex] = argList[i].toString(); // Param|[paramIndex]|[methodIndex]  = [value]
        }
        // 记录调用信息
        var batch = {};
        batch.BatchId = batchId; // 批次号
        batch.MethodIndex = methodIndex; // 方法索引
        batch.Service = serviceName; // 服务名
        batch.Method = methodName; // 方法名
        batch.CallBackFunc = argList[argList.length - 1]; // 回调参数信息

        batchTransfers.batchs[methodIndex] = batch;
    }
	
    // 开启批量提交
    ndwr.BeginBatch = function () {
        batchFlag = true; // 批量标记
    }

    // 远程提交
    ndwr.RemoteMethod = function (serviceName, methodName, args, argLen) {

        var argList = bulidParams(args,argLen); // 规划参数
        var batch = buildTransferParams(serviceName, methodName, argList); // 生成传输的参数

        // 方法索引递增
        methodIndex++;
        // 如果没有开启批量提交且为默认传输模式，则直接提交
        if (batchFlag != true) { 
            RemoteSubmit();
        }
    }

    // 结束批量标志，刷新传输数据
    ndwr.EndBatch = function () {
        RemoteSubmit();
    }

    
    // 执行远程调用
    function RemoteSubmit() {
        // 提交之前把本次批次添加到全局副本中
        batchTransfers.batchs['batchId'] = batchId;
		batches[batchId] = batchTransfers.batchs; //[{batchId : batch}]
        // 提交请求
        ndwr.transport.send(batchTransfers, transferMode);
        // 初始化批次，以准备下一次调用
        resetBatch();
    }

    // 执行回调
    ndwr.handleCallback = function (cb_batchId, cb_methodIndex, data, errors) {
        // 获取该次返回
        var batch = batches[cb_batchId][cb_methodIndex];
        if (batch == null) { // 没找到在客户端的标记
            ServiceError({ "Name": "ClientError", Message: "请求为匹配到回调" });
            return;
        }
        var retCallBack = batch.CallBackFunc;
        // 如果服务端返回结果中带有错误信息，则不会激发回调函数
        if (/*errors != null && errors != 'undefined'*/ errors && errors.length > 0) {
            if (retCallBack == null || retCallBack.errorHandler == null) { // 如果没有指定错误处理，使用默认全局错误处理
                ServiceError(errors);
            } else { // 指定了错误处理方法
                retCallBack.errorHandler(errors);
            }
            return;
        }

        if (retCallBack != null) { // 如果有回调函数
            if (ndwr.dataFilter == null || ndwr.dataFilter(data) != false) { // 如有数据过滤函数为空  或 数据过滤返回true
                retCallBack.callBack(data);// 执行回调函数
            }
        }
        //delete batch; // 移除
    }
    

    function ServiceError(SystemErrors){
        if(ndwr.errorHandle != null){
            ndwr.errorHandle(SystemErrors);
        }
    }

    /************************************ oracle 官网rest使用的 ajax 结构 /************************************/
    ndwr.transport = {
        url : '',
        send : function(batchInfo,mode){
            if (mode == 'xhr') {
                var p = ndwr.transport.xhr.constructRequest(batchInfo.transferData); // 构造参数
                ndwr.transport.xhr.send(batchInfo.batchs, ndwr.transport.url, p); // 提交请求
            }else if (mode == 'iframe'){
                ndwr.transport.iframe.send(batchInfo.batchs, ndwr.transport.url,batchInfo.transferData);
            } else if (mode == 'scriptTag') {

            }
        },
        // 异步请求
		xhr : {
			method : 'POST',
			contentType : 'text/plain',
			reqFunc: (function () {// 按照不同浏览器尝试用不同的异步构造方式
					if (window.XMLHttpRequest) { // Mozilla, Safari,...
						return function () {
							var xhr = new XMLHttpRequest();
							if (xhr.overrideMimeType) {
								// Set type accordingly to anticipated content type.
								xhr.overrideMimeType(ndwr.transport.content_type);
							}
							return xhr;
						};
					} else if (window.ActiveXObject) { // IE
						return function () {
							try {
								return new ActiveXObject("Msxml2.XMLHTTP");
							} catch (e) {
								try {
									return new ActiveXObject("Microsoft.XMLHTTP");
								} catch (e) {
									throw new Error('Cannot create XMLHttpRequest object');
								}
							}
						}
					} else {
						throw new Error('Cannot create XMLHttpRequest object');
					}
			})(),
            // 提交请求
			send: function (batchs, url, parameters) {
			    var req = ndwr.transport.xhr.reqFunc();
			    // 状态变化事件
			    var xbatchs = batchs;
			    req.onreadystatechange = function (ev) {
			        ndwr.transport.xhr.stateChange(req, xbatchs);
			    };
			    // 提交
			    req.open(ndwr.transport.xhr.method, url, true);
			    //req.setRequestHeader("Content-type", ndwr.transport.xhr.contentType);
			    req.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

			    req.send(parameters);
			},
            // 状态改变事件
			stateChange: function (req, batchs) {
			    if (req.readyState == 4 && req.status == 200) {
			        eval(req.responseText);
				}
			},
            // 构造请求
			constructRequest : function (p) {
                // 如果没有参数 返回null
                if (p == null || p.length == 0) {
			        return null;
			    }
                var s = [],
                    r20 = /%20/g, // 匹配参数中空格
			        add = function( key, value ) {
				        s[ s.length ] = encodeURIComponent( key ) + "=" + encodeURIComponent( value );
			        };
                ndwr.util.each(p,function(i){
                    add(i,this[i]);
                });
			    return s.join('&').replace( r20, "+" ); // 来自于jquery方式处理
			}
		},
        // form提交到iframe方式
		iframe: {
            ifmTag  : function(id) { return 'ndwr_ifm_' + id;},
            formTag : function(id) { return 'ndwr_form_' + id; },
            ifmFunc : function(id){
                var ifmtag = ndwr.transport.iframe.ifmTag(id);
                var div = document.createElement("div");
                // Add the div to the document first, otherwise IE 6 will ignore onload handler.
                document.body.appendChild(div);
                div.innerHTML = '<iframe src="javascript:void(0)" frameborder="0" style="width: 0px;height: 0px; border: 0;" id="' + ifmtag + '" name="' + ifmtag + '" onload="ndwr.transport.iframe.callBack(\'' + ifmtag + '\');"></iframe>';
            },
            send : function(batchs, url, parameters){
                ndwr.transport.iframe.ifmFunc(batchs.batchId); // 构建iframe
                var p = ndwr.transport.iframe.constructRequest(parameters);
                // 构建form
                var form = document.createElement("form");
                form.id = ndwr.transport.iframe.formTag(batchs.batchId);
                form.action = url;
                form.method = "post";
                form.target = ndwr.transport.iframe.ifmTag(batchs.batchId);
                form.style.display = "none";
                if (uploadCtrls.length > 0) { // 存在上传
                    form.enctype = "multipart/form-data";
                    ndwr.transport.iframe.moveUploadCtrls(form);
                }
                // 添加参数
                for (var i in p) {
                    var formInput = document.createElement("input");
                    formInput.setAttribute("type", "hidden");
                    formInput.setAttribute("name", i);
                    formInput.setAttribute("value", p[i]);
                    form.appendChild(formInput);
                }

                document.body.appendChild(form);
                form.submit();
            },
            moveUploadCtrls : function(form){
                for (var i = 0; i < uploadCtrls.length; i++) {
                    var ele = uploadCtrls[i];
                    var clone = ele.cloneNode(true); // 上传文件空间副本

                    ele.removeAttribute('id'); // 移除id属性
                    ele.setAttribute('name', 'ndwr_file_' + ele.name); // 添加name属性 ，post提交是用到

                    ele.parentNode.insertBefore(clone, ele); // 在控件前插入副本控件
                    ele.parentNode.removeChild(ele); // 移除控件

                    form.appendChild(ele); // 把控件附加到表单上
                }
            },
            constructRequest : function(p){
                p['transferMode'] = 'iframe';
                return p
            },
            callBack : function(ifm){
                var data = window[ifm].document.body.innerHTML;
            }
		},
        // 加载脚本文件方式，参数只能用get方式
		scriptTag: {

		},
        download : function(key){
            var div = document.createElement("div");
            // Add the div to the document first, otherwise IE 6 will ignore onload handler.
            document.body.appendChild(div);
            div.innerHTML = '<iframe src="ndwr/ndwrdownload.ashx?id=' + key + '" frameborder="0" style="width: 0px;height: 0px; border: 0;" ></iframe>';
        }
	}
 /************************************ oracle 官网rest使用的 ajax 结构 /************************************/

    ndwr.util = {
        // 获取类型
        getType : function(obj){
            var _t;
            var objType = ((_t = typeof (obj)) == "object" ? Object.prototype.toString.call(obj).slice(8, -1) : _t).toLowerCase();
            return objType;
        },
        parseDate : function(date){
            function f(n){ return n < 10 ? '0' + n : n;}
            return  f(date.getFullYear()) + '-' +
                    f(date.getMonth() + 1) + '-' +
                    f(date.getDate()) + ' ' +
                    f(date.getHours()) + ':' +
                    f(date.getMinutes()) + ':' +
                    f(date.getSeconds());
        },
        // 遍历
        each : function( obj, fn, args ) {
	        if (args) { // 含参遍历
		        if (obj.length == undefined){
			        for ( var i in obj ){
				        fn.apply( obj, args );
                    }
		        }else{
			        for ( var i = 0, ol = obj.length; i < ol; i++ ) {
				        if ( fn.apply( obj, args ) === false ){
					        break;
                        }
			        }
		        }
	        } else {
		        if ( obj.length == undefined ) { // 实体属性反射遍历
			        for ( var i in obj ){
				        fn.call( obj, i, obj );
                    }
		        }else{ // 数组遍历
			        for ( var i = 0, ol = obj.length, val = obj[0]; i < ol && fn.call(val,i,val) !== false; val = obj[++i] ){}
		        }
	        }
	        return obj;
        },
        json : {
            decode : function(jsonstr){
                try {
                    return eval("\u0028" + jsonstr + '\u0029'); // '(' + json + ')'
                } catch (exception) {
                    return jsonstr;//eval("\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064"); //'undefine'
                }
            },
            encode : (function () {
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
                    if (typeof C == "\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064" || C === null) // undefined
                        return "null";
                    else if (Object.prototype.toString.call(C) === "\u005b\u006f\u0062\u006a\u0065\u0063\u0074\u0020\u0041\u0072\u0072\u0061\u0079\u005d") { // [object Array]
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
                    } else if ((Object.prototype.toString.call(C) === "\u005b\u006f\u0062\u006a\u0065\u0063\u0074\u0020\u0044\u0061\u0074\u0065\u005d")) // [object Date]
                        return "\"" + C.getFullYear() + "-" + _(C.getMonth() + 1) + "-" + _(C.getDate()) + "T" + _(C.getHours()) + ":" + _(C.getMinutes()) + ":" + _(C.getSeconds()) + "\"";
                    else if (typeof C == "\u0073\u0074\u0072\u0069\u006e\u0067") { // string
                        if (/["\\\x00-\x1f]/.test(C))
                            return "\"" + C.replace(/([\x00-\x1f\\"])/g, function (B, _) {
                                var $ = A[_];
                                if ($)
                                    return $;
                                $ = _.charCodeAt();
                                return "\\u00" + Math.floor($ / 16).toString(16) + ($ % 16).toString(16)
                            }) + "\"";
                        return "\"" + C + "\""
                    } else if (typeof C == "\u006e\u0075\u006d\u0062\u0065\u0072") // number
                        return isFinite(C) ? String(C) : "null";
                    else if (typeof C == "\u0062\u006f\u006f\u006c\u0065\u0061\u006e") // boolean
                        return String(C);
                    else {
                        B = ["\u007b"], G, E, F;
                        for (E in C)
                            if (!$ || C.hasOwnProperty(E)) {
                                F = C[E];
                                if (F === null)
                                    continue;
                                switch (typeof F) {
                                    case "\u0075\u006e\u0064\u0065\u0066\u0069\u006e\u0065\u0064": // undefined
                                    case "\u0066\u0075\u006e\u0063\u0074\u0069\u006f\u006e": // function
                                    case "\u0075\u006e\u006b\u006e\u006f\u0077\u006e": // unknown
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
            })()
        }
    };

})();
