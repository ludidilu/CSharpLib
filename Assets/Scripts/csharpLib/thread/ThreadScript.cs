using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

namespace thread
{
    public class ThreadScript : MonoBehaviour
    {
        private static ThreadScript _Instance;

        public static ThreadScript Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject go = new GameObject("ThreadScriptGameObject");

                    GameObject.DontDestroyOnLoad(go);

                    _Instance = go.AddComponent<ThreadScript>();
                }

                return _Instance;
            }
        }

        private List<Action> callBackList = new List<Action>();

        private List<Thread> checkList = new List<Thread>();

        private List<Action> tmpCallBackList = new List<Action>();

        public void Add(ParameterizedThreadStart _job, object _data, Action _callBack)
        {
            callBackList.Add(_callBack);

            Thread thread = new Thread(_job);

            checkList.Add(thread);

            thread.Start(_data);
        }

        public void Add(ThreadStart _job, Action _callBack, Func<bool> _check)
        {
            callBackList.Add(_callBack);

            Thread thread = new Thread(_job);

            checkList.Add(thread);

            thread.Start();
        }

        void Update()
        {
            if (callBackList.Count > 0)
            {
                for (int i = callBackList.Count - 1; i > -1; i--)
                {
                    if (!checkList[i].IsAlive)
                    {
                        tmpCallBackList.Add(callBackList[i]);

                        callBackList.RemoveAt(i);

                        checkList.RemoveAt(i);
                    }
                }

                if (tmpCallBackList.Count > 0)
                {
                    for (int i = 0; i < tmpCallBackList.Count; i++)
                    {
                        Action tmpCb = tmpCallBackList[i];

                        if (tmpCb != null)
                        {
                            tmpCb();
                        }
                    }

                    tmpCallBackList.Clear();
                }
            }
        }
    }
}