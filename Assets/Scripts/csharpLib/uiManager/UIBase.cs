using System;
using UnityEngine;

public class UIBase : MonoBehaviour
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

    public virtual void OnEnter<U>(U _data)
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
