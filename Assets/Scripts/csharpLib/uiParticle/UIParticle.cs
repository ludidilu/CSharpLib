using UnityEngine;
using UnityEngine.UI;
using publicTools;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticle : Graphic
{

    private static Mesh quadMesh;

    private ParticleSystem ps;

    private ParticleSystemRenderer psr;

    private ParticleSystem.Particle[] p;

    private float fix;

    private UIVertex[] vertexPool;

    private Camera m_camera;

    private Vector2 canvasRectSizeDelta;

    private Mesh mesh;

    protected override void Start()
    {

        base.Start();

        m_camera = canvas.worldCamera;

        canvasRectSizeDelta = (rectTransform.root as RectTransform).sizeDelta;

        Vector2 pos0 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(0, 0, 0));

        Vector2 pos1 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(1, 0, 0));

        fix = Mathf.Abs(pos0.x - pos1.x);
    }

    protected override void Awake()
    {
        base.Awake();

        ps = GetComponent<ParticleSystem>();

        psr = GetComponent<ParticleSystemRenderer>();

        p = new ParticleSystem.Particle[ps.maxParticles];

        base.raycastTarget = false;

        if (psr.renderMode == ParticleSystemRenderMode.Mesh)
        {
            mesh = psr.mesh;

            if (mesh == null)
            {
                Destroy(this);

                return;
            }
            else
            {
                vertexPool = new UIVertex[mesh.vertexCount];
            }
        }
        else
        {
            if (quadMesh == null)
            {
                quadMesh = new Mesh();

                Vector3[] vertex = new Vector3[4];

                Vector2[] uv = new Vector2[4];

                vertex[0] = new Vector3(-0.5f, -0.5f, 0);
                vertex[1] = new Vector3(-0.5f, 0.5f, 0);
                vertex[2] = new Vector3(0.5f, 0.5f, 0);
                vertex[3] = new Vector3(0.5f, -0.5f, 0);

                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(0, 1);
                uv[2] = new Vector2(1, 1);
                uv[3] = new Vector2(1, 0);

                quadMesh.vertices = vertex;

                int[] index = new int[6];

                index[0] = 0;

                index[1] = 1;

                index[2] = 2;

                index[3] = 0;

                index[4] = 2;

                index[5] = 3;

                quadMesh.triangles = index;

                quadMesh.uv = uv;
            }

            mesh = quadMesh;

            vertexPool = new UIVertex[4];
        }

        if (!psr.enabled)
        {
            enabled = false;
        }
        else
        {
            psr.enabled = false;
        }
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

            Vector3 size = pp.GetCurrentSize3D(ps);

            Color color = pp.GetCurrentColor(ps);

            Vector2 pos;

            if (ps.scalingMode == ParticleSystemScalingMode.Hierarchy)
            {
                pos = pp.position;
            }
            else
            {
                size = size * fix;

                pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, pp.position);
            }

            for (int m = 0; m < mesh.vertexCount; m++)
            {
                UIVertex v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + mesh.vertices[m].x * size.x, pos.y + mesh.vertices[m].y * size.y);

                v.uv0 = mesh.uv[m];

                vertexPool[m] = v;

                vh.AddVert(v);
            }

            for (int m = 0; m < mesh.triangles.Length / 3; m++)
            {
                vh.AddTriangle(i * mesh.vertexCount + mesh.triangles[m * 3], i * mesh.vertexCount + mesh.triangles[m * 3 + 1], i * mesh.vertexCount + mesh.triangles[m * 3 + 2]);
            }
        }
    }
}
