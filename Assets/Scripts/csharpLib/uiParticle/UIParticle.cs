using UnityEngine;
using UnityEngine.UI;
using publicTools;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticle : Graphic
{
    private ParticleSystem ps;

    private ParticleSystemRenderer psr;

    private ParticleSystem.Particle[] p;

    private float fix;

    private UIVertex[] vertexPool;

    private Camera m_camera;

    private Vector2 canvasRectSizeDelta;

    private int vertexCount;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

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

        p = new ParticleSystem.Particle[ps.main.maxParticles];

        base.raycastTarget = false;

        if (psr.renderMode == ParticleSystemRenderMode.Mesh)
        {
            Mesh mesh = psr.mesh;

            if (mesh == null)
            {
                Destroy(this);

                return;
            }
            else
            {
                vertexCount = mesh.vertexCount;
                vertices = mesh.vertices;
                uv = mesh.uv;
                triangles = mesh.triangles;
            }
        }
        else
        {
            vertexCount = 4;

            vertices = new Vector3[vertexCount];

            uv = new Vector2[vertexCount];

            vertices[0] = new Vector3(-0.5f, -0.5f, 0);
            vertices[1] = new Vector3(-0.5f, 0.5f, 0);
            vertices[2] = new Vector3(0.5f, 0.5f, 0);
            vertices[3] = new Vector3(0.5f, -0.5f, 0);

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            triangles = new int[6];

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
        }

        vertexPool = new UIVertex[vertexCount];

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

    void LateUpdate()
    {
        if (Application.isPlaying)
        {
            transform.rotation = Quaternion.identity;
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

            if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World)
            {
                Vector2 tp = pp.position - transform.position;

                if (ps.main.scalingMode == ParticleSystemScalingMode.Hierarchy)
                {
                    pos = new Vector2(tp.x / transform.lossyScale.x, tp.y / transform.lossyScale.y);
                }
                else
                {
                    size = size * fix;

                    size = new Vector3(size.x * canvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, size.y * canvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y, size.z * canvas.transform.localScale.z / transform.lossyScale.z * transform.localScale.z);

                    pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, tp);

                    pos = new Vector3(pos.x * canvas.transform.localScale.x / transform.lossyScale.x, pos.y * canvas.transform.localScale.y / transform.lossyScale.y);
                }
            }
            else
            {
                if (ps.main.scalingMode == ParticleSystemScalingMode.Hierarchy)
                {
                    pos = pp.position;
                }
                else
                {
                    size = size * fix;

                    size = new Vector3(size.x * canvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, size.y * canvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y, size.z * canvas.transform.localScale.z / transform.lossyScale.z * transform.localScale.z);

                    pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, pp.position);

                    pos = new Vector3(pos.x * canvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, pos.y * canvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y);
                }
            }

            float uFix;
            float vFix;
            float uFixPlus;
            float vFixPlus;

            if (ps.textureSheetAnimation.enabled)
            {
                uFix = 1f / ps.textureSheetAnimation.numTilesX;
                vFix = 1f / ps.textureSheetAnimation.numTilesY;

                int frameNum;

                if (ps.textureSheetAnimation.animation == ParticleSystemAnimationType.WholeSheet)
                {
                    frameNum = ps.textureSheetAnimation.numTilesX * ps.textureSheetAnimation.numTilesY;
                }
                else
                {
                    frameNum = ps.textureSheetAnimation.numTilesX;
                }

                int frame;

                if (ps.textureSheetAnimation.frameOverTime.mode == ParticleSystemCurveMode.Curve)
                {
                    float t = (pp.startLifetime - pp.remainingLifetime) / pp.startLifetime;

                    frame = (int)(ps.textureSheetAnimation.frameOverTime.curve.Evaluate(t) * frameNum);
                }
                else if (ps.textureSheetAnimation.frameOverTime.mode == ParticleSystemCurveMode.TwoConstants)
                {
                    frame = (int)(Mathf.Clamp((float)pp.randomSeed / uint.MaxValue, ps.textureSheetAnimation.frameOverTime.constantMin, ps.textureSheetAnimation.frameOverTime.constantMax) * frameNum);
                }
                else if (ps.textureSheetAnimation.frameOverTime.mode == ParticleSystemCurveMode.Constant)
                {
                    frame = (int)(ps.textureSheetAnimation.frameOverTime.constant * frameNum);
                }
                else
                {
                    throw new System.Exception("unknown ps.textureSheetAnimation.frameOverTime.mode");
                }

                if (ps.textureSheetAnimation.animation == ParticleSystemAnimationType.WholeSheet)
                {
                    uFixPlus = uFix * (frame % ps.textureSheetAnimation.numTilesX);
                    vFixPlus = vFix * (ps.textureSheetAnimation.numTilesY - 1 - frame / ps.textureSheetAnimation.numTilesX);
                }
                else
                {
                    uFixPlus = uFix * frame;
                    vFixPlus = vFix * (ps.textureSheetAnimation.numTilesY - 1 - ps.textureSheetAnimation.rowIndex);
                }
            }
            else
            {
                uFix = 1;
                vFix = 1;
                uFixPlus = 0;
                vFixPlus = 0;
            }

            for (int m = 0; m < vertexCount; m++)
            {
                UIVertex v = UIVertex.simpleVert;

                v.color = color;

                v.position = new Vector2(pos.x + vertices[m].x * size.x, pos.y + vertices[m].y * size.y);

                Vector2 tmpUv = uv[m];

                v.uv0 = new Vector2(tmpUv.x * uFix + uFixPlus, tmpUv.y * vFix + vFixPlus);

                vertexPool[m] = v;

                vh.AddVert(v);
            }

            for (int m = 0; m < triangles.Length / 3; m++)
            {
                vh.AddTriangle(i * vertexCount + triangles[m * 3], i * vertexCount + triangles[m * 3 + 1], i * vertexCount + triangles[m * 3 + 2]);
            }
        }
    }
}
