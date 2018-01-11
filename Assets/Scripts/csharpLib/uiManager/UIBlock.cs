using System.Collections.Generic;

public class UIBlock : UIBase
{
    public UIView origin;

    public void Replace(LinkedList<List<UIBase>> _stack)
    {
        int index;

        if (origin.gameObject.activeSelf)
        {
            origin.SetVisible(false);

            if (origin.parent == null)
            {
                if (parent == null)
                {
                    List<UIBase> list0 = null;

                    int index0 = -1;

                    List<UIBase> list1 = null;

                    int index1 = -1;

                    IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        List<UIBase> list = enumerator.Current;

                        int tmpLayerIndex2 = list[0].layerIndex;

                        if (tmpLayerIndex2 == origin.layerIndex)
                        {
                            list0 = list;

                            index0 = list.IndexOf(origin);
                        }

                        if (tmpLayerIndex2 == layerIndex)
                        {
                            list1 = list;

                            index1 = list.IndexOf(this);
                        }
                    }

                    list0[index0] = this;

                    list1[index1] = origin;
                }
                else
                {
                    IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        List<UIBase> list = enumerator.Current;

                        if (list[0].layerIndex == origin.layerIndex)
                        {
                            index = list.IndexOf(origin);

                            list[index] = this;

                            break;
                        }
                    }

                    index = parent.children.IndexOf(this);

                    parent.children[index] = origin;
                }
            }
            else
            {
                index = origin.parent.children.IndexOf(origin);

                origin.parent.children[index] = this;

                if (parent == null)
                {
                    IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        List<UIBase> list = enumerator.Current;

                        if (list[0].layerIndex == layerIndex)
                        {
                            index = list.IndexOf(this);

                            list[index] = origin;

                            break;
                        }
                    }
                }
                else
                {
                    index = parent.children.IndexOf(this);

                    parent.children[index] = origin;
                }
            }
        }
        else
        {
            origin.gameObject.SetActive(true);

            Destroy(this);

            if (parent == null)
            {
                IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    List<UIBase> list = enumerator.Current;

                    if (list[0].layerIndex == layerIndex)
                    {
                        index = list.IndexOf(this);

                        list[index] = origin;

                        break;
                    }
                }
            }
            else
            {
                index = parent.children.IndexOf(this);

                parent.children[index] = origin;
            }
        }

        object tmpData = data;

        int tmpUid = uid;

        int tmpLayerIndex = layerIndex;

        UIBase tmpParent = parent;

        List<UIBase> tmpChildren = children;


        data = origin.data;

        uid = origin.uid;

        layerIndex = origin.layerIndex;

        parent = origin.parent;

        children = origin.children;


        origin.data = tmpData;

        origin.uid = tmpUid;

        origin.layerIndex = tmpLayerIndex;

        origin.parent = tmpParent;

        origin.children = tmpChildren;
    }
}
