using System.Collections;
using System;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLTools{

	public static void GetXML(string _str,Action<string,string> _cb){

		XmlDocument xmlDoc = new XmlDocument();

		xmlDoc.LoadXml(XmlFix(_str));

		XmlNodeList hhh = xmlDoc.DocumentElement.ChildNodes;

		foreach (XmlNode node in hhh)
		{
			if (node.Name != "#comment")
			{
				_cb(node.Name,node.InnerText);
			}
		}
	}

	private static string XmlFix(string _str)
	{
		int index = _str.IndexOf("<");

		if (index == -1)
		{
			return "";
		}
		else
		{
			return _str.Substring(index, _str.Length - index);
		}
	}
}
