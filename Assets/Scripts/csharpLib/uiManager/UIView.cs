using System;
using UnityEngine;
using System.Collections.Generic;

public class UIView : MonoBehaviour
{
    protected CanvasGroup cg { private set; get; }

    public bool visible { protected set; get; }

    public ValueType data;

    public int layerIndex;

    public int uid;

    public UIView parent;

    public List<UIView> children = new List<UIView>();

    public virtual void Init()
    {
        cg = gameObject.GetComponent<CanvasGroup>();

        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0;

        cg.blocksRaycasts = false;

        visible = false;
    }

    public UIView FindChild(int _uid)
    {
        for (int i = 0; i < children.Count; i++)
        {
            UIView child = children[i];

            if (child.uid == _uid)
            {
                return child;
            }

            UIView result = child.FindChild(_uid);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void SetVisible(bool _visible, ref Action _action)
    {
        if (visible == _visible)
        {
            return;
        }

        visible = _visible;

        if (visible)
        {
            cg.alpha = 1;

            cg.blocksRaycasts = true;

            _action += OnShow;
        }
        else
        {
            cg.alpha = 0;

            cg.blocksRaycasts = false;

            _action += OnHide;
        }
    }

    public virtual bool IsFullScreen()
    {
        throw new NotImplementedException();
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnShow()
    {
    }

    public virtual void OnHide()
    {
    }
}
