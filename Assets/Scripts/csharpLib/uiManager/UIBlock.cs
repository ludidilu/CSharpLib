public class UIBlock : UIBase
{
    public UIView origin;

    public void Replace(UIView _origin)
    {
        origin = _origin;

        bool tmpVisible = origin.visible;

        object tmpData = data;

        int tmpUid = uid;

        int tmpLayerIndex = layerIndex;

        UIBase tmpParent = parent;

        origin.SetVisible(false);

        data = origin.data;

        parent = origin.parent;

        uid = origin.uid;

        layerIndex = origin.layerIndex;

        SetVisible(tmpVisible);

        if (origin.parent != null)
        {
            int index = origin.parent.children.IndexOf(origin);

            origin.parent.children[index] = this;
        }

        children.Clear();

        for (int i = 0; i < origin.children.Count; i++)
        {
            children.Add(origin.children[i]);
        }

        origin.data = tmpData;

        origin.uid = tmpUid;

        origin.layerIndex = tmpLayerIndex;

        origin.parent = tmpParent;

        if (tmpParent != null)
        {
            int index = tmpParent.children.IndexOf(this);

            tmpParent.children[index] = origin;
        }

        origin.children.Clear();
    }

    public void Revert(UIView _origin)
    {
        _origin.data = data;

        _origin.parent = parent;

        _origin.uid = uid;

        _origin.SetVisible(visible);

        if (_origin.parent != null)
        {
            int index = _origin.parent.children.IndexOf(this);

            _origin.parent.children[index] = _origin;
        }

        _origin.children.Clear();

        for (int i = 0; i < children.Count; i++)
        {
            UIBase ui = children[i];

            _origin.children.Add(ui);

            ui.parent = _origin;
        }
    }
}
