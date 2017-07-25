using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using superRaycast;
using superFunction;

namespace superGraphicRaycast
{
    public class SuperGraphicRaycast : GraphicRaycaster
    {
        public static Dictionary<string, int> dic = new Dictionary<string, int>();

        /// <summary>
        /// _str 传 类名-锁屏/解锁功能名
        /// </summary>
        /// <param name="_isOpen"></param>
        /// <param name="_str"></param>
		public static void SetIsOpen(bool _isOpen, string _str)
        {
            //SuperDebug.Log("-->> _isOpen: " + _isOpen + " , _str: " + _str);
            SuperGraphicRaycastScript.Instance.isOpen = SuperGraphicRaycastScript.Instance.isOpen + (_isOpen ? 1 : -1);

            if (dic.ContainsKey(_str))
            {
                if (_isOpen)
                {
                    dic[_str]++;
                }
                else
                {
                    dic[_str]--;
                }

                if (dic[_str] == 0)
                {
                    dic.Remove(_str);
                }
            }
            else
            {
                if (_isOpen)
                {
                    dic.Add(_str, 1);
                }
                else
                {
                    dic.Add(_str, -1);
                }
            }

            if (SuperGraphicRaycastScript.Instance.isOpen > 1)
            {
                PrintLog();
                SuperDebug.LogError("SuperGraphicRaycast.SetOpen error!");
            }
        }

        public static void PrintLog()
        {
            if (SuperGraphicRaycastScript.Instance.isOpen != 1)
            {
                foreach (KeyValuePair<string, int> pair in dic)
                {
                    SuperDebug.Log("SuperGraphicRaycast key:" + pair.Key + "  value:" + pair.Value);
                }
            }
        }

        public static bool filter
        {
            set
            {
                SuperGraphicRaycastScript.Instance.filter = value;
            }

            get
            {
                return SuperGraphicRaycastScript.Instance.filter;
            }
        }

        public static string filterTag
        {
            set
            {
                SuperGraphicRaycastScript.Instance.filterTag = value;
            }

            get
            {
                return SuperGraphicRaycastScript.Instance.filterTag;
            }
        }

        private int touchCount = 0;

        void LateUpdate()
        {
            if (touchCount != 0)
            {
                touchCount = 0;
            }
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            //SuperDebug.Log("Raycast:" + SuperGraphicRaycastScript.Instance.isOpen);

            if (SuperGraphicRaycastScript.Instance.isOpen < 1)
            {
                return;
            }

            if (touchCount > 0)
            {
                return;
            }

            touchCount++;

            //			if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)){

            base.Raycast(eventData, resultAppendList);

            if (filter)
            {
                for (int i = resultAppendList.Count - 1; i > -1; i--)
                {
                    if (!resultAppendList[i].gameObject.CompareTag(filterTag))
                    {

                        resultAppendList.RemoveAt(i);
                    }
                }
            }
        }
    }
}