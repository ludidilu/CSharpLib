using System;
using UnityEngine;

public class UIView : UIBase
{
    protected CanvasGroup cg { private set; get; }

    public virtual void Init()
    {
        cg = gameObject.GetComponent<CanvasGroup>();

        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public override void SetVisible(bool _visible)
    {
        if (visible == _visible)
        {
            return;
        }

        base.SetVisible(_visible);

        if (visible)
        {
            cg.alpha = 1;

            cg.blocksRaycasts = true;

            OnShow();
        }
        else
        {
            cg.alpha = 0;

            cg.blocksRaycasts = false;

            OnHide();
        }
    }

    public virtual bool IsFullScreen()
    {
        throw new NotImplementedException();
    }

    public virtual void OnShow()
    {
    }

    public virtual void OnHide()
    {
    }
}
