
/**
 * The NDWR object .
 */

if (typeof window['ndwr'] == 'undefined') {
    ndwr = {};
}
(function(){
    
    // 远程方法副本列表，保存本地提交的数据，当服务器返回数据时进行对比回调 RemoteInvoke的集合
    var batcheCaches = {}; // [{batchId : batch}]
    // 批量传输，保存需要传输的数据,以键值对存储
	var batchTransfers = {
		data : {}, // 传输的数据
		cache: {}, // 本次批量中的客户端副本 ，与上个区别是 当前存储的是一个批次，而上面存储所有批次
		cfg : { transferMode : 'xhr', timeout : 0 }, // 传输配置
        instance : null, // 传输实例 xhr => 异步实例 , iframe => iframe id
        timer : null // 定时器
	};
    // 批量提交标志
    var batchFlag = false;
    // 批次号,随提交次数递增
    var batchId = 0;
    // 单批次中每个方法的执行顺序索引,标识一次远程调用
    var methodIndex = 0;
    // 如果输入参数为节点
    var uploadCtrls = new Array();


    // 远程方法执行前事件
    ndwr.beforeTransferEvent = null;
    // 远程方法执行后事件
    ndwr.afterTransferEvent = null;
    // 全局服务器错误句柄
    ndwr.errorHandler = function (errors) { };
    // 数据过滤器 
    ndwr.dataFilter = function (data, textStatus) { return true; }
	

    /*
     * 初始化传输缓存内容
     */
    function initTransfer() {
        batchTransfers.data = {}; // 传输的数据
        batchTransfers.cache = {}; // 不要使用清空数组
		batchTransfers.cfg = {transferMode : ndwr.transport.mode, timeout : ndwr.transport.timeout }; // 初始化传输配置为全局配置
    }
    // 初始化批次
    function resetBatch() {
        initTransfer(); // 清空上一次传输记录数据
		uploadCtrls.length = 0; // 清空上传节点元素数组
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

        //uploadCtrls.length = 0; // 清空上传节点元素数组
        paramList = new Array(argLen + 1);// 新的参数集合
        for (var i = 0; i < argLen; i++) {
            // 取得参数，并验证
            var argItem = args[i]; 
            if(typeof(argItem) == 'function'){
                throw new Error('参数不能为函数类型');
            }
            // 文件上传传参
            //if (ndwr.util.getType(argItem) == 'htmlinputelement') { 
            if (ndwr.util.isInputFile(argItem)) {// 如果为上传控件节点元素isInputFile
				// 验证控件是否存在name属性
                if (!argItem.name /*== '' || argItem.name == undefined*/) {
                    throw new Error('请为上传控件设置name属性');
                }
                uploadCtrls.push(argItem); // 把控件添加到集合中
                argItem = argItem.name; // 如果为上传控件，则把控件name当作参数
                //transferMode = 'iframe'; // 设置传输方式
            }
            // 记录参数
            paramList[i] = ndwr.util.json.encode(argItem); //argItem;
        }
        // 回调方法检测设置
        paramList[argLen] = buildCallBackFunc(args[argLen]);

        return paramList;
    }

    /**
     * 整理用户设置的回调函数
     * callback : 回调参数类型
          1.{ callBack : function(data),errorHandler : function(errors) } 可以自定义错误回调函数
          2.function(data) 直接是回调函数，错误回调默认为全局ndwr.errorHandler
     */
    function buildCallBackFunc(callbackArg) {
        // 没有设置回调 给个默认
        if (!callbackArg /*== 'undefined' || callbackArg == null*/) {
            return { callBack: function (data) { }, errorHandler: ndwr.errorHandler };
        }
		
        // 只设置回调
        if (typeof (callbackArg) == 'function') { // 默认只有一个回调函数时默认为执行成功时回调
            return { callBack: callbackArg, errorHandler: ndwr.errorHandler };
        }
		
        // 以object类型设置回调
        var retCallBack = {};
        var callBackType = typeof (callbackArg.callBack); // 回调类型
        var errorHandlerType = typeof (callbackArg.errorHandler); // 错误处理句柄类型
		var transferModeType = typeof(callbackArg.transferModeType);
		var timeoutType = ndwr.util.getType(callbackArg.timeout);
        // 判断回调
        if (callBackType == 'function') {
            retCallBack.callBack = callbackArg.callBack;
        } else {
            throw new Error('回调函数callBack设置错误')
        }
        // 判断错误处理句柄
        if (errorHandlerType == undefined) { // 未定义则设为全局
            retCallBack.errorHandler = ndwr.errorHandler;
        } else if (errorHandlerType == 'function') { // 定义了
            retCallBack.errorHandler = callbackArg.errorHandler;
        } else { // 否则类型不对
            throw new Error('回调函数errorHandler设置错误')
        }
		// 批次配置
		setCurBatchCfg(callbackArg);
		
        return retCallBack;
    }

    /**
     * 设置批次提交配置
     * 如果在批次中多次设置，则会去最后一次
     * 检测的配置有 {transferMode : 'xhr', timeout : 0}
     */
	function setCurBatchCfg(callbackArg){
		var transferModeType = typeof(callbackArg.transferModeType);
		var timeoutType = ndwr.util.getType(callbackArg.timeout);
	
		// 传输模式
		if (callbackArg.transferMode == 'xhr' || callbackArg.transferMode == 'iframe' || callbackArg.transferMode == 'scriptTag') {
			batchTransfers.cfg.transferMode = callbackArg.transferModeType;
		} else if (callbackArg.transferMode != undefined) {
			throw new Error('传输模式不再可选对象xhr、iframe、scriptTag中');
		}
		// 超时时间
		if(ndwr.util.getType(callbackArg.timeout) == 'number'){
			if(callbackArg.timeout < 0){
				throw new Error('超时时间不能为负数');
			}
			batchTransfers.cfg.timeout = parseInt(callbackArg.timeout,10);
		}else if(callbackArg.timeout != undefined){
			throw new Error('超时时间应设置为整数');
		}
	}
	
    /** 
     * 生成传输的参数
    */
    function buildTransferParams(serviceName, methodName, argList) {
        batchTransfers.data['BatchID'] = batchId; // 批次号
        batchTransfers.data['Method|' + methodIndex] = serviceName + '.' + methodName; //Method|[methodIndex] = [ServiceName].[MethodName]

        for (var i = 0; i < argList.length - 1; i++) { // 顺序添加参数
            batchTransfers.data['Param|' + i + '|' + methodIndex] = argList[i].toString(); // Param|[paramIndex]|[methodIndex]  = [value]
        }
        // 记录调用信息
        var batch = {};
        //batch.BatchId = batchId; // 批次号
        //batch.MethodIndex = methodIndex; // 方法索引
        //batch.Service = serviceName; // 服务名
        //batch.Method = methodName; // 方法名
        batch.CallBackFunc = argList[argList.length - 1]; // 回调参数信息
        // 加入本批次缓存中
        batchTransfers.cache[methodIndex] = batch;
    }
	
    // 开启批量提交
    ndwr.BeginBatch = function () {
        batchFlag = true; // 批量标记
    }

    // 远程提交
    ndwr.RemoteMethod = function (serviceName, methodName, args, argLen) {

        var argList = bulidParams(args,argLen); // 规划参数
        buildTransferParams(serviceName, methodName, argList); // 生成传输的参数

        // 方法索引递增
        methodIndex++;
        // 如果没有开启批量提交且为默认传输模式，则直接提交
        if (batchFlag != true) { 
            RemoteSubmit();
        }
    }

    // 结束批量标志，刷新传输数据
    ndwr.EndBatch = function (cfg) {
        if(cfg){
            setCurBatchCfg(cfg);
        }
        RemoteSubmit();
    }

    
    // 执行远程调用
    function RemoteSubmit() {
        // 提交之前把本次批次添加到全局副本中
        batchTransfers.cache['batchId'] = batchId;
		if(uploadCtrls.length > 0){ // 如果存在上传 强制转换为iframe模式
			batchTransfers.cfg.transferMode = 'iframe';
		}
		batcheCaches[batchId] = batchTransfers.cache; //[{batchId : batch}]
        // 提交请求
        ndwr.transport.send(batchTransfers);
        // 初始化批次，以准备下一次调用
        resetBatch();
    }

    // 执行回调
    ndwr.handleCallback = function (cb_batchId, cb_methodIndex, data, errors) {
        // 获取该次返回
        //var batch = batches[cb_batchId][cb_methodIndex];
        var batch;
        if(!(batch = batcheCaches[cb_batchId]) || !(batch = batch[cb_methodIndex]) ){
        //if (batch == null) { // 没找到在客户端的标记
            if(!errors){ errors = []; }
            errors.push({ "Name": "ClientError", Message: "请求未匹配到客户端回调" });
            return;
        }
        var retCallBack = batch.CallBackFunc;
        // 如果服务端返回结果中带有错误信息，则不会激发回调函数
        if (errors && errors.length > 0) {
            // 指定了错误处理方法
            retCallBack.errorHandler(errors);
        }else {
            // 执行回调函数
            if (!ndwr.dataFilter || ndwr.dataFilter(data) != false) { // 如有数据过滤函数为空  或 数据过滤返回true
                retCallBack.callBack(data);// 执行回调函数
            }
        }
        delete batch; // 移除
    }
    

    ndwr.transport = {
        url : '',
        timeout: 0,
        mode : 'xhr', // xhr,iframe,scriptTag三种
        sendingHandler : function(){ return true;},
        completedHandler : function(){},
        errorHandler : function(req) {},
        send : function(batchInfo){
            if(this.sendingHandler() == false){
                return;
            }
            switch(batchInfo.cfg.transferMode){
                case 'xhr': 
                    this.xhr.send(batchInfo.batchs, this.url, batchInfo.data); // 提交请求
                    break;
                case 'iframe':
                    this.iframe.send(batchInfo.batchs, this.url,batchInfo.data);
                    break;
                case 'scriptTag':
                    break;
                default :
                    break;
            }
        },
        setTimer : function(ins){
//            var timer = setTimeout(function(){
//                c.onOut(c);
//	        }, c.timeout);
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
                // 初始化参数
                var p = this.constructRequest(parameters);
			    // 获取异步请求实例
                var req = ndwr.transport.xhr.reqFunc();
                // 当前批次信息
			    var xbatchs = batchs;
			    // 状态变化事件
			    req.onreadystatechange = function (ev) {
			        ndwr.transport.xhr.stateChange(req, xbatchs);
			    };
			    // 提交
			    req.open(ndwr.transport.xhr.method, url, true);
			    //req.setRequestHeader("Content-type", ndwr.transport.xhr.contentType);
			    req.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

			    req.send(p);
			},
            // 状态改变事件
			stateChange: function (req, batchs) {
			    if (req.readyState == 4) { // 完成
                    ndwr.transport.completedHandler();
                    if(req.status == 200){
			            eval(req.responseText);
                    }else {
                        ndwr.transport.errorHandler(req);
                    }
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
				        s[s.length] = encodeURIComponent( key ) + "=" + encodeURIComponent( value );
			        };
                ndwr.util.each(p,function(i){
                    add(i,this[i]);
                });
			    return s.join('&').replace( r20, "+" ); // 来自于jquery方式处理
			}
		},
        // form提交到iframe方式
		iframe: {
            ifmTag  : function(batchId) { return 'ndwr_ifm_' + batchId;},
            formTag : function(batchId) { return 'ndwr_form_' + batchId; },
            ifmFunc : function(batchId){
                var ifmtag = this.ifmTag(batchId);
                var div = document.createElement("div");
                // Add the div to the document first, otherwise IE 6 will ignore onload handler.
                document.body.appendChild(div);
                div.innerHTML = '<iframe src="javascript:void(0)" frameborder="0" style="width: 0px;height: 0px; border: 0;" id="' + ifmtag + '" name="' + ifmtag + '" onload="ndwr.transport.iframe.callBack(\'' + batchId + '\');"></iframe>';
            },
            send : function(batchs, url, parameters){
                this.ifmFunc(batchs.batchId); // 构建iframe
                var p = this.constructRequest(parameters);
                // form 表单 innerHtml
                var formHtml = "<form id='" + this.formTag(batchs.batchId) + "' action='" + url + "' target='" + this.ifmTag(batchs.batchId) + "' style='display:none;' method='post'";
                if (uploadCtrls.length > 0) { // 如果有上传
                    formHtml += " enctype='multipart/form-data'";
                }
                formHtml += "></form>";
                var div = document.createElement("div");
                div.innerHTML = formHtml;
                var form = div.firstChild;

                if(uploadCtrls.length > 0){ // 如果有上传，则移动控件到表单
                    this.moveUploadCtrls(form);
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
                p['TransportMode'] = 'iframe';
                return p;
            },
            callBack : function(batchId){
                var ifmtag = this.ifmTag(batchId);
                if(window.frames[ifmtag].document.location.href.indexOf('about:blank') >=0){
                    return;
                }
                //this.remove(batchId);
                ndwr.transport.completedHandler();
                
            },
            remove : function(batchId){
                var ifmTag = document.getElementById(this.ifmTag(batchId)).parentNode;
                var formTag = document.getElementById(this.formTag(batchId));
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

    ndwr.util = {
        // 获取类型
        getType : function(obj){
            var _t;
            var objType = ((_t = typeof (obj)) == "object" ? Object.prototype.toString.call(obj).slice(8, -1) : _t).toLowerCase();
            return objType;
        },
        isInputFile : function(ele){
            if (ele && ele.tagName && ele.tagName.toLowerCase() == "input" && ele.type && ele.type.toLowerCase() == "file"){
                return true;
            }
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
