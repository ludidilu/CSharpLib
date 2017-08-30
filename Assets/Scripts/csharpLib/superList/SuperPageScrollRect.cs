using UnityEngine;
using UnityEngine.EventSystems;
using superTween;
using superList;

public class SuperPageScrollRect : SuperScrollRect
{
    private int verticalNum;

    private int horizontalNum;

    private float speedFix;

    private float verticalStep;

    private float verticalHalfStep;

    private float horizontalStep;

    private float horizontalHalfStep;

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        StopMovement();

        if (vertical)
        {
            if (verticalNormalizedPosition > 0 && verticalNormalizedPosition < 1)
            {
                for (int i = 0; i < verticalNum; i++)
                {
                    if (verticalNormalizedPosition < i * verticalStep + verticalHalfStep)
                    {
                        SuperTween.Instance.To(verticalNormalizedPosition, i * verticalStep, Mathf.Abs(i * verticalStep - verticalNormalizedPosition) * speedFix, VerticalSetPos, null);

                        break;
                    }
                }
            }
        }

        if (horizontal)
        {
            if (horizontalNormalizedPosition > 0 && horizontalNormalizedPosition < 1)
            {
                for (int i = 0; i < horizontalNum; i++)
                {
                    if (horizontalNormalizedPosition < i * horizontalStep + horizontalHalfStep)
                    {
                        SuperTween.Instance.To(horizontalNormalizedPosition, i * horizontalHalfStep, Mathf.Abs(i * horizontalHalfStep - horizontalNormalizedPosition) * speedFix, HorizontalSetPos, null);

                        break;
                    }
                }
            }
        }
    }

    private void VerticalSetPos(float _value)
    {
        verticalNormalizedPosition = _value;
    }

    private void HorizontalSetPos(float _value)
    {
        horizontalNormalizedPosition = _value;
    }

    public void SetVerticalNum(int _verticalNum)
    {
        verticalNum = _verticalNum;

        verticalStep = 1f / (verticalNum - 1);

        verticalHalfStep = verticalStep * 0.5f;
    }

    public void SetHorizontalNum(int _horizontalNum)
    {
        horizontalNum = _horizontalNum;

        horizontalStep = 1f / (horizontalNum - 1);

        horizontalHalfStep = horizontalStep * 0.5f;
    }

    public void SetSpeedFix(float _speedFix)
    {
        speedFix = _speedFix;
    }
}
