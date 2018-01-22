using System;
using UnityEngine;
using System.Collections.Generic;
using superTween;
using UnityEngine.UI;
using System.IO;

namespace publicTools
{
    public static class PublicTools
    {
        public static readonly string NORMAL_TAG_NAME = "Untagged";

        public static void ClearTag(GameObject _obj)
        {
            _obj.tag = NORMAL_TAG_NAME;

            for (int i = 0; i < _obj.transform.childCount; i++)
            {

                ClearTag(_obj.transform.GetChild(i).gameObject);
            }
        }

        public static void SetTag(GameObject _obj, string _tag)
        {
            _obj.tag = _tag;

            for (int i = 0; i < _obj.transform.childCount; i++)
            {

                SetTag(_obj.transform.GetChild(i).gameObject, _tag);
            }
        }

        public static GameObject FindChild(GameObject _obj, string _name)
        {
            return FindChild(_obj, _name, 0);
        }

        public static GameObject FindChildForce(GameObject _obj, string _name)
        {
            if (_obj.name == _name)
            {
                return _obj;
            }
            else
            {
                for (int i = 0; i < _obj.transform.childCount; i++)
                {
                    GameObject tmpObj = FindChildForce(_obj.transform.GetChild(i).gameObject, _name);

                    if (tmpObj != null)
                    {
                        return tmpObj;
                    }
                }

                return null;
            }
        }

        private static GameObject FindChild(GameObject _obj, string _name, int _index)
        {
            if (_obj.name == _name)
            {
                return _obj;
            }
            else
            {
                _index++;

                for (int i = 0; i < _obj.transform.childCount; i++)
                {
                    GameObject tmpObj = FindChild(_obj.transform.GetChild(i).gameObject, _name, _index);

                    if (tmpObj != null)
                    {
                        return tmpObj;
                    }
                }

                if (_index == 1)
                {
                    throw new ArgumentOutOfRangeException("【PublicTools】----" + "找不到名字为" + _name + "的GameObject");
                }
                else
                {
                    return null;
                }
            }
        }

        public static void AddChild(GameObject _parent, GameObject _child, string _jointName)
        {
            if (!string.IsNullOrEmpty(_jointName))
            {
                GameObject joint = FindChild(_parent, _jointName).gameObject;

                if (joint != null)
                {
                    _child.transform.SetParent(joint.transform, false);
                }
                else
                {
                    _child.transform.SetParent(_parent.transform, false);
                }

            }
            else
            {
                _child.transform.SetParent(_parent.transform, false);
            }
        }

        public static void SetLayer(GameObject _go, int _layer)
        {
            _go.layer = _layer;

            for (int i = 0; i < _go.transform.childCount; i++)
            {
                SetLayer(_go.transform.GetChild(i).gameObject, _layer);
            }
        }

        public static void SetGameObjectVisible(GameObject _go, bool _visible)
        {
            Renderer[] renderers = _go.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = _visible;
            }
        }

