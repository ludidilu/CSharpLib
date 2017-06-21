using UnityEngine;
using UnityEngine.UI;
using publicTools;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticle : Graphic
{

    private static readonly Vector4 uv = new Vector4(0, 0, 1, 1);

    private ParticleSystem ps;

    private ParticleSystemRenderer psr;

    private ParticleSystem.Particle[] p;

    private float fix;

    private UIVertex[] vertex;

    private Camera m_camera;

    private Vector2 canvasRectSizeDelta;

    void Start()
    {

        canvasRectSizeDelta = (canvas.transform as RectTransform).sizeDelta;

        Vector2 pos0 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(0, 0, 0));

        Vector2 pos1 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(1, 0, 0));

        fix = Mathf.Abs(pos0.x - pos1.x) * 0.5f;
    }

    protected override void Awake()
    {
        base.Awake();

        m_camera = canvas.worldCamera;

        ps = GetComponent<ParticleSystem>();

        psr = GetComponent<ParticleSystemRenderer>();

        p = new ParticleSystem.Particle[ps.maxParticles];

        vertex = new UIVertex[4];

        base.raycastTarget = false;

        psr.enabled = false;
    }

    public override Material material
    {

        get
        {

            if (Application.isPlaying)
            {

                return psr.sharedMaterial;

            }
            else {

                return base.material;
            }
        }
    }

    public override Texture mainTexture
    {

        get
        {

            if (Application.isPlaying)
            {

                return psr.sharedMaterial.mainTexture;

            }
            else {

                return material.mainTexture;
            }
        }
    }

    void Update()
    {

        if (Application.isPlaying)
        {

//			ps.Simulate (Time.unscaledDeltaTime, false, false);

            SetVerticesDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {

        vh.Clear();

        int num = ps.GetParticles(p);

        for (int i = 0; i < num; i++)
        {

            ParticleSystem.Particle pp = p[i];

            Vector2 size = pp.GetCurrentSize3D(ps);

            Color color = pp.GetCurrentColor(ps);

            if(ps.scalingMode == ParticleSystemScalingMode.Local)
            {
                size = size * fix;

                Vector2 pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, pp.position);

                UIVertex v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x - size.x, pos.y - size.y);

                v.uv0 = new Vector2(uv.x, uv.y);

                vertex[0] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x - size.x, pos.y + size.y);

                v.uv0 = new Vector2(uv.x, uv.w);

                vertex[1] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + size.x, pos.y + size.y);

                v.uv0 = new Vector2(uv.z, uv.w);

                vertex[2] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + size.x, pos.y - size.y);

                v.uv0 = new Vector2(uv.z, uv.y);

                vertex[3] = v;
            }
            else if(ps.scalingMode == ParticleSystemScalingMode.Hierarchy)
            {
                size = size * 0.5f;

                Vector2 pos = pp.position;

                UIVertex v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x - size.x, pos.y - size.y);

                v.uv0 = new Vector2(uv.x, uv.y);

                vertex[0] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x - size.x, pos.y + size.y);

                v.uv0 = new Vector2(uv.x, uv.w);

                vertex[1] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + size.x, pos.y + size.y);

                v.uv0 = new Vector2(uv.z, uv.w);

                vertex[2] = v;

                v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + size.x, pos.y - size.y);

                v.uv0 = new Vector2(uv.z, uv.y);

                vertex[3] = v;
            }

            vh.AddUIVertexQuad(vertex);
        }
    }
}
