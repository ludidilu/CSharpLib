using System;
using UnityEngine;

public class UIView : UIBase
{
    public CanvasGroup cg { private set; get; }

    public virtual void Init()
    {
        cg = gameObject.GetComponent<CanvasGroup>();

        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public virtual bool IsFullScreen()
    {
        throw new NotImplementedException();
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnEnter(object _data)
    {
        data = _data;
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
