using UnityEngine;
using System.Collections.Generic;
using System;

namespace gameObjectFactory
{
    public class GameObjectFactory
    {
        public Dictionary<string, GameObjectFactoryUnit> dic = new Dictionary<string, GameObjectFactoryUnit>();

        private static GameObjectFactory _Instance;

        public static GameObjectFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObjectFactory();
                }

                return _Instance;
            }
        }

        public void PreloadGameObjects(string[] _paths, Action _callBack)
        {
            Action callBack = null;

            if (_callBack != null)
            {
                int loadNum = _paths.Length;

                callBack = delegate ()
                {
                    loadNum--;

                    if (loadNum == 0)
                    {
                        _callBack();
                    }
                };
            }

            for (int i = 0; i < _paths.Length; i++)
            {
                PreloadGameObject(_paths[i], callBack);
            }
        }

        public void PreloadGameObject(string _path, Action _callBack)
        {
            GameObjectFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                unit = new GameObjectFactoryUnit(_path);

                dic.Add(_path, unit);
            }

            unit.GetGameObject(_callBack);
        }

        public GameObject GetGameObject(string _path, Action<GameObject> _callBack)
        {
            GameObjectFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                unit = new GameObjectFactoryUnit(_path);

                dic.Add(_path, unit);
            }

            return unit.GetGameObject(_callBack);
        }

        public bool Hold(string _path)
        {
            GameObjectFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                return false;
            }
            else
            {
                unit.AddUseNum();

                return true;
            }
        }

        public bool Release(string _path)
        {
            GameObjectFactoryUnit unit;

            if (!dic.TryGetValue(_path, out unit))
            {
                return false;
            }
            else
            {
                unit.DelUseNum();

                return true;
            }
        }

        public void Dispose(bool _force)
        {
            List<string> delKeyList = null;

            IEnumerator<KeyValuePair<string, GameObjectFactoryUnit>> enumerator = dic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                KeyValuePair<string, GameObjectFactoryUnit> pair = enumerator.Current;

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