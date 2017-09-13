using UnityEngine;
using UnityEngine.UI;

public class TextHelper
{
    private TextGenerator tg;

    private TextGenerationSettings tgs;

    // Use this for initialization
    public void Init(Text _text)
    {
        tg = new TextGenerator();

        tgs = new TextGenerationSettings();

        tgs.font = _text.font;

        tgs.fontSize = _text.fontSize;

        tgs.fontStyle = _text.fontStyle;

        tgs.color = _text.color;

        tgs.lineSpacing = _text.lineSpacing;

        tgs.verticalOverflow = _text.verticalOverflow;

        tgs.horizontalOverflow = _text.horizontalOverflow;

        tgs.richText = _text.supportRichText;

        tgs.resizeTextForBestFit = _text.resizeTextForBestFit;

        tgs.resizeTextMaxSize = _text.resizeTextMaxSize;

        tgs.resizeTextMinSize = _text.resizeTextMinSize;

        tgs.textAnchor = _text.alignment;

        tgs.scaleFactor = _text.GetComponentInParent<Canvas>().scaleFactor;

        Debug.Log("tgs.scaleFactor:" + tgs.scaleFactor);

        tgs.generationExtents = _text.rectTransform.rect.size;

        Debug.Log("tgs.generationExtents:" + tgs.generationExtents);
    }

    public float GetPreferedHeight(string _str)
    {
        return tg.GetPreferredHeight(_str, tgs);
    }

    public float GetPreferedWidth(string _str)
    {
        return tg.GetPreferredWidth(_str, tgs);
    }
}
