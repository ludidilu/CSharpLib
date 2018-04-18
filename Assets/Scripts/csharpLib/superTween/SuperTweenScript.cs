using UnityEngine;
using System.Collections.Generic;
using System;

namespace superTween
{
    public class SuperTweenScript : MonoBehaviour
    {
        private Dictionary<int, SuperTweenUnit> dic = new Dictionary<int, SuperTweenUnit>();

        private Dictionary<Action<float>, SuperTweenUnit> toDic = new Dictionary<Action<float>, SuperTweenUnit>();

        private int index;

        private List<int> endList = new List<int>();

        private Dictionary<int, float> toList = new Dictionary<int, float>();

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, Action _endCallBack, bool isFixed)
        {
            SuperTweenUnit unit;

            int result = GetIndex();

            if (toDic.TryGetValue(_delegate, out unit))
            {
                dic.Remove(unit.index);
            }
            else
            {
                unit = GetUnit();

                toDic.Add(_delegate, unit);
            }

            unit.Init(result, _startValue, _endValue, _time, _delegate, _endCallBack, isFixed);

            dic.Add(result, unit);

            return result;
        }

        public void SetTag(int _index, string _tag)
        {
            SuperTweenUnit unit;

            if (dic.TryGetValue(_index, out unit))
            {
                unit.tag = _tag;
            }
        }

        public void Remove(int _index, bool _toEnd)
        {
            SuperTweenUnit unit;

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

                    if (unit.endCallBack != null)
                    {
                        unit.endCallBack();
                    }
                }

                ReleaseUnit(unit);
            }
        }

        public void RemoveAll(bool _toEnd)
        {
            Dictionary<int, SuperTweenUnit> tmpDic = dic;

            dic = new Dictionary<int, SuperTweenUnit>();

            toDic = new Dictionary<Action<float>, SuperTweenUnit>();

            IEnumerator<SuperTweenUnit> enumerator = tmpDic.Values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                SuperTweenUnit unit = enumerator.Current;

                if (_toEnd)
                {
                    if (unit.dele != null)
                    {
                        unit.dele(unit.endValue);
                    }

                    if (unit.endCallBack != null)
                    {
                        unit.endCallBack();
                    }
                }

                ReleaseUnit(unit);
            }
        }

        public void RemoveWithTag(string _tag, bool _toEnd)
        {
            List<SuperTweenUnit> list = new List<SuperTweenUnit>();

            IEnumerator<SuperTweenUnit> enumerator = dic.Values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                SuperTweenUnit unit = enumerator.Current;

                if (unit.tag == _tag)
                {
                    list.Add(unit);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                SuperTweenUnit unit = list[i];

                dic.Remove(unit.index);

                if (unit.dele != null)
                {
                    toDic.Remove(unit.dele);
                }

                if (!_toEnd)
                {
                    ReleaseUnit(unit);
                }
            }

            if (_toEnd)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SuperTweenUnit unit = list[i];

                    if (unit.dele != null)
                    {
                        unit.dele(unit.endValue);
                    }

                    if (unit.endCallBack != null)
                    {
                        unit.endCallBack();
                    }

                    ReleaseUnit(unit);
                }
            }
        }

        public int DelayCall(float _time, Action _endCallBack, bool isFixed)
        {
            int result = GetIndex();

            SuperTweenUnit unit = GetUnit();

            unit.Init(result, 0, 0, _time, null, _endCallBack, isFixed);

            dic.Add(result, unit);

            return result;
        }

        public int NextFrameCall(Action _endCallBack)
        {
            int result = GetIndex();

            SuperTweenUnit unit = GetUnit();

            unit.Init(result, 0, 0, 0, null, _endCallBack, false);

            dic.Add(result, unit);

            return result;
        }

        // Update is called once per frame
        void Update()
        {
            if (dic.Count > 0)
            {
                float nowTime = Time.time;

                float nowUnscaleTime = Time.unscaledTime;

                IEnumerator<SuperTweenUnit> enumerator = dic.Values.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    SuperTweenUnit unit = enumerator.Current;

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
                            toList.Add(unit.index, unit.endValue);
                        }

                        endList.Add(unit.index);
                    }
                    else if (unit.dele != null)
                    {
                        float value = unit.startValue + (unit.endValue - unit.startValue) * (tempTime - unit.startTime) / unit.time;

                        toList.Add(unit.index, value);
                    }
                }

                if (toList.Count > 0)
                {
                    IEnumerator<KeyValuePair<int, float>> enumerator2 = toList.GetEnumerator();

                    while (enumerator2.MoveNext())
                    {
                        int index = enumerator2.Current.Key;

                        SuperTweenUnit unit;

                        if (dic.TryGetValue(index, out unit))
                        {
                            unit.dele(enumerator2.Current.Value);
                        }
                    }

                    toList.Clear();
                }

                if (endList.Count > 0)
                {
                    for (int i = 0; i < endList.Count; i++)
                    {
                        int index = endList[i];

                        SuperTweenUnit unit;

                        if (dic.TryGetValue(index, out unit))
                        {
                            dic.Remove(index);

                            if (unit.dele != null)
                            {
                                toDic.Remove(unit.dele);
                            }

                            if (unit.endCallBack != null)
                            {
                                unit.endCallBack();
                            }

                            ReleaseUnit(unit);
                        }
                    }

                    endList.Clear();
                }
            }
        }

        private int GetIndex()
        {
            int result = index;

            index++;

            return result;
        }

        private Queue<SuperTweenUnit> pool = new Queue<SuperTweenUnit>();

        private SuperTweenUnit GetUnit()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                return new SuperTweenUnit();
            }
        }

        private void ReleaseUnit(SuperTweenUnit _unit)
        {
            pool.Enqueue(_unit);
        }
    }
}