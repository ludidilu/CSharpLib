using UnityEngine;
using System;

namespace superTween
{
    public struct SuperTweenUnit
    {
        public int index;

        public string tag;

        public float startValue;
        public float endValue;
        public float time;
        public float startTime;

        public Action endCallBack;

        public Action<float> dele;

        public bool isFixed;

        public bool isRemoved;

        public void Init(int _index, float _startValue, float _endValue, float _time, Action<float> _delegate, Action _endCallBack, bool _isFixed)
        {
            index = _index;

            isFixed = _isFixed;

            startValue = _startValue;
            endValue = _endValue;
            time = _time;
            dele = _delegate;

            endCallBack = _endCallBack;

            isRemoved = false;

            if (isFixed)
            {
                startTime = Time.unscaledTime;
            }
            else
            {
                startTime = Time.time;
            }
        }
    }
}