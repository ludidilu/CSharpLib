using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class MeshColorControl : MonoBehaviour
{
    private Color color;

    private List<Color> colorList = new List<Color>();

    private Mesh mesh;

    // Use this for initialization
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            colorList.Add(Color.white);
        }
    }

    public void SetColor(Color _color)
    {
        color = _color;

        for (int i = 0; i < mesh.vertexCount; i++)
        {
            colorList[i] = color;
        }

        mesh.SetColors(colorList);
    }

    public Color GetColor()
    {
        return color;
    }
}
