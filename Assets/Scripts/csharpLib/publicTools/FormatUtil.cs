/// <summary>
/// Copyright by csm, update 2016.11.01
/// </summary>
using System.Collections.Generic;
using System;
using System.Text;

public class FormatUtil{

	public static byte[] Reverse(byte[] src){
		int length = src.Length;
		if (length <= 1)
			return src;
		byte[] ret = new byte[length];
		for (int i = 0; i < length; i++) {
			ret[i] = src[src.Length - i - 1];
		}
		return ret;
	}

	public static byte[] GetBytes(string s, Encoding encoding){
		return encoding.GetBytes (s);
	}

	public static byte[] GetBytes(int src){
		return BitConverter.GetBytes (src);
	}
	
	public static byte[] GetBytes(float src){
		return BitConverter.GetBytes (src);
	}
	
	public static byte[] GetBytes(short src){
		return BitConverter.GetBytes (src);
	}

	public static string GetString(byte[] src, Encoding encoding){
		return encoding.GetString (src);
	}

	public static uint GetUint(byte[] src, ref int startIndex){
		uint i = BitConverter.ToUInt16 (src, startIndex);
		startIndex += 2;
		return i;
	}

	public static int GetInt(byte[] src, ref int startIndex){
		int i = BitConverter.ToInt32 (src, startIndex);
		startIndex += 4;
		return i;
	}

	public static float GetFloat(byte[] src, ref int startIndex){
		float f = BitConverter.ToSingle (src, startIndex);
		startIndex += 4;
		return f;
	}

	public static short GetShort(byte[] src, ref int startIndex){
		short s = BitConverter.ToInt16 (src, startIndex);
		startIndex += 2;
		return s;
	}

	public static byte[] Combine(byte[] src, byte[] append){
		byte[] ret = new byte[src.Length + append.Length];
		src.CopyTo (ret, 0);
		append.CopyTo (ret, src.Length);
		return ret;
	}
	
	public static byte[] Combine(params byte[][] values){
		int sum = 0;
		for (int i = 0; i < values.Length; i++) {
			sum += values[i].Length;
		}
		byte[] ret = new byte[sum];
		int startIndex = 0;
		for (int i = 0; i < values.Length; i++) {
			values[i].CopyTo(ret,startIndex);
			startIndex += values[i].Length;
		}
		return ret;
	}
	
	public static byte[] Combine(params int[] values){
		int sum = values.Length * 4;
		byte[] ret = new byte[sum];
		int startIndex = 0;
		for (int i = 0; i < values.Length; i++) {
			BitConverter.GetBytes(values[i]).CopyTo(ret, startIndex);
			startIndex += 4;
		}
		return ret;
	}
	
	public static byte[] Combine(params short[] values){
		int sum = values.Length * 2;
		byte[] ret = new byte[sum];
		int startIndex = 0;
		for (int i = 0; i < values.Length; i++) {
			BitConverter.GetBytes(values[i]).CopyTo(ret, startIndex);
			startIndex += 2;
		}
		return ret;
	}
	
	public static byte[] Sub(byte[] src, int startIndex){
		return Sub (src, startIndex, src.Length - startIndex);
	}
	
	public static byte[] Sub(byte[] src, int startIndex, int length){
		byte[] ret = new byte[length];
		for(int i = 0; i < length; i++){
			ret[i] = src[i+startIndex];
		}
		return ret;
	}

	public static string Bytes2HexStrLinefeed(byte[] bytes){
		if (bytes == null || bytes.Length == 0)
			return "";

		string returnStr = ""; 
		for (int i = 0; i < bytes.Length; i++) {
			returnStr += bytes[i].ToString("X2") + " ";
			if(i % 16 == 15)
				returnStr += "\n";
		}
		return returnStr;
	}

	public static string Bytes2HexStr(byte[] bytes){
		if (bytes == null || bytes.Length == 0)
			return "";

		string returnStr = ""; 
		for (int i = 0; i < bytes.Length; i++) {
			returnStr += bytes[i].ToString("X2") + " ";
		}
		return returnStr;
	}

	public static byte[] HexStr2Bytes(string str){
		str = str.Replace(" ", ""); 
		if ((str.Length % 2) != 0) 
			str += " "; 
		byte[] returnBytes = new byte[str.Length / 2]; 
		for (int i = 0; i < returnBytes.Length; i++) {
			returnBytes [i] = Convert.ToByte (str.Substring (i * 2, 2), 16); 
		}
		return returnBytes; 

	}
}
