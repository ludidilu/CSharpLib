using UnityEngine;
using System.Collections.Generic;
using System;

namespace superTween
{
    public class SuperTweenScript : MonoBehaviour
    {
        private Dictionary<int, SuperTweenUnitBase> dic = new Dictionary<int, SuperTweenUnitBase>();

        private Dictionary<Action<float>, SuperTweenUnitBase> toDic = new Dictionary<Action<float>, SuperTweenUnitBase>();
        private int index;

        private List<SuperTweenUnitBase> endList = new List<SuperTweenUnitBase>();

        private List<KeyValuePair<SuperTweenUnitBase, float>> toList = new List<KeyValuePair<SuperTweenUnitBase, float>>();

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    unit.Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnitBase();

                    unit.Init(result, _startValue, _endValue, _time, _delegate, isFixed);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action _endCallBack)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    (unit as SuperTweenUnit).Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnit();

                    (unit as SuperTweenUnit).Init(result, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public int To<T1>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1> _endCallBack, T1 _t1)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    (unit as SuperTweenUnit<T1>).Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnit<T1>();

                    (unit as SuperTweenUnit<T1>).Init(result, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public int To<T1, T2>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    (unit as SuperTweenUnit<T1, T2>).Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnit<T1, T2>();

                    (unit as SuperTweenUnit<T1, T2>).Init(result, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public int To<T1, T2, T3>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    (unit as SuperTweenUnit<T1, T2, T3>).Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnit<T1, T2, T3>();

                    (unit as SuperTweenUnit<T1, T2, T3>).Init(result, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public int To<T1, T2, T3, T4>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (toDic.TryGetValue(_delegate, out unit))
                {
                    (unit as SuperTweenUnit<T1, T2, T3, T4>).Init(unit.index, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3, _t4);

                    return unit.index;
                }
                else
                {
                    int result = GetIndex();

                    unit = new SuperTweenUnit<T1, T2, T3, T4>();

                    (unit as SuperTweenUnit<T1, T2, T3, T4>).Init(result, _startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3, _t4);

                    dic.Add(result, unit);

                    toDic.Add(_delegate, unit);

                    return result;
                }
            }
        }

        public void SetTag(int _index, string _tag)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (dic.TryGetValue(_index, out unit))
                {
                    unit.tag = _tag;
                }
            }
        }

        public void Remove(int _index, bool _toEnd)
        {
            lock (dic)
            {
                SuperTweenUnitBase unit;

                if (dic.TryGetValue(_index, out unit))
                {
                    dic.Remove(_index);

                    if (unit.dele != null)
                    {
                        toDic.Remove(unit.dele);
                    }

                    if (_toEnd)
                    {
                        if (unit.dele != null)
                        {
                            unit.dele(unit.endValue);
                        }

                        unit.End();
                    }

                    unit.isRemoved = true;
                }
            }
        }

        public void RemoveAll(bool _toEnd)
        {
            lock (dic)
            {
                Dictionary<int, SuperTweenUnitBase> tmpDic = dic;

                dic = new Dictionary<int, SuperTweenUnitBase>();
                toDic = new Dictionary<Action<float>, SuperTweenUnitBase>();

                Dictionary<int, SuperTweenUnitBase>.ValueCollection.Enumerator enumerator = tmpDic.Values.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    SuperTweenUnitBase unit = enumerator.Current;

                    if (_toEnd)
                    {
                        if (unit.dele != null)
                        {
                            unit.dele(unit.endValue);
                        }

                        unit.End();
                    }

                    unit.isRemoved = true;
                }
            }
        }

        public void RemoveWithTag(string _tag, bool _toEnd)
        {
            lock (dic)
            {
                List<SuperTweenUnitBase> list = new List<SuperTweenUnitBase>();

                Dictionary<int, SuperTweenUnitBase>.ValueCollection.Enumerator enumerator = dic.Values.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    SuperTweenUnitBase unit = enumerator.Current;

                    if (unit.tag != null && unit.tag.Equals(_tag))
                    {
                        list.Add(unit);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    SuperTweenUnitBase unit = list[i];

                    dic.Remove(unit.index);

                    if (unit.dele != null)
                    {
                        toDic.Remove(unit.dele);
                    }

                    if (_toEnd)
                    {
                        if (unit.dele != null)
                        {
                            unit.dele(unit.endValue);
                        }

                        unit.End();
                    }

                    unit.isRemoved = true;
                }
            }
        }

        public int DelayCall(float _time, bool isFixed, Action _endCallBack)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit unit = new SuperTweenUnit();

                unit.Init(result, 0, 0, _time, null, isFixed, _endCallBack);

                dic.Add(result, unit);

                return result;
            }
        }

        public int DelayCall<T1>(float _time, bool isFixed, Action<T1> _endCallBack, T1 _t1)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1> unit = new SuperTweenUnit<T1>();

                unit.Init(result, 0, 0, _time, null, isFixed, _endCallBack, _t1);

                dic.Add(result, unit);

                return result;
            }
        }

        public int DelayCall<T1, T2>(float _time, bool isFixed, Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2> unit = new SuperTweenUnit<T1, T2>();

                unit.Init(result, 0, 0, _time, null, isFixed, _endCallBack, _t1, _t2);

                dic.Add(result, unit);

                return result;
            }
        }

        public int DelayCall<T1, T2, T3>(float _time, bool isFixed, Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2, T3> unit = new SuperTweenUnit<T1, T2, T3>();

                unit.Init(result, 0, 0, _time, null, isFixed, _endCallBack, _t1, _t2, _t3);

                dic.Add(result, unit);

                return result;
            }
        }

        public int DelayCall<T1, T2, T3, T4>(float _time, bool isFixed, Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2, T3, T4> unit = new SuperTweenUnit<T1, T2, T3, T4>();

                unit.Init(result, 0, 0, _time, null, isFixed, _endCallBack, _t1, _t2, _t3, _t4);

                dic.Add(result, unit);

                return result;
            }
        }

        public int NextFrameCall(Action _endCallBack)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit unit = new SuperTweenUnit();

                unit.Init(result, 0, 0, 0, null, false, _endCallBack);

                dic.Add(result, unit);

                return result;
            }
        }

        public int NextFrameCall<T1>(Action<T1> _endCallBack, T1 _t1)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1> unit = new SuperTweenUnit<T1>();

                unit.Init(result, 0, 0, 0, null, false, _endCallBack, _t1);

                dic.Add(result, unit);

                return result;
            }
        }

        public int NextFrameCall<T1, T2>(Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2> unit = new SuperTweenUnit<T1, T2>();

                unit.Init(result, 0, 0, 0, null, false, _endCallBack, _t1, _t2);

                dic.Add(result, unit);

                return result;
            }
        }

        public int NextFrameCall<T1, T2, T3>(Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2, T3> unit = new SuperTweenUnit<T1, T2, T3>();

                unit.Init(result, 0, 0, 0, null, false, _endCallBack, _t1, _t2, _t3);

                dic.Add(result, unit);

                return result;
            }
        }

        public int NextFrameCall<T1, T2, T3, T4>(Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            lock (dic)
            {
                int result = GetIndex();

                SuperTweenUnit<T1, T2, T3, T4> unit = new SuperTweenUnit<T1, T2, T3, T4>();

                unit.Init(result, 0, 0, 0, null, false, _endCallBack, _t1, _t2, _t3, _t4);

                dic.Add(result, unit);

                return result;
            }
        }

        // Update is called once per frame
        void Update()
        {
            lock (dic)
            {
                if (dic.Count > 0)
                {
                    float nowTime = Time.time;

                    float nowUnscaleTime = Time.unscaledTime;

                    Dictionary<int, SuperTweenUnitBase>.Enumerator enumerator = dic.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        SuperTweenUnitBase unit = enumerator.Current.Value;

                        float tempTime = 0;

                        if (unit.isFixed)
                        {
                            tempTime = nowUnscaleTime;
                        }
                        else
                        {
                            tempTime = nowTime;
                        }

                        if (tempTime > unit.startTime + unit.time)
                        {
                            if (unit.dele != null)
                            {
                                toList.Add(new KeyValuePair<SuperTweenUnitBase, float>(unit, unit.endValue));

                                toDic.Remove(unit.dele);
                            }

                            endList.Add(unit);
                        }
                        else if (unit.dele != null)
                        {
                            float value = unit.startValue + (unit.endValue - unit.startValue) * (tempTime - unit.startTime) / unit.time;

                            toList.Add(new KeyValuePair<SuperTweenUnitBase, float>(unit, value));
                        }
                    }

                    if (toList.Count > 0)
                    {
                        for (int i = 0; i < toList.Count; i++)
                        {
                            KeyValuePair<SuperTweenUnitBase, float> pair = toList[i];

                            pair.Key.dele(pair.Value);
                        }

                        toList.Clear();
                    }

                    if (endList.Count > 0)
                    {
                        for (int i = 0; i < endList.Count; i++)
                        {
                            SuperTweenUnitBase unit = endList[i];

                            dic.Remove(unit.index);

                            if (!unit.isRemoved)
                            {
                                unit.End();
                            }
                        }

                        endList.Clear();
                    }
                }
            }
        }

        private int GetIndex()
        {
            int result = index;

            index++;

            return result;
        }
    }
}