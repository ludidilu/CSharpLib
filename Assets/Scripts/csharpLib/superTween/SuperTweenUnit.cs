using UnityEngine;
using System;

namespace superTween
{
    public class SuperTweenUnitBase
    {
        public int index;

        public string tag;

        public float startValue;
        public float endValue;
        public float time;
        public float startTime;

        public Action<float> dele;

        public bool isFixed;

        public bool isRemoved = false;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed)
        {
            index = _index;

            isFixed = _isFixed;

            startValue = _startValue;
            endValue = _endValue;
            time = _time;
            dele = _delegate;

            if (isFixed)
            {
                startTime = Time.unscaledTime;
            }
            else
            {
                startTime = Time.time;
            }
        }

        public virtual void End()
        {

        }
    }

    public class SuperTweenUnit : SuperTweenUnitBase
    {
        private Action endCallBack;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed, Action _endCallBack)
        {
            base.Init(_index, _startValue, _endValue, _time, _delegate, _isFixed);

            endCallBack = _endCallBack;
        }

        public override void End()
        {
            endCallBack();
        }
    }

    public class SuperTweenUnit<T1> : SuperTweenUnitBase
    {
        private Action<T1> endCallBack;

        private T1 t1;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed, Action<T1> _endCallBack, T1 _t1)
        {
            base.Init(_index, _startValue, _endValue, _time, _delegate, _isFixed);

            endCallBack = _endCallBack;

            t1 = _t1;
        }

        public override void End()
        {
            endCallBack(t1);
        }
    }

    public class SuperTweenUnit<T1, T2> : SuperTweenUnitBase
    {
        private Action<T1, T2> endCallBack;

        private T1 t1;

        private T2 t2;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed, Action<T1, T2> _endCallBack, T1 _t1, T2 _t2)
        {
            base.Init(_index, _startValue, _endValue, _time, _delegate, _isFixed);

            endCallBack = _endCallBack;

            t1 = _t1;

            t2 = _t2;
        }

        public override void End()
        {
            endCallBack(t1, t2);
        }
    }

    public class SuperTweenUnit<T1, T2, T3> : SuperTweenUnitBase
    {
        private Action<T1, T2, T3> endCallBack;

        private T1 t1;

        private T2 t2;

        private T3 t3;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed, Action<T1, T2, T3> _endCallBack, T1 _t1, T2 _t2, T3 _t3)
        {
            base.Init(_index, _startValue, _endValue, _time, _delegate, _isFixed);

            endCallBack = _endCallBack;

            t1 = _t1;

            t2 = _t2;

            t3 = _t3;
        }

        public override void End()
        {
            endCallBack(t1, t2, t3);
        }
    }

    public class SuperTweenUnit<T1, T2, T3, T4> : SuperTweenUnitBase
    {
        private Action<T1, T2, T3, T4> endCallBack;

        private T1 t1;

        private T2 t2;

        private T3 t3;

        private T4 t4;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, bool _isFixed, Action<T1, T2, T3, T4> _endCallBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            base.Init(_index, _startValue, _endValue, _time, _delegate, _isFixed);

            endCallBack = _endCallBack;

            t1 = _t1;

            t2 = _t2;

            t3 = _t3;

            t4 = _t4;
        }

        public override void End()
        {
            endCallBack(t1, t2, t3, t4);
        }
    }
}