using UnityEngine;
using System.Collections;
using System;

public class SuperDebug{

	public static void Log(object _str){
		#if !LOG_DISABLE
		Debug.Log(_str);
//		RecordLog.Write(_str as String);
		#endif
	}

	public static void Log(object _str,UnityEngine.Object _context){
		#if !LOG_DISABLE
		Debug.Log(_str,_context);
		#endif
	}

	public static void LogError(object _str){
		#if !LOG_DISABLE
		Debug.LogError(_str);
		#endif
	}
	
	public static void LogError(object _str,UnityEngine.Object _context){
		#if !LOG_DISABLE
		Debug.LogError(_str,_context);
		#endif
	}

	public static void LogWarning(object _str){
		#if !LOG_DISABLE
		Debug.LogWarning(_str);
		#endif
	}
	
	public static void LogWarning(object _str,UnityEngine.Object _context){
		#if !LOG_DISABLE
		Debug.LogWarning(_str,_context);
		#endif
	}

	public static void LogWarningFormat(string format, params object[] args){
		#if !LOG_DISABLE
		Debug.LogWarningFormat(format,args);
		#endif
	}

	public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args){
		#if !LOG_DISABLE
		Debug.LogWarningFormat(context,format,args);
		#endif
	}

	public static void LogException(Exception _str){
		#if !LOG_DISABLE
		Debug.LogException(_str);
		#endif
	}
	
	public static void LogException(Exception _str,UnityEngine.Object _context){
		#if !LOG_DISABLE
		Debug.LogException(_str,_context);
		#endif
	}

	public static void LogErrorFormat(string format, params object[] args){
		#if !LOG_DISABLE
		Debug.LogErrorFormat(format,args);
		#endif
	}

	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args){
		#if !LOG_DISABLE
		Debug.LogErrorFormat(context,format,args);
		#endif
	}

	public static void Log(Color color, string str){
		#if !LOG_DISABLE
		string col = "<color=#" + ColorToHexStr(color) + ">";
		Debug.Log(col + str + "</color>");
		#endif
	}

	public static void LogWarning(Color color, string str){
		#if !LOG_DISABLE
		string col = "<color=#" + ColorToHexStr(color) + ">";
		Debug.LogWarning(col + str + "</color>");
		#endif
	}

	public static void LogError(Color color, string str){
		#if !LOG_DISABLE
		string col = "<color=#" + ColorToHexStr(color) + ">";
		Debug.LogError(col + str + "</color>");
		#endif
	}

	static string ColorToHexStr(Color col) 
	{ 
		Color32 col32 = (Color32)col;
		byte r = col32.r;
		byte g = col32.g;
		byte b = col32.b;
		byte a = col32.a;
		string returnStr = ""; 
		return r.ToString ("X2") + g.ToString ("X2") + b.ToString ("X2") + a.ToString ("X2");
	} 
}
