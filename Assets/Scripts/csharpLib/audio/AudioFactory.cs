using UnityEngine;
using System.Collections.Generic;
using System;

namespace audio
{
    public class AudioFactory
    {
        private static AudioFactory _Instance;

        public static AudioFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AudioFactory();
                }

                return _Instance;
            }
        }

        private Dictionary<string, AudioFactoryUnit> dic = new Dictionary<string, AudioFactoryUnit>();

        public AudioClip GetClip(string _name, Action<AudioClip> _callBack)
        {
            return GetClip(_name, _callBack, true);
        }

        public AudioClip GetClip(string _name, Action<AudioClip> _callBack, bool _willDispose)
        {
            AudioFactoryUnit unit;

            if (!dic.TryGetValue(_name, out unit))
            {
                unit = new AudioFactoryUnit(_name);

                dic.Add(_name, unit);
            }

            return unit.GetClip(_callBack, _willDispose);
        }

        public void RemoveClip(AudioClip _clip)
        {
            string key = _clip.name;

            AudioFactoryUnit unit;

            if (dic.TryGetValue(key, out unit))
            {
                unit.Dispose();

                dic.Remove(key);
            }
        }

        public void Dispose(bool _force)
        {
            List<string> delKeyList = null;

            Dictionary<string, AudioFactoryUnit>.Enumerator enumerator = dic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                KeyValuePair<string, AudioFactoryUnit> pair = enumerator.Current;

                AudioFactoryUnit unit = pair.Value;

                if (_force || unit.willDispose)
                {
                    unit.Dispose();

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