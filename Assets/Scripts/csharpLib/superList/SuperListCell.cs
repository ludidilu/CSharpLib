using UnityEngine;
using UnityEngine.EventSystems;
using superTween;
using System;

namespace superList
{
    public class SuperListCell : MonoBehaviour, IPointerClickHandler
    {
        protected object data;
        protected bool selected;

        [HideInInspector]
        public CanvasGroup canvasGroup;

        [HideInInspector]
        public int index;

        protected SuperList superList;

        public virtual void Init(SuperList _superList, object[] _objs)
        {
            superList = _superList;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (data != null)
            {
                superList.CellClick(this);
            }
        }

        public virtual bool SetData(object _data)
        {
            data = _data;

            return false;
        }

        public virtual void SetSelected(bool _value)
        {
            selected = _value;

            //由于Rect2DMask有bug  所以添加这2行2B代码
        }

        private int tweenID;

        public void AlphaIn(Action _callBack)
        {
            Action dele = delegate ()
            {
                tweenID = -1;

                _callBack();
            };

            tweenID = SuperTween.Instance.To(0, 1, 0.1f, AlphaInDel, dele);
        }

        public void StopAlphaIn()
        {
            if (tweenID != -1)
            {
                SuperTween.Instance.Remove(tweenID);
            }
        }

        private void AlphaInDel(float _value)
        {
            canvasGroup.alpha = _value;
        }
    }
}
