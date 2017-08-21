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

    private string text;

    private float alpha;

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

        text = tm.text;

        clone.text = text;

        alpha = tm.color.a;

        clone.color = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowColor.a * alpha);

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

        if (!enabled)
        {
            OnDisable();
        }
    }

    public void SetShadowColor(Color _color)
    {
        shadowColor = _color;

        clone.color = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowColor.a * alpha);
    }

    public void SetShadowOffset(Vector3 _offset)
    {
        offset = _offset;

        clone.transform.localPosition = new Vector3(offset.x, offset.y, 0);

        clone.offsetZ = offset.z;
    }

    void LateUpdate()
    {
        if (tm.text != text)
        {
            text = tm.text;

            clone.text = text;
        }

        if (tm.color.a != alpha)
        {
            alpha = tm.color.a;

            clone.color = new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowColor.a * alpha);
        }
    }

    void OnDestroy()
    {
        Destroy(clone.gameObject);
    }

    void OnEnable()
    {
        clone.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        clone.gameObject.SetActive(false);
    }
}
