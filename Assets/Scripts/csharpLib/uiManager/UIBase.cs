using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public object data;

    public int layerIndex;

    public int uid;

    public UIBase parent;

    public List<UIBase> children = new List<UIBase>();

    public UIBase root
    {
        get
        {
            if (parent != null)
            {
                UIBase ui = parent;

                while (true)
                {
                    if (ui.parent != null)
                    {
                        ui = ui.parent;
                    }
                    else
                    {
                        return ui;
                    }
                }
            }
            else
            {
                return this;
            }
        }
    }

    public List<UIBase> chain
    {
        get
        {
            List<UIBase> result = new List<UIBase>();

            result.Add(this);

            if (parent != null)
            {
                UIBase ui = parent;

                while (true)
                {
                    result.Add(ui);

                    if (ui.parent != null)
                    {
                        ui = ui.parent;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
