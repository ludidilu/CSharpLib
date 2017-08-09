using UnityEngine;
using System.Collections.Generic;
using System;

namespace superFunction
{
    public class SuperFunction
    {
        private static SuperFunction _Instance;

        public static SuperFunction Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SuperFunction();
                }

                return _Instance;
            }
        }

        public delegate void SuperFunctionCallBack0(int _index);
        public delegate void SuperFunctionCallBack1<T1>(int _index, T1 t1);
        public delegate void SuperFunctionCallBack2<T1, T2>(int _index, T1 t1, T2 t2);
        public delegate void SuperFunctionCallBack3<T1, T2, T3>(int _index, T1 t1, T2 t2, T3 t3);
        public delegate void SuperFunctionCallBack4<T1, T2, T3, T4>(int _index, T1 t1, T2 t2, T3 t3, T4 t4);

        public delegate void SuperFunctionCallBackV0<T>(int _index, ref T t);
        public delegate void SuperFunctionCallBackV1<T, T1>(int _index, ref T t, T1 t1);
        public delegate void SuperFunctionCallBackV2<T, T1, T2>(int _index, ref T t, T1 t1, T2 t2);
        public delegate void SuperFunctionCallBackV3<T, T1, T2, T3>(int _index, ref T t, T1 t1, T2 t2, T3 t3);
        public delegate void SuperFunctionCallBackV4<T, T1, T2, T3, T4>(int _index, ref T t, T1 t1, T2 t2, T3 t3, T4 t4);

        private Dictionary<int, SuperFunctionUnit> dic;
        private Dictionary<GameObject, Dictionary<string, List<SuperFunctionUnit>>> dic2;

        private int index = 0;

        private Queue<List<SuperFunctionUnit>> pool = new Queue<List<SuperFunctionUnit>>();
        private Queue<Dictionary<string, List<SuperFunctionUnit>>> pool2 = new Queue<Dictionary<string, List<SuperFunctionUnit>>>();

        public SuperFunction()
        {
            dic = new Dictionary<int, SuperFunctionUnit>();
            dic2 = new Dictionary<GameObject, Dictionary<string, List<SuperFunctionUnit>>>();
        }

        public int AddOnceEventListener(GameObject _target, string _eventName, SuperFunctionCallBack0 _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener(GameObject _target, string _eventName, SuperFunctionCallBack0 _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T1>(GameObject _target, string _eventName, SuperFunctionCallBack1<T1> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T1>(GameObject _target, string _eventName, SuperFunctionCallBack1<T1> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBack2<T1, T2> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBack2<T1, T2> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBack3<T1, T2, T3> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBack3<T1, T2, T3> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBack4<T1, T2, T3, T4> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBack4<T1, T2, T3, T4> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T>(GameObject _target, string _eventName, SuperFunctionCallBackV0<T> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T>(GameObject _target, string _eventName, SuperFunctionCallBackV0<T> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T, T1>(GameObject _target, string _eventName, SuperFunctionCallBackV1<T, T1> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T, T1>(GameObject _target, string _eventName, SuperFunctionCallBackV1<T, T1> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T, T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBackV2<T, T1, T2> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T, T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBackV2<T, T1, T2> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T, T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBackV3<T, T1, T2, T3> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T, T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBackV3<T, T1, T2, T3> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        public int AddOnceEventListener<T, T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBackV4<T, T1, T2, T3, T4> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, true);
        }

        public int AddEventListener<T, T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBackV4<T, T1, T2, T3, T4> _callBack)
        {
            return AddEventListener(_target, _eventName, _callBack, false);
        }

        private int AddEventListener(GameObject _target, string _eventName, Delegate _callBack, bool _isOnce)
        {
            int result = GetIndex();

            SuperFunctionUnit unit = new SuperFunctionUnit(_target, _eventName, _callBack, result, _isOnce);

            dic.Add(result, unit);

            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (!dic2.TryGetValue(_target, out tmpDic))
            {
                _target.AddComponent<SuperFunctionControl>();

                tmpDic = GetDic();

                dic2.Add(_target, tmpDic);
            }

            List<SuperFunctionUnit> tmpList;

            if (!tmpDic.TryGetValue(_eventName, out tmpList))
            {
                tmpList = GetList();

                tmpDic.Add(_eventName, tmpList);
            }

            tmpList.Add(unit);

            return result;
        }

        public void RemoveEventListener(int _index)
        {
            SuperFunctionUnit unit;

            if (dic.TryGetValue(_index, out unit))
            {
                dic.Remove(_index);

                Dictionary<string, List<SuperFunctionUnit>> tmpDic = dic2[unit.target];

                List<SuperFunctionUnit> tmpList = tmpDic[unit.eventName];

                for (int i = 0; i < tmpList.Count; i++)
                {
                    if (tmpList[i].index == unit.index)
                    {
                        tmpList.RemoveAt(i);

                        break;
                    }
                }

                if (tmpList.Count == 0)
                {
                    ReleaseList(tmpList);

                    tmpDic.Remove(unit.eventName);

                    if (tmpDic.Count == 0)
                    {
                        ReleaseDic(tmpDic);

                        DestroyControl(unit.target);
                    }
                }
            }
        }

        public void RemoveEventListener(GameObject _target)
        {
            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (dic2.TryGetValue(_target, out tmpDic))
            {
                DestroyControl(_target);

                Dictionary<string, List<SuperFunctionUnit>>.ValueCollection.Enumerator enumerator = tmpDic.Values.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    List<SuperFunctionUnit> tmpList = enumerator.Current;

                    for (int i = 0; i < tmpList.Count; i++)
                    {
                        SuperFunctionUnit unit = tmpList[i];

                        dic.Remove(unit.index);
                    }

                    tmpList.Clear();

                    ReleaseList(tmpList);
                }

                tmpDic.Clear();

                ReleaseDic(tmpDic);
            }
        }

        public void RemoveEventListener(GameObject _target, string _eventName)
        {
            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (dic2.TryGetValue(_target, out tmpDic))
            {
                List<SuperFunctionUnit> list;

                if (tmpDic.TryGetValue(_eventName, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SuperFunctionUnit unit = list[i];

                        dic.Remove(unit.index);
                    }

                    list.Clear();

                    ReleaseList(list);

                    tmpDic.Remove(_eventName);

                    if (tmpDic.Count == 0)
                    {
                        ReleaseDic(tmpDic);

                        DestroyControl(_target);
                    }
                }
            }
        }

        public void RemoveEventListener(GameObject _target, string _eventName, SuperFunctionCallBack0 _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T1>(GameObject _target, string _eventName, SuperFunctionCallBack1<T1> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBack2<T1, T2> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBack3<T1, T2, T3> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBack4<T1, T2, T3, T4> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T>(GameObject _target, string _eventName, SuperFunctionCallBackV0<T> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T, T1>(GameObject _target, string _eventName, SuperFunctionCallBackV1<T, T1> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T, T1, T2>(GameObject _target, string _eventName, SuperFunctionCallBackV2<T, T1, T2> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T, T1, T2, T3>(GameObject _target, string _eventName, SuperFunctionCallBackV3<T, T1, T2, T3> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        public void RemoveEventListener<T, T1, T2, T3, T4>(GameObject _target, string _eventName, SuperFunctionCallBackV4<T, T1, T2, T3, T4> _callBack)
        {
            RemoveEventListenerReal(_target, _eventName, _callBack);
        }

        private void RemoveEventListenerReal(GameObject _target, string _eventName, Delegate _callBack)
        {
            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (dic2.TryGetValue(_target, out tmpDic))
            {
                List<SuperFunctionUnit> list;

                if (tmpDic.TryGetValue(_eventName, out list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SuperFunctionUnit unit = list[i];

                        if (unit.callBack == _callBack)
                        {
                            dic.Remove(unit.index);

                            list.RemoveAt(i);

                            break;
                        }
                    }

                    if (list.Count == 0)
                    {
                        ReleaseList(list);

                        tmpDic.Remove(_eventName);

                        if (tmpDic.Count == 0)
                        {
                            ReleaseDic(tmpDic);

                            DestroyControl(_target);
                        }
                    }
                }
            }
        }

        public bool DispatchEvent(GameObject _target, string _eventName)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBack0>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBack0 cb = unit.callBack as SuperFunctionCallBack0;

                    cb(unit.index);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T1>(GameObject _target, string _eventName, T1 t1)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBack1<T1>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBack1<T1> cb = unit.callBack as SuperFunctionCallBack1<T1>;

                    cb(unit.index, t1);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T1, T2>(GameObject _target, string _eventName, T1 t1, T2 t2)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBack2<T1, T2>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBack2<T1, T2> cb = unit.callBack as SuperFunctionCallBack2<T1, T2>;

                    cb(unit.index, t1, t2);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T1, T2, T3>(GameObject _target, string _eventName, T1 t1, T2 t2, T3 t3)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBack3<T1, T2, T3>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBack3<T1, T2, T3> cb = unit.callBack as SuperFunctionCallBack3<T1, T2, T3>;

                    cb(unit.index, t1, t2, t3);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T1, T2, T3, T4>(GameObject _target, string _eventName, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBack4<T1, T2, T3, T4>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBack4<T1, T2, T3, T4> cb = unit.callBack as SuperFunctionCallBack4<T1, T2, T3, T4>;

                    cb(unit.index, t1, t2, t3, t4);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T>(GameObject _target, string _eventName, ref T _t)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBackV0<T>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBackV0<T> cb = unit.callBack as SuperFunctionCallBackV0<T>;

                    cb(unit.index, ref _t);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T, T1>(GameObject _target, string _eventName, ref T _t, T1 _t1)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBackV1<T, T1>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBackV1<T, T1> cb = unit.callBack as SuperFunctionCallBackV1<T, T1>;

                    cb(unit.index, ref _t, _t1);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T, T1, T2>(GameObject _target, string _eventName, ref T _t, T1 _t1, T2 _t2)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBackV2<T, T1, T2>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBackV2<T, T1, T2> cb = unit.callBack as SuperFunctionCallBackV2<T, T1, T2>;

                    cb(unit.index, ref _t, _t1, _t2);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T, T1, T2, T3>(GameObject _target, string _eventName, ref T _t, T1 _t1, T2 _t2, T3 _t3)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBackV3<T, T1, T2, T3>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBackV3<T, T1, T2, T3> cb = unit.callBack as SuperFunctionCallBackV3<T, T1, T2, T3>;

                    cb(unit.index, ref _t, _t1, _t2, _t3);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DispatchEvent<T, T1, T2, T3, T4>(GameObject _target, string _eventName, ref T _t, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            List<SuperFunctionUnit> unitList = DispatchEventReal<SuperFunctionCallBackV4<T, T1, T2, T3, T4>>(_target, _eventName);

            if (unitList != null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    SuperFunctionUnit unit = unitList[i];

                    if (unit.isOnce)
                    {
                        RemoveEventListener(unit.index);
                    }

                    SuperFunctionCallBackV4<T, T1, T2, T3, T4> cb = unit.callBack as SuperFunctionCallBackV4<T, T1, T2, T3, T4>;

                    cb(unit.index, ref _t, _t1, _t2, _t3, _t4);
                }

                unitList.Clear();

                ReleaseList(unitList);

                return true;
            }
            else
            {
                return false;
            }
        }

        private List<SuperFunctionUnit> DispatchEventReal<T>(GameObject _target, string _eventName)
        {
            List<SuperFunctionUnit> result = null;

            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (dic2.TryGetValue(_target, out tmpDic))
            {
                List<SuperFunctionUnit> tmpList;

                if (tmpDic.TryGetValue(_eventName, out tmpList))
                {
                    for (int i = 0; i < tmpList.Count; i++)
                    {
                        SuperFunctionUnit unit = tmpList[i];

                        if (unit.callBack is T)
                        {
                            if (result == null)
                            {
                                result = GetList();
                            }

                            result.Add(unit);
                        }
                    }
                }
            }

            return result;
        }

        public void DestroyGameObject(GameObject _target)
        {
            Dictionary<string, List<SuperFunctionUnit>> tmpDic;

            if (dic2.TryGetValue(_target, out tmpDic))
            {
                dic2.Remove(_target);

                Dictionary<string, List<SuperFunctionUnit>>.ValueCollection.Enumerator enumerator = tmpDic.Values.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    List<SuperFunctionUnit> tmpList = enumerator.Current;

                    for (int i = 0; i < tmpList.Count; i++)
                    {
                        SuperFunctionUnit unit = tmpList[i];

                        dic.Remove(unit.index);
                    }

                    tmpList.Clear();

                    ReleaseList(tmpList);
                }

                tmpDic.Clear();

                ReleaseDic(tmpDic);
            }
        }

        private void DestroyControl(GameObject _target)
        {
            SuperFunctionControl[] controls = _target.GetComponents<SuperFunctionControl>();

            for (int i = 0; i < controls.Length; i++)
            {
                SuperFunctionControl control = controls[i];

                if (!control.isDestroy)
                {
                    control.isDestroy = true;

                    GameObject.Destroy(control);
                }
            }

            dic2.Remove(_target);
        }

        private int GetIndex()
        {
            index++;

            int result = index;

            return result;
        }

        private List<SuperFunctionUnit> GetList()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                List<SuperFunctionUnit> list = new List<SuperFunctionUnit>();

                return list;
            }
        }

        private void ReleaseList(List<SuperFunctionUnit> _list)
        {
            pool.Enqueue(_list);
        }

        private Dictionary<string, List<SuperFunctionUnit>> GetDic()
        {
            if (pool2.Count > 0)
            {
                return pool2.Dequeue();
            }
            else
            {
                Dictionary<string, List<SuperFunctionUnit>> dic = new Dictionary<string, List<SuperFunctionUnit>>();

                return dic;
            }
        }

        private void ReleaseDic(Dictionary<string, List<SuperFunctionUnit>> _dic)
        {
            pool2.Enqueue(_dic);
        }

        public int GetNum()
        {
            return dic.Count;
        }
    }
}