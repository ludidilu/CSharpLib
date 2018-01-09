public class UIBlock : UIBase
{
    public UIView origin;

    public void Replace(UIView _origin)
    {
        origin = _origin;

        data = origin.data;

        parent = origin.parent;

        uid = origin.uid;

        if (origin.visible)
        {
            origin.OnHide();
        }

        SetVisible(origin.visible);

        if (parent != null)
        {
            int index = parent.children.IndexOf(origin);

            parent.children[index] = this;
        }

        children.Clear();

        for (int i = 0; i < origin.children.Count; i++)
        {
            UIBase ui = origin.children[i];

            children.Add(ui);

            ui.parent = this;
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
