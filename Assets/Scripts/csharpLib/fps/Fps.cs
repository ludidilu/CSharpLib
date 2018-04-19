using UnityEngine;

public class Fps : MonoBehaviour
{
    private GUIStyle gs;

    private int num;

    private float dt;

    private string text;

    void Awake()
    {
        gs = new GUIStyle();

        gs.fontSize = 20;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 50, 50), text, gs);
    }

    void Update()
    {
        num++;

        dt += Time.unscaledDeltaTime;

        if (dt > 1)
        {
            text = num.ToString();

            num = 0;

            dt -= 1;
        }
    }
}