        public static void StopParticle(GameObject _go)
        {
            ParticleSystem[] systems = _go.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem system in systems)
            {
                system.Stop();
            }
        }

        public static void PlayParticle(GameObject _go)
        {
            ParticleSystem[] systems = _go.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem system in systems)
            {
                system.Play();
            }
        }

        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string XmlFix(string _str)
        {
            int index = _str.IndexOf("<");

            if (index == -1)
            {
                return "";
            }
            else
            {
                return _str.Substring(index, _str.Length - index);
            }
        }

        public static Vector3 WorldPositionToCanvasPosition(Camera _worldCamera, Vector2 _canvasRectSizeDelta, Vector3 _worldPosition)
        {
            Vector3 screenPos = _worldCamera.WorldToViewportPoint(_worldPosition);

            return new Vector3((screenPos.x * _canvasRectSizeDelta.x) - (_canvasRectSizeDelta.x * 0.5f), (screenPos.y * _canvasRectSizeDelta.y) - (_canvasRectSizeDelta.y * 0.5f), screenPos.z);
        }

        public static Vector3 CanvasPositionToWorldPosition(Camera _worldCamea, Vector2 _canvasRectSizeDelta, Vector2 _canvasPosition)
        {
            Vector3 screenPos = new Vector3((_canvasPosition.x + _canvasRectSizeDelta.x * 0.5f) / _canvasRectSizeDelta.x, (_canvasPosition.y + _canvasRectSizeDelta.y * 0.5f) / _canvasRectSizeDelta.y, 0);

            return _worldCamea.ViewportToWorldPoint(screenPos);
        }

        public static Vector3 MousePositionToCanvasPosition(Canvas _canvas, Vector3 _mousePosition)
        {
            Vector3 screenPos = _canvas.worldCamera.ScreenToViewportPoint(_mousePosition);

            RectTransform canvasRect = _canvas.transform as RectTransform;

            return new Vector3((screenPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f), (screenPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f), screenPos.z);
        }

        public static Vector3 CanvasPostionToMousePosition(Canvas _canvas, Vector2 _canvasPosition)
        {
            RectTransform canvasRect = _canvas.transform as RectTransform;

            Vector3 screenPos = new Vector3((_canvasPosition.x + canvasRect.sizeDelta.x * 0.5f) / canvasRect.sizeDelta.x, (_canvasPosition.y + canvasRect.sizeDelta.y * 0.5f) / canvasRect.sizeDelta.y, 0);

            return _canvas.worldCamera.ViewportToScreenPoint(screenPos);
        }

        //场景被卸载时，如果场景上有GameObject从来没有active过，那GameObject所引用的贴图就会残留在内存里了  这个方法就是确保场景上所有GameObject都曾经被active过
        public static void UnloadAllSceneGameObjects(GameObject _go)
        {
            if (!_go.activeSelf)
            {
                _go.SetActive(true);

                _go.SetActive(false);
            }

            for (int i = 0; i < _go.transform.childCount; i++)
            {
                UnloadAllSceneGameObjects(_go.transform.GetChild(i).gameObject);
            }
        }

        public static ushort ReverseBytes(ushort value)
        {
            return (ushort)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static short ReverseBytes(short value)
        {
            return (short)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static uint ReverseBytes(uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static int ReverseBytes(int value)
        {
            return (int)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                           (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }

        public static ulong ReverseBytes(ulong value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                    (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                    (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        public static void FlipParticleMeshWithAxisX(GameObject _go, float _zPos)
        {
            ParticleSystemRenderer[] particleRenders = _go.GetComponentsInChildren<ParticleSystemRenderer>();

            for (int i = 0; i < particleRenders.Length; i++)
            {
                ParticleSystemRenderer particleRender = particleRenders[i];

                Material mat = particleRender.material;

                if (mat != null && particleRender.renderMode == ParticleSystemRenderMode.Mesh)
                {
                    mat.SetFloat("_Flip", -1);
                }
            }

            FlipGameObjectWithAxisX(_go.transform, _zPos);
        }

        private static void FlipGameObjectWithAxisX(Transform _trans, float _zPos)
        {
            Vector3 position = new Vector3(_trans.position.x, _trans.position.y, _zPos * 2 - _trans.position.z);

            Vector3 up = _trans.up;

            Vector3 upFix = new Vector3(up.x, up.y, -up.z);

            Vector3 forward = _trans.position + _trans.forward;

            Vector3 forwardFix = new Vector3(forward.x, forward.y, _zPos * 2 - forward.z);

            _trans.position = position;

            _trans.LookAt(forwardFix, upFix);

            for (int i = 0; i < _trans.childCount; i++)
            {
                FlipGameObjectWithAxisX(_trans.GetChild(i), _zPos);
            }
        }

        public static void FlashOut(GameObject _go, float _time, int _times, Action _callBack)
        {
            Renderer[] renders = _go.GetComponentsInChildren<Renderer>();

            float oneTime = _time / _times;

            Action<float> toDel = delegate (float obj)
            {
                int index = (int)(obj / oneTime);

                float fix = (obj - index * oneTime) / oneTime;

                if (fix > (float)index / _times)
                {
                    for (int i = 0; i < renders.Length; i++)
                    {
                        renders[i].enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < renders.Length; i++)
                    {
                        renders[i].enabled = false;
                    }
                }
            };

            Action endDel = delegate ()
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    renders[i].enabled = false;
                }

                if (_callBack != null)
                {
                    _callBack();
                }
            };

            SuperTween.Instance.To(0, _time, _time, toDel, endDel);
        }

        public static void FlashIn(GameObject _go, float _time, int _times, Action _callBack)
        {
            Renderer[] renders = _go.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].enabled = false;
            }

            float oneTime = _time / _times;

            Action<float> toDel = delegate (float obj)
            {
                int index = (int)(obj / oneTime);

                float fix = (obj - index * oneTime) / oneTime;

                if (fix < (float)index / _times)
                {
                    for (int i = 0; i < renders.Length; i++)
                    {
                        renders[i].enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < renders.Length; i++)
                    {
                        renders[i].enabled = false;
                    }
                }
            };

            Action endDel = delegate ()
            {
                for (int i = 0; i < renders.Length; i++)
                {
                    renders[i].enabled = true;
                }

                if (_callBack != null)
                {
                    _callBack();
                }
            };

            SuperTween.Instance.To(0, _time, _time, toDel, endDel);
        }

        public static Mesh CombineMeshs(GameObject[] _gos)
        {
            int num = _gos.Length;

            Mesh mesh = new Mesh();

            CombineInstance[] cis = new CombineInstance[num];

            for (int i = 0; i < num; i++)
            {
                GameObject go = _gos[i];

                MeshFilter mf = go.GetComponent<MeshFilter>();

                Mesh tmpMesh = mf.mesh;

                Vector4[] ts = new Vector4[tmpMesh.vertexCount];

                for (int m = 0; m < tmpMesh.vertexCount; m++)
                {
                    ts[m] = new Vector4(i, i, i, i);
                }

                tmpMesh.tangents = ts;

                cis[i].mesh = tmpMesh;

                //				cis[i].transform = _gos[i].transform.localToWorldMatrix;

                cis[i].transform = Matrix4x4.TRS(go.transform.localPosition, go.transform.localRotation, go.transform.localScale);

                SetGameObjectVisible(go, false);
            }

            mesh.CombineMeshes(cis);

            return mesh;
        }

        public static GameObject CombineImage(Transform _container, Material _mat, List<List<Sprite>> _sprites, List<Vector2> _pos, List<Vector2> _rect)
        {
            int num = _sprites.Count;

            Mesh mesh = new Mesh();

            List<CombineInstance> cis = new List<CombineInstance>();

            GameObject tmpGo = new GameObject("tmpGo", typeof(RectTransform));

            tmpGo.transform.SetParent(_container, false);

            RectTransform tmpRect = tmpGo.transform as RectTransform;

            for (int i = 0; i < num; i++)
            {
                List<Sprite> sps = _sprites[i];

                if (sps.Count == 1)
                {
                    Sprite sp = _sprites[i][0];

                    tmpRect.anchoredPosition = _pos[i];

                    Mesh tmpMesh = new Mesh();

                    Vector3[] vertices = new Vector3[sp.vertices.Length];

                    for (int m = 0; m < vertices.Length; m++)
                    {
                        vertices[m] = sp.vertices[m];
                    }

                    tmpMesh.vertices = vertices;

                    Vector2[] uv = new Vector2[sp.vertices.Length];

                    for (int m = 0; m < uv.Length; m++)
                    {
                        uv[m] = new Vector2(-1, 0);
                    }

                    tmpMesh.uv2 = uv;

                    int[] triangles = new int[sp.triangles.Length];

                    for (int m = 0; m < triangles.Length; m++)
                    {
                        triangles[m] = sp.triangles[m];
                    }

                    tmpMesh.triangles = triangles;

                    tmpMesh.uv = sp.uv;

                    CombineInstance ci = new CombineInstance();

                    ci.mesh = tmpMesh;

                    ci.transform = Matrix4x4.TRS(tmpRect.localPosition, Quaternion.identity, new Vector3(_rect[i].x * 100 / sp.rect.width, _rect[i].y * 100 / sp.rect.height, 0));

                    cis.Add(ci);
                }
                else
                {
                    for (int m = 0; m < sps.Count; m++)
                    {
                        Sprite sp = _sprites[i][m];

                        tmpRect.anchoredPosition = _pos[i];

                        Mesh tmpMesh = new Mesh();

                        Vector3[] vertices = new Vector3[sp.vertices.Length];

                        for (int n = 0; n < vertices.Length; n++)
                        {
                            vertices[n] = sp.vertices[n];
                        }

                        tmpMesh.vertices = vertices;

                        Vector2[] uv = new Vector2[sp.vertices.Length];

                        for (int n = 0; n < uv.Length; n++)
                        {
                            uv[n] = new Vector2(m, sps.Count);
                        }

                        tmpMesh.uv2 = uv;

                        int[] triangles = new int[sp.triangles.Length];

                        for (int n = 0; n < triangles.Length; n++)
                        {
                            triangles[n] = sp.triangles[n];
                        }

                        tmpMesh.triangles = triangles;

                        tmpMesh.uv = sp.uv;

                        CombineInstance ci = new CombineInstance();

                        ci.mesh = tmpMesh;

                        ci.transform = Matrix4x4.TRS(tmpRect.localPosition, Quaternion.identity, new Vector3(_rect[i].x * 100 / sp.rect.width, _rect[i].y * 100 / sp.rect.height, 0));

                        cis.Add(ci);
                    }
                }
            }

            mesh.CombineMeshes(cis.ToArray());

            GameObject result = new GameObject();

            MeshFilter mf = result.AddComponent<MeshFilter>();

            mf.mesh = mesh;

            MeshRenderer mr = result.AddComponent<MeshRenderer>();

            _mat.mainTexture = _sprites[0][0].texture;

            mr.material = _mat;

            result.transform.SetParent(_container, false);

            GameObject.Destroy(tmpGo);

            return result;
        }

        public static GameObject CombineImage(Transform _container, Material _mat, List<Sprite> _sprites, List<Vector2> _pos, List<Vector2> _rect)
        {
            int num = _sprites.Count;

            Mesh mesh = new Mesh();

            CombineInstance[] cis = new CombineInstance[num];

            GameObject tmpGo = new GameObject("tmpGo", typeof(RectTransform));

            tmpGo.transform.SetParent(_container, false);

            RectTransform tmpRect = tmpGo.transform as RectTransform;

            for (int i = 0; i < num; i++)
            {
                Sprite sp = _sprites[i];

                tmpRect.anchoredPosition = _pos[i];

                Mesh tmpMesh = new Mesh();

                Vector3[] vertices = new Vector3[sp.vertices.Length];

                for (int m = 0; m < vertices.Length; m++)
                {
                    vertices[m] = sp.vertices[m];
                }

                tmpMesh.vertices = vertices;

                int[] triangles = new int[sp.triangles.Length];

                for (int m = 0; m < triangles.Length; m++)
                {

                    triangles[m] = sp.triangles[m];
                }

                tmpMesh.triangles = triangles;

                tmpMesh.uv = sp.uv;

                cis[i].mesh = tmpMesh;

                cis[i].transform = Matrix4x4.TRS(tmpRect.localPosition, Quaternion.identity, new Vector3(_rect[i].x * 100 / sp.rect.width, _rect[i].y * 100 / sp.rect.height, 0));
            }

            mesh.CombineMeshes(cis);

            GameObject result = new GameObject();

            MeshFilter mf = result.AddComponent<MeshFilter>();

            mf.mesh = mesh;

            MeshRenderer mr = result.AddComponent<MeshRenderer>();

            _mat.mainTexture = _sprites[0].texture;

            mr.material = _mat;

            result.transform.SetParent(_container, false);

            GameObject.Destroy(tmpGo);

            return result;
        }

        public static GameObject CombineImage(Transform _container, Material _mat, List<Image> _images)
        {
            int num = _images.Count;

            Mesh mesh = new Mesh();

            CombineInstance[] cis = new CombineInstance[num];

            for (int i = 0; i < num; i++)
            {
                Sprite sp = _images[i].sprite;

                Mesh tmpMesh = new Mesh();

                Vector3[] vertices = new Vector3[sp.vertices.Length];

                for (int m = 0; m < vertices.Length; m++)
                {
                    vertices[m] = sp.vertices[m];
                }

                tmpMesh.vertices = vertices;

                int[] triangles = new int[sp.triangles.Length];

                for (int m = 0; m < triangles.Length; m++)
                {
                    triangles[m] = sp.triangles[m];
                }

                tmpMesh.triangles = triangles;

                tmpMesh.uv = sp.uv;

                cis[i].mesh = tmpMesh;

                RectTransform rect = _images[i].transform as RectTransform;

                cis[i].transform = Matrix4x4.TRS(rect.localPosition, Quaternion.identity, new Vector3(rect.sizeDelta.x * 100 / sp.rect.width, rect.sizeDelta.y * 100 / sp.rect.height, 0));
            }

            mesh.CombineMeshes(cis);

            GameObject result = new GameObject();

            MeshFilter mf = result.AddComponent<MeshFilter>();

            mf.mesh = mesh;

            MeshRenderer mr = result.AddComponent<MeshRenderer>();

            _mat.mainTexture = _images[0].sprite.texture;

            mr.material = _mat;

            result.transform.SetParent(_container, false);

            return result;
        }

        public static int[] SplitInt(int _data, int _num, float _range)
        {
            int[] result = new int[_num];

            int numRec = _num;

            for (int i = 0; i < numRec - 1; i++)
            {
                int data = (int)((float)_data / _num * (1 + (UnityEngine.Random.value * 2 - 1) * _range));

                result[i] = data;

                _data -= data;

                _num--;
            }

            result[numRec - 1] = _data;

            return result;
        }

        public static float[] SplitFloat(float _data, int _num, float _range)
        {
            float[] result = new float[_num];

            int numRec = _num;

            for (int i = 0; i < numRec - 1; i++)
            {
                float data = (float)_data / _num * (1 + (UnityEngine.Random.value * 2 - 1) * _range);

                result[i] = data;

                _data -= data;

                _num--;
            }

            result[numRec - 1] = _data;

            return result;
        }

        public static string FixStringChangeLine(string _str)
        {
            return _str.Replace("\\n", "\n");
        }

        public static List<Sprite> CreateSpriteFrames(Sprite _sprite, int _frameWidth, int _frameNum)
        {
            List<Sprite> result = new List<Sprite>();

            int frameWidth = (int)_sprite.rect.width / _frameWidth;
            int height = Mathf.CeilToInt((float)_frameNum / (float)_frameWidth);
            int frameHeight = (int)_sprite.rect.height / height;
            int frameNb = 0;
            Rect rFrame = new Rect(0f, 0f, (float)frameWidth, (float)frameHeight);

            for (rFrame.y = _sprite.rect.yMax - frameHeight; rFrame.y >= _sprite.rect.y; rFrame.y -= frameHeight)
            {
                for (rFrame.x = _sprite.rect.x; rFrame.x < _sprite.rect.xMax; rFrame.x += frameWidth, ++frameNb)
                {
                    try
                    {
                        Sprite sprFrame = Sprite.Create(_sprite.texture, rFrame, new Vector2(0.5f, 0.5f), 100f);
                        sprFrame.name = _sprite.name + "_" + frameNb;
                        result.Add(sprFrame);
                        sprFrame.texture.filterMode = FilterMode.Bilinear;
                        sprFrame.texture.Apply();
                        if (frameNb == _frameNum)
                            break;
                    }
                    catch
                    {
                        //NOTE: this happens when texture size is not multiple of AnimFrames. In this case, wrong frames are skipped
                        frameNb--;
                    }
                }

                if (frameNb == _frameNum)
                    break;
            }

            return result;
        }

        public static Vector3 GetLineHitPlanePoint(Vector3 _planeVector, Vector3 _planePoint, Vector3 _lineVector, Vector3 _linePoint)
        {
            Vector3 result = new Vector3();

            float vp1, vp2, vp3, n1, n2, n3, v1, v2, v3, m1, m2, m3, vpt;

            vp1 = _planeVector.x;
            vp2 = _planeVector.y;
            vp3 = _planeVector.z;
            n1 = _planePoint.x;
            n2 = _planePoint.y;
            n3 = _planePoint.z;
            v1 = _lineVector.x;
            v2 = _lineVector.y;
            v3 = _lineVector.z;
            m1 = _linePoint.x;
            m2 = _linePoint.y;
            m3 = _linePoint.z;

            vpt = v1 * vp1 + v2 * vp2 + v3 * vp3;

            //首先判断直线是否与平面平行  
            if (vpt != 0)
            {
                float t = ((n1 - m1) * vp1 + (n2 - m2) * vp2 + (n3 - m3) * vp3) / vpt;
                float x = m1 + v1 * t;
                float y = m2 + v2 * t;
                float z = m3 + v3 * t;
                result = new Vector3(x, y, z);
            }

            return result;
        }

        public static void SaveRenderTextureToPNG(int _texWidth, int _texHeight, RenderTexture _rt, Rect _sourceRect, int _destX, int _destY, string _path)
        {
            Texture2D png = RenderTextureToTexture2D(_texWidth, _texHeight, _rt, _sourceRect, _destX, _destY);

            byte[] bytes = png.EncodeToPNG();

            FileInfo fi = new FileInfo(_path);

            if (fi.Exists)
            {
                fi.Delete();
            }

            using (FileStream fs = File.Open(_path, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(bytes);

                    UnityEngine.Object.Destroy(png);
                }
            }

        }

        public static void CaptureScreen(RenderTexture _rt, Camera[] _cameraArr, int[] _cameraCullingMaskArr)
        {
            for (int i = 0; i < _cameraArr.Length; i++)
            {
                Camera camera = _cameraArr[i];



                RenderTexture rt = camera.targetTexture;

                int cullingMask = camera.cullingMask;


                camera.targetTexture = _rt;

                camera.cullingMask = _cameraCullingMaskArr[i];


                camera.Render();


                camera.targetTexture = rt;

                camera.cullingMask = cullingMask;
            }
        }

        public static Texture2D RenderTextureToTexture2D(int _texWidth, int _texHeight, RenderTexture _rt, Rect _sourceRect, int _destX, int _destY)
        {
            RenderTexture prev = RenderTexture.active;

            RenderTexture.active = _rt;

            Texture2D png = new Texture2D(_texWidth, _texHeight, TextureFormat.ARGB32, false);

            png.ReadPixels(_sourceRect, _destX, _destY);

            RenderTexture.active = prev;

            return png;
        }

        public static void AtlasGetOriginalUV(Image _img)
        {
            Vector4 fix;

            Vector4 fix2;

            if (_img.sprite.packed)
            {
                fix = new Vector4(_img.sprite.textureRect.x / _img.sprite.texture.width, _img.sprite.textureRect.y / _img.sprite.texture.height, _img.sprite.textureRect.width / _img.sprite.texture.width, _img.sprite.textureRect.height / _img.sprite.texture.height);

                fix2 = new Vector4(_img.sprite.textureRectOffset.x / _img.sprite.rect.width, _img.sprite.textureRectOffset.y / _img.sprite.rect.height, _img.sprite.textureRect.width / _img.sprite.rect.width, _img.sprite.textureRect.height / _img.sprite.rect.height);
            }
            else
            {
                fix = new Vector4(0, 0, 1, 1);

                fix2 = new Vector4(0, 0, 1, 1);
            }

            _img.material.SetVector("fix", fix);

            _img.material.SetVector("fix2", fix2);
        }
    }
}

