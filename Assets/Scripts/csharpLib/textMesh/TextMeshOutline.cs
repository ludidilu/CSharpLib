using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextMeshOutline : MonoBehaviour
{
    [SerializeField]
    private float outlineWidth;

    [SerializeField]
    private Color outlineColor;

    [SerializeField]
    private float offsetZ;

    private TextMesh tm;

    private TextMesh[] clones;

    private string text;

    private float alpha;

    private static readonly Vector2[] vs = new Vector2[]
    {
        new Vector2( 1,  0 ),
        new Vector2(-1,  0 ),
        new Vector2( 0,  1 ),
        new Vector2( 0, -1 ),
        new Vector2( 1,  1 ),
        new Vector2(-1, -1 ),
        new Vector2( 1, -1 ),
        new Vector2(-1,  1 )
    };

    // Use this for initialization
    void Awake()
    {
        tm = GetComponent<TextMesh>();

        MeshRenderer mr = GetComponent<MeshRenderer>();

        clones = new TextMesh[8];

        for (int i = 0; i < 8; i++)
        {
            GameObject go = new GameObject();

            go.transform.SetParent(transform, false);

            Vector2 v = vs[i];

            go.transform.localPosition = new Vector3(v.x * outlineWidth, v.y * outlineWidth, 0);

            go.transform.localScale = Vector3.one;

            TextMesh tt = go.AddComponent<TextMesh>();

            tt.anchor = tm.anchor;

            tt.font = tm.font;

            text = tm.text;

            tt.text = text;

            alpha = tt.color.a;

            tt.color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a * alpha * alpha);

            tt.offsetZ = offsetZ;

            tt.alignment = tm.alignment;

            tt.lineSpacing = tm.lineSpacing;

            tt.tabSize = tm.tabSize;

            tt.fontSize = tm.fontSize;

            tt.fontStyle = tm.fontStyle;

            tt.characterSize = tm.characterSize;

            tt.richText = tm.richText;

            MeshRenderer mm = go.GetComponent<MeshRenderer>();

            mm.material = mr.sharedMaterial;

            clones[i] = tt;
        }

        if (!enabled)
        {
            OnDisable();
        }
    }

    public void SetOutlineColor(Color _outlineColor)
    {
        outlineColor = _outlineColor;

        for (int i = 0; i < 8; i++)
        {
            clones[i].color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a * alpha * alpha);
        }
    }

    public void SetOutlineWidth(float _outlineWidth)
    {
        outlineWidth = _outlineWidth;

        for (int i = 0; i < 8; i++)
        {
            Vector2 v = vs[i];

            clones[i].transform.localPosition = new Vector3(v.x * outlineWidth, v.y * outlineWidth, 0);
        }
    }

    public void SetOffsetZ(float _offsetZ)
    {
        for (int i = 0; i < 8; i++)
        {
            clones[i].offsetZ = _offsetZ;
        }
    }

    void LateUpdate()
    {
        if (tm.text != text)
        {
            text = tm.text;

            for (int i = 0; i < 8; i++)
            {
                clones[i].text = text;
            }
        }

        if (tm.color.a != alpha)
        {
            alpha = tm.color.a;

            for (int i = 0; i < 8; i++)
            {
                clones[i].color = new Color(outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a * alpha * alpha);
            }
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < 8; i++)
        {
            Destroy(clones[i].gameObject);
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < 8; i++)
        {
            clones[i].gameObject.SetActive(true);
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < 8; i++)
        {
            clones[i].gameObject.SetActive(false);
        }
    }
}
