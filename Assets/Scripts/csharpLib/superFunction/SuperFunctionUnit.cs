using UnityEngine;
using System;

namespace superFunction
{
    public struct SuperFunctionUnit
    {
        public Delegate callBack;
        public GameObject target;
        public string eventName;
        public int index;
        public bool isOnce;

        public SuperFunctionUnit(GameObject _target, string _eventName, Delegate _callBack, int _index, bool _isOnce)
        {
            target = _target;
            eventName = _eventName;
            callBack = _callBack;
            index = _index;
            isOnce = _isOnce;
        }
    }
}