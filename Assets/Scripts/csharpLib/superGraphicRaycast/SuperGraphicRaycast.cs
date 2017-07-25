using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace superGraphicRaycast
{
    public class SuperGraphicRaycast : GraphicRaycaster
    {
        public static void SetIsOpen(bool _isOpen, string _str)
        {
            SuperGraphicRaycastScript.Instance.isOpen = SuperGraphicRaycastScript.Instance.isOpen + (_isOpen ? 1 : -1);

            if (SuperGraphicRaycastScript.Instance.isOpen > 1)
            {
                SuperDebug.LogError("SuperGraphicRaycast.SetOpen error!");
            }
        }

        public static void SetFilter(bool _value)
        {
            SuperGraphicRaycastScript.Instance.filter = _value;
        }

        public static void AddFilterTag(string _tag)
        {
            SuperGraphicRaycastScript.Instance.tagDic.Add(_tag, true);
        }

        public static void RemoveFilterTag(string _tag)
        {
            SuperGraphicRaycastScript.Instance.tagDic.Remove(_tag);
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

            if (SuperGraphicRaycastScript.Instance.filter)
            {
                for (int i = resultAppendList.Count - 1; i > -1; i--)
                {
                    if (!SuperGraphicRaycastScript.Instance.tagDic.ContainsKey(resultAppendList[i].gameObject.tag))
                    {
                        resultAppendList.RemoveAt(i);
                    }
                }
            }
        }
    }
}