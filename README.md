һ�����
=================

DWR ����2 ����Ҫ���֣� 
-------------------------------------------------

*1.һ�������ڷ������˵�HandlerFactory���������������������������Ӧ�� 
*2.������������˵�JavaScript��������������һ��ܶ�̬������ҳ�� 

��������һ��NDWR����
=================

*1.���򼯣�NDWR.dll ��Newtonsoft.Json.dll(json���л�������)
	
*2.web.config�����ļ���
		<system.web>
			<httpHandlers>
				<add verb="*" path="ndwr/*.ashx" type="NDWR.Web.Handler.HandlerFactory,NDWR" />
			</httpHandlers>
		</system.web>
*3.Global.asax -> Application_Start �¼�
		void Application_Start(object sender, EventArgs e) 
		{
			//��Ӧ�ó�������ʱ���еĴ���
			NDWR.Config.GlobalConfig.Instance.DefaultConfig("RemoteService");
			// ��������Լ����������������մ˷�ʽ���
			//NDWR.Config.GlobalConfig.Instance.Interceptors.Add(new RemoteService.NHVEntityValidateInterceptor());
		}
	
*4.��дservice
	[RemoteService]
	public class RemoteDemoNew {

		[RemoteMethod]
		public string HW() {
			return "Hello World";
		}
	}
	
*5.ǰ̨�ű�
		<script src="ndwr/ndwrcore.ashx" type="text/javascript"></script><!-- ndwr���Ŀ� -->
		<script src="ndwr/RemoteDemoNew.ashx" type="text/javascript"></script><!-- ���ŷ��� -->
		<script type="text/javascript">
            function HW() {
                RemoteDemoNew.HW(function(data){
					alert(data);
				});
            }
        </script>
		<button onclick="HW()"></button>
		
����NDWR��������
=================

	1.����Ҫ���������������[RemoteService]����
	
		1.Name�������������ƣ�Ĭ��Ϊ����
		
		2.Singleton �������Ƿ������������ֻ�ᴴ��һ�β�����
		
	2.����Ҫ�������������[RemoteMethod]����
	
		1.Name �� ������������Ĭ��Ϊ������
		
		
�ġ�NDWR������GlobalConfig
=================

	1.LogFactory  => ������־����
	
	2.JsonSerializer => json���л�����
	
		ʵ������̳� IJsonSerializer
		
		Ĭ���ṩNewtonsoftJsonSerializerImplʵ��
		
	3.ServiceScanner => ����ɨ����
	
		ʵ������̳� IServiceScanner
		
		Ĭ���ṩ AttributeServiceScannerʵ��
		
	4.Interceptors => �������б�
	
		��������̳� Interceptor
		
		Ĭ���ṩʵ�ֵ��������У�
		
			1.ExceptionInterceptor �� �쳣����
			
			2.ParamConvertInterceptor �� ����ת��
			
			3.DownloadInterceptor �� ��������
			
		�����Զ�����������ӵ�Interceptors��
		
			eg:Interceptors.Add(new MyInterceptor());
			
	5.Ĭ�����÷���DefaultConfig�ṩ��һ��Ĭ�ϵ�����
	
	  ��Ҫ����Ϊ�������ڳ�����
	  
		
�塢Web������
=================

	1.������Ŀ�
	
		<script src="ndwr/ndwrcore.ashx" type="text/javascript"></script>
		
	2.���빫�����񷽷���
	
		<script src="ndwr/[Service1].ashx" type="text/javascript"></script>
		
		<script src="ndwr/[Service2].ashx" type="text/javascript"></script>
		
		����[Service]Ϊ���������ָ����
		
	3.������·������ /[website]/ndwr/xx.ashx
	
	  �������ÿ����ǽű�·��ͳһ�����㻺��

����������������
=================

	1.  �޻ص�����
	
		[Service].[Method](param1,param2,...,paramx);
		
	2.  // �ص������򵥷�ʽ
	
		[Service].[Method](param1,param2,...,paramx,function(data){
			// do something...
		}); 

	3.  // ������ϸ��Ϣ�ص�
	
		[Service].[Method](param1,param2,...,paramx,{
			callBack : function(data){ // �ص� ��ѡ
				// do something...
			},
			errorHandler : function(errors){ // ����˴������� ��ѡ
				// do something
			},
			transferMode : ['xhr']|['iframe']|['tagScript'], // ����ģʽ ��ѡ 
			timeout : 0 // ���䳬ʱ ��ѡ
		}); 

�ߡ��ϴ�����
=================

	1.�ϴ���������Ϊ file�ؼ���Node,file�ؼ�������name���ԣ�ӳ�����˲�������ΪTransferFile
		<input type="file" id="myfile" name="myfile" />
		eg:[Service].[Method](document.getElementById('myfile'),function(data){});

	2.���ز�������˷���ֵ����ΪTransferFile�����ز����е�dataΪ������ļ����ļ�id����ͨ��id�����ļ�
		����������£�
		[Service].[Method](function(data){
			ndwr.transport.download(data); // ������ʼ�����ļ�
		});

�ˡ���������
=================

	ndwr.BeginBatch();
	[Service].[Method1](param1,param2,...,paramx,function(data){});
	[Service].[Method2](param1,param2,...,paramx,function(data){});
	...
	[Service].[Methodx](param1,param2,...,paramx,function(data){});
    ndwr.EndBatch();
	
�š�web�˵����¼�������
=================

	�¼�������                             ����                 ����
	ndwr.beforeTransferEvent               function             Զ�̷���ִ��ǰ�¼�
	ndwr.afterTransferEvent                function             Զ�̷���ִ�к��¼�
	ndwr.errorHandler                      function             ����˴�������
	ndwr.dataFilter                        function             ����˷������ݹ���
	ndwr.transport.timeout                 number               ��ʱ
	ndwr.transport.mode                    ['xhr']|             ����ģʽ
										   ['iframe']|
										   ['tagScript']
	ndwr.transport.url                     string               �����url
	ndwr.transport.sendingHandler          function             ��ʼ��������
	ndwr.transport.completedHandler        function             �������
	ndwr.transport.errorHandler            function             �����������
		
ʮ��ע��
=================

	1.�����ķ����ͷ�����������ndwr��ͷ
	
	2.��Ҫ������ndwr��ע���Զ�����������������ܻᵼ�½�������
	
	3.��������Ҫʹ��FuncSwitch
	
	4.����������������������һ��
	