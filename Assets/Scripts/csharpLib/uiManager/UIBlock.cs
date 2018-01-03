public class UIBlock : UIBase
{
    public UIView origin { private set; get; }

    public void Replace(UIView _origin)
    {
        origin = _origin;

        SetVisible(_origin.visible);

        data = origin.data;

        parent = origin.parent;

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
    }

    public void Revert(UIView _origin)
    {
        _origin.parent = parent;

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
