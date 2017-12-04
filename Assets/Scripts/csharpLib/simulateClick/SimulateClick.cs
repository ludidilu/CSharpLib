using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class SimulateClick
{
    private static List<RaycastResult> raycastResultList = new List<RaycastResult>();

    private static PointerEventData clickEventData;

    public static void Click(GraphicRaycaster _graphicRaycaster, Camera _camera, UnityEngine.Object _obj)
    {
        if (clickEventData == null)
        {
            clickEventData = new PointerEventData(EventSystem.current);
        }

        Transform target;

        if (_obj is GameObject)
        {
            target = (_obj as GameObject).transform;
        }
        else if (_obj is Component)
        {
            target = (_obj as Component).gameObject.transform;
        }
        else
        {
            throw new Exception("SimulateClick error!Unknown obj type:" + _obj);
        }

        clickEventData.position = _camera.WorldToScreenPoint(target.position);

        ClickReal(_graphicRaycaster);
    }

    public static void Click(GraphicRaycaster _graphicRaycaster, Vector3 _pos)
    {
        if (clickEventData == null)
        {
            clickEventData = new PointerEventData(EventSystem.current);
        }

        clickEventData.position = _pos;

        ClickReal(_graphicRaycaster);
    }

    private static void ClickReal(GraphicRaycaster _graphicRaycaster)
    {
        _graphicRaycaster.Raycast(clickEventData, raycastResultList);

        if (raycastResultList.Count > 0)
        {
            GameObject go = raycastResultList[0].gameObject;

            ExecuteClick(go);

            raycastResultList.Clear();
        }
    }

    private static void ExecuteClick(GameObject _go)
    {
        bool b = ExecuteEvents.Execute(_go, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

        if (!b)
        {
            if (_go.transform.parent != null)
            {
                ExecuteClick(_go.transform.parent.gameObject);
            }
        }
    }
}
