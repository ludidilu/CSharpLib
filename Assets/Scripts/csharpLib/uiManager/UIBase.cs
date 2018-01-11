using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBase : MonoBehaviour
{
    public ValueType data;

    public int layerIndex;

    public int uid;

    public UIBase parent;

    public List<UIBase> children = new List<UIBase>();

    public UIBase FindChild(int _uid)
    {
        for (int i = 0; i < children.Count; i++)
        {
            UIBase child = children[i];

            if (child.uid == _uid)
            {
                return child;
            }

            UIBase result = child.FindChild(_uid);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
