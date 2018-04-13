using UnityEngine;
using superFunction;
using System.Collections.Generic;
using System;
using superGraphicRaycast;

namespace superRaycast
{
    public class SuperRaycast : MonoBehaviour
    {
        private struct MouseEnterUnit
        {
            public GameObject gameObject;
            public RaycastHit hit;
            public int index;

            public MouseEnterUnit(GameObject _gameObject, RaycastHit _hit, int _index)
            {
                gameObject = _gameObject;
                hit = _hit;
                index = _index;
            }
        }

        public const string GetMouseButtonDown = "GetMouseButtonDown";
        public const string GetMouseButton = "GetMouseButton";
        public const string GetMouseButtonUp = "GetMouseButtonUp";
        public const string GetMouseEnter = "GetMouseEnter";
        public const string GetMouseExit = "GetMouseExit";
        public const string GetMouseClick = "GetMouseClick";

        private static SuperRaycast _Instance;

        public static void SetCamera(Camera _camera)
        {
            Instance.renderCamera = _camera;
        }

        public static void AddLayer(string _layerName)
        {
            Instance.AddLayerReal(_layerName);
        }

        public static void RemoveLayer(string _layerName)
        {
            Instance.RemoveLayerReal(_layerName);
        }

        public static void AddFilterTag(string _tag)
        {
            Instance.AddFilterTagReal(_tag);
        }

        public static void RemoveFilterTag(string _tag)
        {
            Instance.RemoveFilterTagReal(_tag);
        }

        public static void SetFilter(bool _value)
        {
            Instance.filter = _value;
        }

        public static bool GetFilter()
        {
            return Instance.filter;
        }

        public static GameObject Go
        {
            get
            {
                return Instance.gameObject;
            }
        }

        private static SuperRaycast Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject go = new GameObject("SuperRaycastGameObject");

                    _Instance = go.AddComponent<SuperRaycast>();
                }

