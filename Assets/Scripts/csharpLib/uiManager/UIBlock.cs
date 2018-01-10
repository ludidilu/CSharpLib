using System.Collections.Generic;

public class UIBlock : UIBase
{
    public UIView origin;

    public void Replace(LinkedList<List<UIBase>> _stack)
    {
        if (origin.gameObject.activeSelf)
        {
            origin.SetVisible(false);

            if (origin.parent == null)
            {
                IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    List<UIBase> list = enumerator.Current;

                    if (list[0].layerIndex == origin.layerIndex)
                    {
                        int index = list.IndexOf(origin);

                        list[index] = this;

                        break;
                    }
                }
            }
            else
            {
                int index = origin.parent.children.IndexOf(origin);

                origin.parent.children[index] = this;
            }
        }
        else
        {
            origin.gameObject.SetActive(true);

            Destroy(this);
        }

        if (parent == null)
        {
            IEnumerator<List<UIBase>> enumerator = _stack.GetEnumerator();

            while (enumerator.MoveNext())
            {
                List<UIBase> list = enumerator.Current;

                if (list[0].layerIndex == layerIndex)
                {
                    int index = list.IndexOf(this);

                    list[index] = origin;

                    break;
                }
            }
        }
        else
        {
            int index = parent.children.IndexOf(this);

            parent.children[index] = origin;
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
