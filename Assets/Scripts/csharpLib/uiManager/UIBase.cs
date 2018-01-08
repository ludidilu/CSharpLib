using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public object data;

    public int uid;

    public UIBase parent;

    public List<UIBase> children = new List<UIBase>();

    public bool visible { protected set; get; }

    public virtual void SetVisible(bool _visible)
    {
        visible = _visible;
    }
}
