using UnityEngine;
using System.Collections.Generic;
using System;
using assetManager;

namespace materialFactory
{
    public class MaterialFactoryUnit
    {
        private string name;
        private Material data;

        private int type = -1;

        private List<Action<Material>> callBackList = new List<Action<Material>>();

        public int useNum;

        public MaterialFactoryUnit(string _name)
        {
            name = _name;
        }

        public Material GetMaterial(Action<Material> _callBack)
        {
            if (type == -1)
            {
                type = 0;

                callBackList.Add(_callBack);

                AssetManager.Instance.GetAsset<Material>(name, GetAsset);

                return null;
            }
            else if (type == 0)
            {
                callBackList.Add(_callBack);

                return null;
            }
            else
            {
                if (_callBack != null)
                {
                    _callBack(data);
                }

                return data;
            }
        }

        private void GetAsset(Material _data)
        {
            data = _data;

            type = 1;

            for (int i = 0; i < callBackList.Count; i++)
            {
                callBackList[i](data);
            }

            callBackList.Clear();
        }

        public void DelUseNum()
        {
            useNum--;
        }

        public void AddUseNum()
        {
            useNum++;
        }

        public void Dispose()
        {
            Resources.UnloadAsset(data);
        }
    }
}