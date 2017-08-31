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

    private int verticalTweenID = -1;

    private int horizontalTweenID = -1;

    protected override void Awake()
    {
        base.Awake();

        inertia = false;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (verticalTweenID != -1)
        {
            SuperTween.Instance.Remove(verticalTweenID);

            verticalTweenID = -1;
        }

        if (horizontalTweenID != -1)
        {
            SuperTween.Instance.Remove(horizontalTweenID);

            horizontalTweenID = -1;
        }
    }

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
                        SetVerticalPosIndex(i, false);

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
                        SetHorizontalPosIndex(i, false);

                        break;
                    }
                }
            }
        }
    }

    public void SetVerticalPosIndex(int _index, bool _instant)
    {
        if (_instant)
        {
            SetVerticalPos(_index * verticalStep);
        }
        else
        {
            verticalTweenID = SuperTween.Instance.To(verticalNormalizedPosition, _index * verticalStep, Mathf.Abs(_index * verticalStep - verticalNormalizedPosition) * speedFix, SetVerticalPos, VerticalTweenOver);
        }
    }

    public void SetHorizontalPosIndex(int _index, bool _instant)
    {
        if (_instant)
        {
            SetHorizontalPos(_index * horizontalStep);
        }
        else
        {
            horizontalTweenID = SuperTween.Instance.To(horizontalNormalizedPosition, _index * horizontalHalfStep, Mathf.Abs(_index * horizontalHalfStep - horizontalNormalizedPosition) * speedFix, SetHorizontalPos, HorizontalTweenOver);
        }
    }

    private void SetVerticalPos(float _value)
    {
        verticalNormalizedPosition = _value;
    }

    private void SetHorizontalPos(float _value)
    {
        horizontalNormalizedPosition = _value;
    }

    private void VerticalTweenOver()
    {
        verticalTweenID = -1;
    }

    private void HorizontalTweenOver()
    {
        horizontalTweenID = -1;
    }

    public void SetVerticalNum(int _verticalNum)
    {
        verticalNum = _verticalNum;

        if (verticalNum > 1)
        {
            verticalStep = 1f / (verticalNum - 1);
        }
        else
        {
            verticalStep = 0;
        }

        verticalHalfStep = verticalStep * 0.5f;
    }

    public void SetHorizontalNum(int _horizontalNum)
    {
        horizontalNum = _horizontalNum;

        if (horizontalNum > 1)
        {
            horizontalStep = 1f / (horizontalNum - 1);
        }
        else
        {
            horizontalStep = 0;
        }

        horizontalHalfStep = horizontalStep * 0.5f;
    }

    public void SetSpeedFix(float _speedFix)
    {
        speedFix = _speedFix;
    }
}
