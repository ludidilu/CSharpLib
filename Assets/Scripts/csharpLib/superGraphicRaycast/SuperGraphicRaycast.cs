using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace superGraphicRaycast
{
    public interface IPointerHitHandler : IEventSystemHandler
    {
        void OnPointerHit(BaseEventData eventData);
    }

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

        private static ExecuteEvents.EventFunction<IPointerHitHandler> hitFun = delegate (IPointerHitHandler _handler, BaseEventData _eventData)
        {
            _handler.OnPointerHit(_eventData);
        };

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            //SuperDebug.Log("Raycast:" + SuperGraphicRaycastScript.Instance.isOpen);

            if (SuperGraphicRaycastScript.Instance.isOpen < 1)
            {
                return;
            }

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

            for (int i = resultAppendList.Count - 1; i > -1; i--)
            {
                bool b = ExecuteEvents.Execute(resultAppendList[i].gameObject, eventData, hitFun);

                if (b)
                {
                    resultAppendList.RemoveAt(i);
                }
            }
        }
    }
}