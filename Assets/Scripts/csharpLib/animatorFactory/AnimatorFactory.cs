using UnityEngine;
using System.Collections.Generic;
using System;

namespace animatorFactoty
{
    public class AnimatorFactory
    {
        private static AnimatorFactory _Instance;

        public static AnimatorFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AnimatorFactory();
                }

                return _Instance;
            }
        }

        public Dictionary<string, AnimatorFactoryUnit> dic;

        public AnimatorFactory()
        {
            dic = new Dictionary<string, AnimatorFactoryUnit>();
        }

        public RuntimeAnimatorController GetAnimator(string _path, Action<RuntimeAnimatorController> _callBack)
        {
            AnimatorFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                unit = new AnimatorFactoryUnit(_path);

                dic.Add(_path, unit);
            }

            return unit.GetAnimator(_callBack);
        }

        public void AddUseNum(string _path)
        {
            AnimatorFactoryUnit unit;

            if (dic.TryGetValue(_path, out unit))
            {
                unit.AddUseNum();
            }
        }

        public void DelUseNum(string _path)
        {
            AnimatorFactoryUnit unit;

            if (dic.TryGetValue(_path, out unit))
            {
                unit.DelUseNum();
            }
        }

        public void Dispose(bool _force)
        {
            List<string> delKeyList = null;

            IEnumerator<KeyValuePair<string, AnimatorFactoryUnit>> enumerator = dic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                KeyValuePair<string, AnimatorFactoryUnit> pair = enumerator.Current;

                if (_force || pair.Value.useNum == 0)
                {
                    pair.Value.Dispose();

                    if (delKeyList == null)
                    {
                        delKeyList = new List<string>();
                    }

                    delKeyList.Add(pair.Key);
                }
            }

            if (delKeyList != null)
            {
                for (int i = 0; i < delKeyList.Count; i++)
                {
                    dic.Remove(delKeyList[i]);
                }
            }
        }
    }
}