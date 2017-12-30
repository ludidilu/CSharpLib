using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager _Instance;

    public static UIManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new UIManager();
            }

            return _Instance;
        }
    }

    private Action<Type, Action<GameObject>> getAssetCallBack;

    private Transform root;

    private Transform mask;

    private Dictionary<Type, Queue<UIBase>> pool = new Dictionary<Type, Queue<UIBase>>();

    private List<UIBase> stack = new List<UIBase>();

    public void Init(Transform _root, Transform _mask, Action<Type, Action<GameObject>> _getAssetCallBack)
    {
        root = _root;

        mask = _mask;

        if (_mask != null)
        {
            mask.SetParent(root, false);

            mask.gameObject.SetActive(false);
        }

        getAssetCallBack = _getAssetCallBack;
    }

    public void Show<T>() where T : UIBase
    {
        Type type = typeof(T);

        Queue<UIBase> queue;

        if (pool.TryGetValue(type, out queue))
        {
            if (queue.Count > 0)
            {
                UIBase ui = queue.Dequeue();

                ShowReal(ui);

                return;
            }
        }
        else
        {
            pool.Add(type, new Queue<UIBase>());
        }

        Action<GameObject> dele = delegate (GameObject _go)
        {
            _go.transform.SetParent(root, false);

            T ui = _go.GetComponent<T>();

            if (ui == null)
            {
                ui = _go.AddComponent<T>();
            }

            ui.Init();

            ShowReal(ui);
        };

        getAssetCallBack(type, dele);
    }

    public void Show<T>(object _data) where T : UIBase
    {
        Type type = typeof(T);

        Queue<UIBase> queue;

        if (pool.TryGetValue(type, out queue))
        {
            if (queue.Count > 0)
            {
                UIBase ui = queue.Dequeue();

                ShowReal(ui, _data);

                return;
            }
        }
        else
        {
            pool.Add(type, new Queue<UIBase>());
        }

        Action<GameObject> dele = delegate (GameObject _go)
        {
            _go.transform.SetParent(root, false);

            T ui = _go.GetComponent<T>();

            if (ui == null)
            {
                ui = _go.AddComponent<T>();
            }

            ui.Init();

            ShowReal(ui, _data);
        };

        getAssetCallBack(type, dele);
    }

    private void ShowReal(UIBase _ui)
    {
        AddUI(_ui);

        _ui.OnEnter();
    }

    private void ShowReal(UIBase _ui, object _data)
    {
        AddUI(_ui);

        _ui.OnEnter(_data);
    }

    private void AddUI(UIBase _ui)
    {
        if (_ui.IsFullScreen())
        {
            HideAll();
        }

        _ui.gameObject.SetActive(true);

        _ui.cg.alpha = 1;

        _ui.cg.blocksRaycasts = true;

        _ui.transform.SetAsLastSibling();

        stack.Add(_ui);

        RefreshMask();
    }

    private void HideAll()
    {
        for (int i = stack.Count - 1; i > -1; i--)
        {
            UIBase ui = stack[i];

            ui.cg.alpha = 0;

            ui.cg.blocksRaycasts = false;

            ui.OnHide();

            if (ui.IsFullScreen())
            {
                break;
            }
        }
    }

    public void Hide<T>() where T : UIBase
    {
        for (int i = stack.Count - 1; i > -1; i--)
        {
            UIBase ui = stack[i];

            if (ui is T)
            {
                HideReal(i);

                break;
            }
        }
    }

    public void Hide(UIBase _ui)
    {
        int index = stack.LastIndexOf(_ui);

        if (index != -1)
        {
            HideReal(index);
        }
    }

    public void HideUntil<T>() where T : UIBase
    {
        for (int i = stack.Count - 1; i > -1; i--)
        {
            UIBase ui = stack[i];

            if (ui is T)
            {
                if (i < stack.Count - 1)
                {
                    HideReal(i + 1);
                }

                break;
            }
        }
    }

    public void HideUntil(UIBase _ui)
    {
        int index = stack.LastIndexOf(_ui);

        if (index != -1 && index < stack.Count - 1)
        {
            HideReal(index + 1);
        }
    }

    private void HideReal(int _index)
    {
        UIBase tmpUI = stack[_index];

        for (int i = _index; i < stack.Count; i++)
        {
            UIBase ui = stack[i];

            RemoveUI(ui);
        }

        stack.RemoveRange(_index, stack.Count - _index);

        if (tmpUI.IsFullScreen())
        {
            for (int i = stack.Count - 1; i > -1; i--)
            {
                UIBase ui = stack[i];

                ui.cg.alpha = 1;

                ui.cg.blocksRaycasts = true;

                ui.OnShow();

                if (ui.IsFullScreen())
                {
                    break;
                }
            }
        }

        RefreshMask();
    }

    private void RemoveUI(UIBase _ui)
    {
        _ui.gameObject.SetActive(false);

        _ui.OnExit();

        pool[_ui.GetType()].Enqueue(_ui);
    }

    private void RefreshMask()
    {
        if (mask != null)
        {
            if (stack.Count > 0)
            {
                UIBase tmpUI = stack[stack.Count - 1];

                if (!tmpUI.IsFullScreen())
                {
                    if (!mask.gameObject.activeSelf)
                    {
                        mask.gameObject.SetActive(true);
                    }

                    mask.SetAsLastSibling();

                    tmpUI.transform.SetAsLastSibling();
                }
                else
                {
                    if (mask.gameObject.activeSelf)
                    {
                        mask.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (mask.gameObject.activeSelf)
                {
                    mask.gameObject.SetActive(false);
                }
            }
        }
    }
}
