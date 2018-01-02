using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public object data { protected set; get; }

    public UIBase parent;

    public List<UIBase> children = new List<UIBase>();
}
