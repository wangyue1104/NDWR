using System;
using System.Collections.Generic;


public interface InvocationMethod{
	InvocationMethod Instance {get;}
	IList<string> ErrorMessage {get;set;}
	object FuncSwitch(string methodName,object[] paramlst);
}

public class InvocationMethodSwitch : InvocationMethod {
	// must be global
	private Service service = new Service();
		
	public InvocationMethod Instance {
		get{ return new InvocationMethodSwitch(); }
	}
	
	public IList<string> ErrorMessage {get;set;}
	
	public object FuncSwitch(string methodName,object[] paramlst){
		
		object ret = null;
		
		switch(methodName){
			case "DoWork0" : {
					// DoWork0 params
					int DoWork0_index = (int)paramlst[0] ;
					DateTime DoWork0_date = (DateTime) paramlst[1];
					ret = service.DoWork0(DoWork0_index,DoWork0_date);
				}
				break;
			case "DoWork1" : {
					// DoWork2 params
					string DoWork1_str = (string)paramlst[0];
					ret = service.DoWork1(DoWork1_str);
				}
				break;
			case "DoWork2" :  {
					// DoWork2 params
					long DoWork2_length = (long)paramlst[0];
					ret = service.DoWork2(DoWork2_length);
				}
				break;
			default : throw new Exception("No Matching");
		}
		ErrorMessage = service.ErrorMessage;
		
		return ret;
	}
}

public class Service {

	public IList<string> ErrorMessage {get;set;}
	
	public IList<Object> DoWork0(int index,DateTime date){
		// do something...
        return null;
	}
	
	public int DoWork1(string str){
        // do something...
        return 0;
	}
	
	public string DoWork2(long length){
        // do something...
        return null;
	}
}
