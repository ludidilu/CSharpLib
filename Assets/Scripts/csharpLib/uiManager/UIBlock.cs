public class UIBlock : UIBase
{
    public UIBase origin { private set; get; }

    public void SetOrigin(UIBase _origin)
    {
        origin = _origin;

        data = origin.data;

        parent = origin.parent;

        children.Clear();

        for (int i = 0; i < origin.children.Count; i++)
        {
            children.Add(origin.children[i]);
        }
    }
}
