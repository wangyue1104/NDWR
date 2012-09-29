一、简介
=================

DWR 包含2 个主要部分： 
-------------------------------------------------

*1.一个运行在服务器端的HandlerFactory，它处理请求并且向浏览器发回响应。 
*2.运行在浏览器端的JavaScript，它发送请求而且还能动态更新网页。 

二、构建一个NDWR程序
=================

*1.程序集：NDWR.dll ，Newtonsoft.Json.dll(json序列化依赖库)
	
*2.web.config配置文件：
		<system.web>
			<httpHandlers>
				<add verb="*" path="ndwr/*.ashx" type="NDWR.Web.Handler.HandlerFactory,NDWR" />
			</httpHandlers>
		</system.web>
*3.Global.asax -> Application_Start 事件
		void Application_Start(object sender, EventArgs e) 
		{
			//在应用程序启动时运行的代码
			NDWR.Config.GlobalConfig.Instance.DefaultConfig("RemoteService");
			// 如果你有自己的拦截器，可以照此方式添加
			//NDWR.Config.GlobalConfig.Instance.Interceptors.Add(new RemoteService.NHVEntityValidateInterceptor());
		}
	
*4.编写service
	[RemoteService]
	public class RemoteDemoNew {

		[RemoteMethod]
		public string HW() {
			return "Hello World";
		}
	}
	
*5.前台脚本
		<script src="ndwr/ndwrcore.ashx" type="text/javascript"></script><!-- ndwr核心库 -->
		<script src="ndwr/RemoteDemoNew.ashx" type="text/javascript"></script><!-- 开放服务 -->
		<script type="text/javascript">
            function HW() {
                RemoteDemoNew.HW(function(data){
					alert(data);
				});
            }
        </script>
		<button onclick="HW()"></button>
		
三、NDWR公开服务
=================

	1.在需要公开服务类上添加[RemoteService]特性
	
		1.Name：公开服务名称，默认为类名
		
		2.Singleton ：服务是否单例，如果是则只会创建一次并缓存
		
	2.在需要公开方法上添加[RemoteMethod]特性
	
		1.Name ： 公开方法名，默认为方法名
		
		
四、NDWR配置类GlobalConfig
=================

	1.LogFactory  => 设置日志工厂
	
	2.JsonSerializer => json序列化工具
	
		实现类需继承 IJsonSerializer
		
		默认提供NewtonsoftJsonSerializerImpl实现
		
	3.ServiceScanner => 服务扫描类
	
		实现类需继承 IServiceScanner
		
		默认提供 AttributeServiceScanner实现
		
	4.Interceptors => 拦截器列表
	
		拦截器需继承 Interceptor
		
		默认提供实现的拦截器有：
		
			1.ExceptionInterceptor ： 异常捕获
			
			2.ParamConvertInterceptor ： 参数转换
			
			3.DownloadInterceptor ： 下载拦截
			
		可以自定义拦截器添加到Interceptors中
		
			eg:Interceptors.Add(new MyInterceptor());
			
	5.默认配置方法DefaultConfig提供了一个默认的配置
	
	  需要参数为服务所在程序集名
	  
		
五、Web端配置
=================

	1.引入核心库
	
		<script src="ndwr/ndwrcore.ashx" type="text/javascript"></script>
		
	2.引入公开服务方法库
	
		<script src="ndwr/[Service1].ashx" type="text/javascript"></script>
		
		<script src="ndwr/[Service2].ashx" type="text/javascript"></script>
		
		其中[Service]为公开服务的指定名
		
	3.引入库的路径是以 /[website]/ndwr/xx.ashx
	
	  这样引用可以是脚本路径统一，方便缓存

六、公开方法调用
=================

	1.  无回调调用
	
		[Service].[Method](param1,param2,...,paramx);
		
	2.  // 回调方法简单方式
	
		[Service].[Method](param1,param2,...,paramx,function(data){
			// do something...
		}); 

	3.  // 包含详细信息回调
	
		[Service].[Method](param1,param2,...,paramx,{
			callBack : function(data){ // 回调 必选
				// do something...
			},
			errorHandler : function(errors){ // 服务端错误处理句柄 可选
				// do something
			},
			transferMode : ['xhr']|['iframe']|['tagScript'], // 传输模式 可选 
			timeout : 0 // 传输超时 可选
		}); 

七、上传下载
=================

	1.上传所传参数为 file控件的Node,file控件必须有name属性，映射服务端参数类型为TransferFile
		<input type="file" id="myfile" name="myfile" />
		eg:[Service].[Method](document.getElementById('myfile'),function(data){});

	2.下载操作服务端返回值类型为TransferFile，下载操作中的data为服务端文件的文件id，在通过id请求文件
		具体操作如下：
		[Service].[Method](function(data){
			ndwr.transport.download(data); // 真正开始下载文件
		});

八、批量调用
=================

	ndwr.BeginBatch();
	[Service].[Method1](param1,param2,...,paramx,function(data){});
	[Service].[Method2](param1,param2,...,paramx,function(data){});
	...
	[Service].[Methodx](param1,param2,...,paramx,function(data){});
    ndwr.EndBatch();
	
九、web端调用事件及设置
=================

	事件或配置                             类型                 描述
	ndwr.beforeTransferEvent               function             远程方法执行前事件
	ndwr.afterTransferEvent                function             远程方法执行后事件
	ndwr.errorHandler                      function             服务端错误处理句柄
	ndwr.dataFilter                        function             服务端返回数据过滤
	ndwr.transport.timeout                 number               超时
	ndwr.transport.mode                    ['xhr']|             传输模式
										   ['iframe']|
										   ['tagScript']
	ndwr.transport.url                     string               服务端url
	ndwr.transport.sendingHandler          function             开始发送请求
	ndwr.transport.completedHandler        function             完成请求
	ndwr.transport.errorHandler            function             请求错误处理句柄
		
十、注意
=================

	1.公开的方法和服务名不能以ndwr开头
	
	2.不要尝试在ndwr里注入自定义参数名，这样可能会导致解析歧义
	
	3.方法名不要使用FuncSwitch
	
	4.服务名不能重名，方法名一样
	