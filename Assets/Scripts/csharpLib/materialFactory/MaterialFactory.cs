using UnityEngine;
using System.Collections.Generic;
using System;

namespace materialFactory
{
    public class MaterialFactory
    {
        public Dictionary<string, MaterialFactoryUnit> dic = new Dictionary<string, MaterialFactoryUnit>();

        private static MaterialFactory _Instance;

        public static MaterialFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MaterialFactory();
                }

                return _Instance;
            }
        }

        public Material GetMaterial(string _path, Action<Material> _callBack)
        {
            MaterialFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                unit = new MaterialFactoryUnit(_path);

                dic.Add(_path, unit);
            }

            return unit.GetMaterial(_callBack);
        }

        public void AddUseNum(string _path)
        {
            MaterialFactoryUnit unit;

            if (dic.TryGetValue(_path, out unit))
            {
                unit.AddUseNum();
            }
        }

        public void DelUseNum(string _path)
        {
            MaterialFactoryUnit unit;

            if (dic.TryGetValue(_path, out unit))
            {
                unit.DelUseNum();
            }
        }

        public void Dispose(bool _force)
        {
            List<string> delKeyList = null;

            IEnumerator<KeyValuePair<string, MaterialFactoryUnit>> enumerator = dic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                KeyValuePair<string, MaterialFactoryUnit> pair = enumerator.Current;

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