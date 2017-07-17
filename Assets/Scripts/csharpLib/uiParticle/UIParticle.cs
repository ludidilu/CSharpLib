using UnityEngine;
using UnityEngine.UI;
using publicTools;

[RequireComponent(typeof(ParticleSystem))]
public class UIParticle : Graphic
{
    private static ParticleSystem.Particle[] p = new ParticleSystem.Particle[1000];

    private ParticleSystem ps;

    private ParticleSystemRenderer psr;

    private float fix;

    private Camera m_camera;

    private Vector2 canvasRectSizeDelta;

    private int vertexCount;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private Transform tmpTrans;

    private float minSize;

    private float maxSize;

    protected override void Start()
    {
        base.Start();

        m_camera = canvas.worldCamera;

        canvasRectSizeDelta = (canvas.rootCanvas.transform as RectTransform).sizeDelta;

        Vector2 pos0 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(0, 0, 0));

        Vector2 pos1 = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, new Vector3(1, 0, 0));

        fix = Mathf.Abs(pos0.x - pos1.x);

        if (psr.renderMode == ParticleSystemRenderMode.Billboard)
        {
            Vector3 vvv = m_camera.ScreenToWorldPoint(new Vector3(0, 0, 0));

            Vector3 vvv2 = m_camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            float screenSize = vvv2.x - vvv.x;

            minSize = screenSize * psr.minParticleSize;

            maxSize = screenSize * psr.maxParticleSize;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        ps = GetComponent<ParticleSystem>();

        psr = GetComponent<ParticleSystemRenderer>();

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

        if (psr.renderMode == ParticleSystemRenderMode.Stretch)
        {
            GameObject gg = new GameObject();

            tmpTrans = gg.transform;

            tmpTrans.SetParent(transform, false);
        }

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
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, Vector3.one);

        vh.Clear();

        int num = ps.GetParticles(p);

        for (int i = 0; i < num; i++)
        {
            ParticleSystem.Particle pp = p[i];

            Vector3 size = pp.GetCurrentSize3D(ps);

            if (psr.renderMode == ParticleSystemRenderMode.Billboard)
            {
                size = new Vector3(Mathf.Clamp(size.x, minSize, maxSize), Mathf.Clamp(size.y, minSize, maxSize), Mathf.Clamp(size.z, minSize, maxSize));
            }

            Color color = pp.GetCurrentColor(ps);

            Vector3 pos;

            if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World)
            {
                Vector3 tp = pp.position - transform.position;

                if (ps.main.scalingMode == ParticleSystemScalingMode.Hierarchy)
                {
                    pos = new Vector3(tp.x / transform.lossyScale.x, tp.y / transform.lossyScale.y, tp.z / transform.lossyScale.z);
                }
                else
                {
                    size = size * fix;

                    size = new Vector3(size.x * canvas.rootCanvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, size.y * canvas.rootCanvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y, size.z * canvas.transform.localScale.z / transform.lossyScale.z * transform.localScale.z);

                    pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, tp);

                    pos = new Vector3(pos.x * canvas.rootCanvas.transform.localScale.x / transform.lossyScale.x, pos.y * canvas.rootCanvas.transform.localScale.y / transform.lossyScale.y);
                }
            }
            else
            {
                if (ps.main.scalingMode == ParticleSystemScalingMode.Hierarchy)
                {
                    pos = pp.position;

                    pos = matrix.MultiplyPoint3x4(pos);
                }
                else
                {
                    size = size * fix;

                    size = new Vector3(size.x * canvas.rootCanvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, size.y * canvas.rootCanvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y, size.z * canvas.transform.localScale.z / transform.lossyScale.z * transform.localScale.z);

                    pos = matrix.MultiplyPoint3x4(pp.position);

                    pos = PublicTools.WorldPositionToCanvasPosition(m_camera, canvasRectSizeDelta, pos);

                    pos = new Vector3(pos.x * canvas.rootCanvas.transform.localScale.x / transform.lossyScale.x * transform.localScale.x, pos.y * canvas.rootCanvas.transform.localScale.y / transform.lossyScale.y * transform.localScale.y);
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
                    Random.InitState((int)pp.randomSeed);

                    frame = (int)(Random.Range(ps.textureSheetAnimation.frameOverTime.constantMin, ps.textureSheetAnimation.frameOverTime.constantMax) * frameNum);

                    //frame = (int)(Mathf.Clamp(Random.value, ps.textureSheetAnimation.frameOverTime.constantMin, ps.textureSheetAnimation.frameOverTime.constantMax) * frameNum);
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

            Vector3 scale = Vector3.one;

            Quaternion qq;

            if (psr.renderMode == ParticleSystemRenderMode.Billboard)
            {
                qq = Quaternion.AngleAxis(pp.rotation, Vector3.back);
            }
            else if (psr.renderMode == ParticleSystemRenderMode.Mesh)
            {
                qq = Quaternion.AngleAxis(pp.rotation, pp.axisOfRotation);
            }
            else if (psr.renderMode == ParticleSystemRenderMode.Stretch)
            {
                Vector3 vv = matrix.inverse.MultiplyPoint3x4(pos);

                tmpTrans.localPosition = vv;

                float lengthScale = transform.lossyScale.x / transform.lossyScale.z * psr.lengthScale;

                if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
                {
                    tmpTrans.LookAt(transform.TransformPoint(vv + pp.velocity), Vector3.back);

                    tmpTrans.localPosition = tmpTrans.localPosition - transform.InverseTransformDirection(tmpTrans.forward) * lengthScale * 0.5f;

                    tmpTrans.Rotate(Vector3.up, 90f);

                    tmpTrans.Rotate(Vector3.left, -90f);

                    qq = tmpTrans.localRotation;
                }
                else
                {
                    tmpTrans.LookAt(transform.TransformPoint(vv) + pp.velocity, Vector3.back);

                    tmpTrans.localPosition = tmpTrans.localPosition - transform.InverseTransformDirection(tmpTrans.forward) * lengthScale * 0.5f;

                    tmpTrans.Rotate(Vector3.up, 90f);

                    tmpTrans.Rotate(Vector3.left, -90f);

                    qq = tmpTrans.rotation;
                }

                pos = matrix.MultiplyPoint3x4(tmpTrans.localPosition);

                scale.x = lengthScale;
            }
            else
            {
                throw new System.Exception("error!");
            }

            Matrix4x4 mm = Matrix4x4.TRS(Vector3.zero, qq, scale);

            if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local && psr.renderMode != ParticleSystemRenderMode.Billboard)
            {
                mm = matrix * mm;
            }

            for (int m = 0; m < vertexCount; m++)
            {
                Color nowColor = color;

                //Color nowColor = Color.clear;

                Vector3 vv = mm.MultiplyPoint3x4(vertices[m]);

                //vv = new Vector3(pos.x + vv.x * size.x, pos.y + vv.y * size.y, pos.z + vv.z * size.z);

                vv = new Vector3(pos.x + vv.x * size.x, pos.y + vv.y * size.y, 0);

                Vector3 nowPos = matrix.inverse.MultiplyPoint3x4(vv);

                Vector2 tmpUv = uv[m];

                Vector2 nowUv = new Vector2(tmpUv.x * uFix + uFixPlus, tmpUv.y * vFix + vFixPlus);

                vh.AddVert(nowPos, nowColor, nowUv);
            }

            for (int m = 0; m < triangles.Length / 3; m++)
            {
                vh.AddTriangle(i * vertexCount + triangles[m * 3], i * vertexCount + triangles[m * 3 + 1], i * vertexCount + triangles[m * 3 + 2]);
            }
        }
    }
}
