using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshShadow : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Color shadowColor;

    private TextMesh tm;

    private TextMesh clone;

    // Use this for initialization
    void Awake()
    {
        tm = GetComponent<TextMesh>();

        MeshRenderer mr = GetComponent<MeshRenderer>();

        GameObject go = new GameObject();

        go.transform.SetParent(transform, false);

        go.transform.localScale = Vector3.one;

        clone = go.AddComponent<TextMesh>();

        clone.anchor = tm.anchor;

        clone.font = tm.font;

        clone.text = tm.text;

        clone.color = shadowColor;

        clone.alignment = tm.alignment;

        clone.lineSpacing = tm.lineSpacing;

        clone.tabSize = tm.tabSize;

        clone.fontSize = tm.fontSize;

        clone.fontStyle = tm.fontStyle;

        clone.characterSize = tm.characterSize;

        clone.richText = tm.richText;

        SetShadowOffset(offset);

        MeshRenderer mm = go.GetComponent<MeshRenderer>();

        mm.material = mr.sharedMaterial;
    }

    public void Secloneext(string _str)
    {
        tm.text = _str;

        clone.text = _str;
    }

    public void SetColor(Color _color)
    {
        tm.color = _color;
    }

    public void SetShadowColor(Color _color)
    {
        clone.color = _color;
    }

    public void SetShadowOffset(Vector3 _offset)
    {
        offset = _offset;

        clone.transform.localPosition = new Vector3(offset.x, offset.y, 0);

        clone.offsetZ = offset.z;
    }
}