                return _Instance;
            }
        }

        public static bool GetIsOpen()
        {
            return Instance.isOpen > 0;
        }

        public static void SetIsOpen(bool _isOpen, string _str)
        {
            Instance.isOpen = Instance.isOpen + (_isOpen ? 1 : -1);

            if (Instance.isOpen == 0)
            {
                if (!Instance.isProcessingUpdate)
                {
                    Instance.objs.Clear();
                }
                else
                {
                    Instance.needClearObjs = true;
                }
            }
            else if (Instance.isOpen > 1)
            {
                SuperDebug.Log("SuperRaycast error!!!!!!!!!!!!!");
            }
        }

        private int layerIndex;

        private int isOpen = 0;

        private bool filter = false;

        private Dictionary<string, bool> filterTagDic = new Dictionary<string, bool>();

        private List<GameObject> downObjs = new List<GameObject>();

        private List<GameObject> newObjs = new List<GameObject>();

        private List<GameObject> objs = new List<GameObject>();

        private List<MouseEnterUnit> mouseEnterList = new List<MouseEnterUnit>();

        private bool isProcessingUpdate = false;

        private bool needClearObjs = false;

        private Camera renderCamera;

        private void AddLayerReal(string _layerName)
        {
            layerIndex = layerIndex | (1 << LayerMask.NameToLayer(_layerName));
        }

        private void RemoveLayerReal(string _layerName)
        {
            layerIndex = layerIndex & ~(1 << LayerMask.NameToLayer(_layerName));
        }

        private void AddFilterTagReal(string _tag)
        {
            if (!filterTagDic.ContainsKey(_tag))
            {
                filterTagDic.Add(_tag, false);
            }
        }

        private void RemoveFilterTagReal(string _tag)
        {
            filterTagDic.Remove(_tag);
        }

        void Update()
        {
            if (isOpen > 0 && renderCamera != null)
            {
                isProcessingUpdate = true;

                RaycastHit[] hits = null;

                bool blockByUI = false;

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);

                    blockByUI = IsPointerOverUIObject();

                    if (layerIndex == 0)
                    {
                        hits = Physics.RaycastAll(ray, float.MaxValue);
                    }
                    else
                    {
                        hits = Physics.RaycastAll(ray, float.MaxValue, layerIndex);
                    }

                    Array.Sort(hits, SortHits);

                    int i = 0;

                    for (int m = 0; m < hits.Length; m++)
                    {
                        RaycastHit hit = hits[m];

                        if (filter && !filterTagDic.ContainsKey(hit.collider.gameObject.tag))
                        {
                            continue;
                        }

                        objs.Add(hit.collider.gameObject);

                        downObjs.Add(hit.collider.gameObject);

                        SuperFunction.Instance.DispatchEvent(hit.collider.gameObject, GetMouseButtonDown, blockByUI, hit, i);

                        i++;
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (hits == null)
                    {
                        Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);

                        blockByUI = IsPointerOverUIObject();

                        if (layerIndex == 0)
                        {
                            hits = Physics.RaycastAll(ray, float.MaxValue);
                        }
                        else
                        {
                            hits = Physics.RaycastAll(ray, float.MaxValue, layerIndex);
                        }

                        Array.Sort(hits, SortHits);
                    }

                    int i = 0;

                    for (int m = 0; m < hits.Length; m++)
                    {
                        RaycastHit hit = hits[m];

                        if (filter && !filterTagDic.ContainsKey(hit.collider.gameObject.tag))
                        {
                            continue;
                        }

                        newObjs.Add(hit.collider.gameObject);

                        if (!objs.Contains(hit.collider.gameObject))
                        {
                            mouseEnterList.Add(new MouseEnterUnit(hit.collider.gameObject, hit, i));

                            //SuperFunction.Instance.DispatchEvent(hit.collider.gameObject, GetMouseEnter, blockByUI, hit, i);
                        }
                        else
                        {
                            objs.Remove(hit.collider.gameObject);
                        }

                        SuperFunction.Instance.DispatchEvent(hit.collider.gameObject, GetMouseButton, blockByUI, hit, i);

                        i++;
                    }

                    for (i = 0; i < objs.Count; i++)
                    {
                        SuperFunction.Instance.DispatchEvent(objs[i], GetMouseExit, blockByUI);
                    }

                    for (i = 0; i < mouseEnterList.Count; i++)
                    {
                        MouseEnterUnit unit = mouseEnterList[i];

                        SuperFunction.Instance.DispatchEvent(unit.gameObject, GetMouseEnter, blockByUI, unit.hit, unit.index);
                    }

                    mouseEnterList.Clear();

                    List<GameObject> tmpObjs = objs;

                    objs = newObjs;

                    newObjs = tmpObjs;

                    newObjs.Clear();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (hits == null)
                    {
                        Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);

                        blockByUI = IsPointerOverUIObject();

                        if (layerIndex == 0)
                        {
                            hits = Physics.RaycastAll(ray, float.MaxValue);
                        }
                        else
                        {
                            hits = Physics.RaycastAll(ray, float.MaxValue, layerIndex);
                        }

                        Array.Sort(hits, SortHits);
                    }

                    int i = 0;

                    for (int m = 0; m < hits.Length; m++)
                    {
                        RaycastHit hit = hits[m];

                        if (filter && !filterTagDic.ContainsKey(hit.collider.gameObject.tag))
                        {
                            continue;
                        }

                        SuperFunction.Instance.DispatchEvent(hit.collider.gameObject, GetMouseButtonUp, blockByUI, hit, i);

                        if (downObjs.Contains(hit.collider.gameObject))
                        {
                            SuperFunction.Instance.DispatchEvent(hit.collider.gameObject, GetMouseClick, blockByUI, hit, i);
                        }

                        i++;
                    }

                    downObjs.Clear();

                    objs.Clear();
                }

                if (needClearObjs)
                {
                    needClearObjs = false;

                    objs.Clear();

                    downObjs.Clear();
                }

                isProcessingUpdate = false;
            }
        }

        private int SortHits(RaycastHit _hit0, RaycastHit _hit1)
        {
            if (_hit0.distance > _hit1.distance)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private bool IsPointerOverUIObject()
        {
            return SuperGraphicRaycast.resultAppendListCount > 0;
        }
    }
}
