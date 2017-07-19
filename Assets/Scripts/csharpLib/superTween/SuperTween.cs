using UnityEngine;
using System;

namespace superTween
{
    public class SuperTween
    {
        private static SuperTween _Instance;

        public static SuperTween Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SuperTween();
                }

                return _Instance;
            }
        }

        private GameObject go;

        private SuperTweenScript script;

        public SuperTween()
        {
            go = new GameObject("SuperTweenGameObject");

            GameObject.DontDestroyOnLoad(go);

            script = go.AddComponent<SuperTweenScript>();
        }

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, Action _endCallBack)
        {
            if (_endCallBack != null)
            {
                return To(_startValue, _endValue, _time, _delegate, false, _endCallBack);
            }
            else
            {
                return To(_startValue, _endValue, _time, _delegate, false);
            }
        }

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed);
        }

        public int To(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action _endCallBack)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed, _endCallBack);
        }

        public int To<T1>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1> _endCallBack, T1 _t1)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1);
        }

        public int To<T1, T2>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2);
        }

        public int To<T1, T2, T3>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3);
        }

        public int To<T1, T2, T3, T4>(float _startValue, float _endValue, float _time, Action<float> _delegate, bool isFixed, Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            return script.To(_startValue, _endValue, _time, _delegate, isFixed, _endCallBack, _t1, _t2, _t3, _t4);
        }

        public void Remove(int _index)
        {
            script.Remove(_index, false);
        }

        public void Remove(int _index, bool _toEnd)
        {
            script.Remove(_index, _toEnd);
        }

        public void SetTag(int _index, string _tag)
        {
            script.SetTag(_index, _tag);
        }

        public void RemoveAll(bool _toEnd)
        {
            script.RemoveAll(_toEnd);
        }

        public void RemoveWithTag(string _tag, bool _toEnd)
        {
            script.RemoveWithTag(_tag, _toEnd);
        }

        public int DelayCall(float _time, Action _endCallBack)
        {
            return DelayCall(_time, false, _endCallBack);
        }

        public int DelayCall(float _time, bool isFixed, Action _endCallBack)
        {
            return script.DelayCall(_time, isFixed, _endCallBack);
        }

        public int DelayCall<T1>(float _time, bool isFixed, Action<T1> _endCallBack, T1 _t1)
        {
            return script.DelayCall(_time, isFixed, _endCallBack, _t1);
        }

        public int DelayCall<T1, T2>(float _time, bool isFixed, Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            return script.DelayCall(_time, isFixed, _endCallBack, _t1, _t2);
        }

        public int DelayCall<T1, T2, T3>(float _time, bool isFixed, Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            return script.DelayCall(_time, isFixed, _endCallBack, _t1, _t2, _t3);
        }

        public int DelayCall<T1, T2, T3, T4>(float _time, bool isFixed, Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            return script.DelayCall(_time, isFixed, _endCallBack, _t1, _t2, _t3, _t4);
        }

        public int NextFrameCall(Action _endCallBack)
        {
            return script.NextFrameCall(_endCallBack);
        }

        public int NextFrameCall<T1>(Action<T1> _endCallBack, T1 _t1)
        {
            return script.NextFrameCall(_endCallBack, _t1);
        }

        public int NextFrameCall<T1, T2>(Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            return script.NextFrameCall(_endCallBack, _t1, _t2);
        }

        public int NextFrameCall<T1, T2, T3>(Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            return script.NextFrameCall(_endCallBack, _t1, _t2, _t3);
        }

        public int NextFrameCall<T1, T2, T3, T4>(Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            return script.NextFrameCall(_endCallBack, _t1, _t2, _t3, _t4);
        }
    }
}
