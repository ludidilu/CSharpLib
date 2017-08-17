using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using wwwManager;
using thread;
using System.Threading;
using publicTools;

public class StaticData
{
    public static string path;

    public const string datName = "csv.dat";

    public static Dictionary<Type, IDictionary> dic;

    private static Dictionary<Type, IList> dicList = new Dictionary<Type, IList>();

    public static T GetData<T>(int _id) where T : CsvBase
    {
        Dictionary<int, T> tmpDic = dic[typeof(T)] as Dictionary<int, T>;

        T data;

        if (!tmpDic.TryGetValue(_id, out data))
        {
            SuperDebug.LogError(typeof(T).Name + "表中未找到ID为:" + _id + "的行!");
        }

        return data;
    }

    public static T GetDataOfKey<T>(string key, object keyValueParam) where T : CsvBase
    {
        Dictionary<int, T> dict = GetDic<T>();

        Type type = typeof(T);

        FieldInfo field = type.GetField(key);

        Dictionary<int, T>.ValueCollection.Enumerator enumerator = dict.Values.GetEnumerator();

        while (enumerator.MoveNext())
        {

            T item = enumerator.Current;

            object keyValue = field.GetValue(item);

            if (keyValue.Equals(keyValueParam))
            {
                return item;
            }
        }

        return default(T);
    }

    public static Dictionary<int, T> GetDic<T>() where T : CsvBase
    {
        Type type = typeof(T);

        IDictionary data;

        if (!dic.TryGetValue(type, out data))
        {
            SuperDebug.LogError("not find: " + type);
        }

        return data as Dictionary<int, T>;
    }

    public static List<T> GetList<T>() where T : CsvBase
    {
        Type type = typeof(T);

        IList data;

        if (!dicList.TryGetValue(type, out data))
        {
            Dictionary<int, T> dict = GetDic<T>();

            data = new List<T>();

            Dictionary<int, T>.ValueCollection.Enumerator enumerator = dict.Values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                data.Add(enumerator.Current);
            }

            dicList.Add(type, data);
        }

        return data as List<T>;
    }

    public static bool IsIDExists<T>(int _id) where T : CsvBase
    {
        Dictionary<int, T> dict = GetDic<T>();

        return dict.ContainsKey(_id);
    }

    public static void LoadCsvDataFromFile(Action _callBack, Func<byte[], Dictionary<Type, IDictionary>> _getDicCallBack)
    {
        ParameterizedThreadStart cb2 = delegate (object obj)
        {
            dic = _getDicCallBack(obj as byte[]);
        };

        Action<WWW> cb = delegate (WWW obj)
        {
            ThreadScript.Instance.Add(cb2, obj.bytes, _callBack);
        };

        WWWManager.Instance.Load(datName, cb);
    }
    
    public static void Dispose()
    {
        dic = new Dictionary<Type, IDictionary>();
    }

    public static void Load<T>(string _name) where T : CsvBase, new()
    {
        Type type = typeof(T);

        if (dic.ContainsKey(type))
        {
            return;
        }

        Dictionary<int, T> result = new Dictionary<int, T>();

        using (FileStream fs = new FileStream(path + "/" + _name + ".csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                int i = 0;

                string lineStr = reader.ReadLine();

                FieldInfo[] infoArr = null;

                while (lineStr != null)
                {
                    if (i == 2)
                    {
                        string[] dataArr = lineStr.Split(',');

                        infoArr = new FieldInfo[dataArr.Length];

                        for (int m = 1; m < dataArr.Length; m++)
                        {
                            infoArr[m] = type.GetField(dataArr[m]);
                        }
                    }
                    else if (i > 2)
                    {
                        string[] dataArr = lineStr.Split(',');

                        T csv = new T();

                        csv.ID = Int32.Parse(dataArr[0]);

                        for (int m = 1; m < infoArr.Length; m++)
                        {
                            FieldInfo info = infoArr[m];

                            if (info != null)
                            {
                                SetData(info, csv, dataArr[m]);
                            }
                        }

                        csv.Fix();

                        result.Add(csv.ID, csv);
                    }

                    i++;

                    lineStr = reader.ReadLine();
                }
            }
        }

        dic.Add(type, result);
    }

    private static void SetData(FieldInfo _info, CsvBase _csv, string _data)
    {
        try
        {
            switch (_info.FieldType.Name)
            {
                case "Int32":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, int.Parse(_data));
                    }

                    break;

                case "String":

                    _info.SetValue(_csv, PublicTools.FixStringChangeLine(_data));

                    break;

                case "Boolean":

                    _info.SetValue(_csv, _data == "1" ? true : false);

                    break;

                case "Single":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, float.Parse(_data));
                    }

                    break;

                case "Double":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, double.Parse(_data));
                    }

                    break;

                case "Int16":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, short.Parse(_data));
                    }

                    break;

                case "Int32[]":

                    int[] intResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        intResult = new int[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            intResult[i] = int.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        intResult = new int[0];
                    }

                    _info.SetValue(_csv, intResult);

                    break;

                case "String[]":

                    string[] stringResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] tmpStr = _data.Split('$');

                        stringResult = new string[tmpStr.Length];

                        for (int i = 0; i < tmpStr.Length; i++)
                        {

                            stringResult[i] = PublicTools.FixStringChangeLine(tmpStr[i]);
                        }
                    }
                    else
                    {
                        stringResult = new string[0];
                    }

                    _info.SetValue(_csv, stringResult);

                    break;

                case "Boolean[]":

                    bool[] boolResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        boolResult = new bool[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            boolResult[i] = strArr[i] == "1" ? true : false;
                        }
                    }
                    else
                    {
                        boolResult = new bool[0];
                    }

                    _info.SetValue(_csv, boolResult);

                    break;

                case "Single[]":

                    float[] floatResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        floatResult = new float[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            floatResult[i] = float.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        floatResult = new float[0];
                    }

                    _info.SetValue(_csv, floatResult);

                    break;

                case "Double[]":

                    double[] doubleResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        doubleResult = new double[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            doubleResult[i] = double.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        doubleResult = new double[0];
                    }

                    _info.SetValue(_csv, doubleResult);

                    break;

                case "Int16[]":

                    short[] shortResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split('$');

                        shortResult = new short[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            shortResult[i] = short.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        shortResult = new short[0];
                    }

                    _info.SetValue(_csv, shortResult);

                    break;

                default:

                    throw new Exception("csv表中的类型不支持反射  setData:" + _info.Name + "   " + _info.FieldType.Name + "   " + _data);
            }
        }
        catch (Exception e)
        {
            string str = "setData:" + _info.Name + "   " + _info.FieldType.Name + "   " + _data + "   " + _data.Length + Environment.NewLine;

            Console.WriteLine(str + "   " + e.ToString());
        }
    }
}
