using UnityEngine;
using System.Collections.Generic;

namespace superGraphicRaycast
{
    public class SuperGraphicRaycastScript : MonoBehaviour
    {
        private static SuperGraphicRaycastScript _Instance;

        public static SuperGraphicRaycastScript Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject go = new GameObject("SuperGraphicRaycastScriptGameObject");

                    _Instance = go.AddComponent<SuperGraphicRaycastScript>();
                }

                return _Instance;
            }
        }

        public int isOpen = 1;

        public bool filter = false;

        public Dictionary<string, bool> tagDic = new Dictionary<string, bool>();
    }
}