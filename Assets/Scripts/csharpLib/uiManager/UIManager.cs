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

    private Dictionary<Type, UIView> pool = new Dictionary<Type, UIView>();

    private List<UIBase> stack = new List<UIBase>();

    private GameObject blockGo;

    public void Init(Transform _root, Transform _mask, Action<Type, Action<GameObject>> _getAssetCallBack)
    {
        blockGo = new GameObject();

        root = _root;

        mask = _mask;

        if (_mask != null)
        {
            mask.SetParent(root, false);

            mask.gameObject.SetActive(false);
        }

        getAssetCallBack = _getAssetCallBack;
    }

    public void Show<T>(object _data) where T : UIView
    {
        Type type = typeof(T);

        UIView view;

        if (pool.TryGetValue(type, out view))
        {
            int index = stack.IndexOf(view);

            if (index == -1)
            {
                ShowReal(view, null, _data);
            }
            else
            {
                UIBlock block = blockGo.AddComponent<UIBlock>();

                block.Replace(view);

                stack[index] = block;

                ShowReal(view, null, _data);
            }
        }
        else
        {
            Action<GameObject> dele = delegate (GameObject _go)
            {
                _go.transform.SetParent(root, false);

                view = _go.GetComponent<T>();

                pool.Add(typeof(T), view);

                if (view == null)
                {
                    view = _go.AddComponent<T>();
                }

                view.Init();

                ShowReal(view, null, _data);
            };

            getAssetCallBack(type, dele);
        }
    }

    public void Show<T, U>(object _data) where T : UIView where U : UIView
    {
        UIView parent;

        if (pool.TryGetValue(typeof(U), out parent))
        {
            if (stack.Contains(parent))
            {
                Type type = typeof(T);

                UIView view;

                if (pool.TryGetValue(type, out view))
                {
                    int index = stack.IndexOf(view);

                    if (index == -1)
                    {
                        ShowReal(view, parent, _data);
                    }
                    else
                    {
                        UIBlock block = blockGo.AddComponent<UIBlock>();

                        block.Replace(view);

                        stack[index] = block;

                        ShowReal(view, parent, _data);
                    }
                }
                else
                {
                    Action<GameObject> dele = delegate (GameObject _go)
                    {
                        _go.transform.SetParent(root, false);

                        view = _go.GetComponent<T>();

                        pool.Add(typeof(T), view);

                        if (view == null)
                        {
                            view = _go.AddComponent<T>();
                        }

                        view.Init();

                        ShowReal(view, parent, _data);
                    };

                    getAssetCallBack(type, dele);
                }
            }
        }
    }

    private void ShowReal(UIView _view, UIBase _parent, object _data)
    {
        AddUI(_view, _parent);

        _view.OnEnter(_data);
    }

    private void AddUI(UIView _view, UIBase _parent)
    {
        if (_view.IsFullScreen())
        {
            HideBefore();
        }

        _view.gameObject.SetActive(true);

        _view.SetVisible(true);

        _view.transform.SetAsLastSibling();

        if (_parent != null)
        {
            _parent.children.Add(_view);

            _view.parent = _parent;
        }

        stack.Add(_view);

        RefreshMask();
    }

    private void HideBefore()
    {
        for (int i = stack.Count - 1; i > -1; i--)
        {
            UIBase ui = stack[i];

            if (ui is UIView)
            {
                UIView view = ui as UIView;

                view.SetVisible(false);

                view.OnHide();

                if (view.IsFullScreen())
                {
                    break;
                }
            }
            else
            {
                UIBlock block = ui as UIBlock;

                if (block.origin.IsFullScreen())
                {
                    break;
                }
            }
        }
    }

    public void Hide<T>() where T : UIView
    {
        UIView view;

        if (pool.TryGetValue(typeof(T), out view))
        {
            if (stack.Contains(view))
            {
                HideReal(view);
            }
        }
    }

    public void Hide(UIBase _ui)
    {
        if (stack.Contains(_ui))
        {
            HideReal(_ui);
        }
    }

    private void HideReal(UIBase _ui)
    {
        bool showBefore = false;

        if (_ui is UIView)
        {
            UIView view = _ui as UIView;

            showBefore = view.IsFullScreen();
        }
        else
        {
            UIBlock block = _ui as UIBlock;

            showBefore = block.origin.IsFullScreen();
        }

        if (showBefore)
        {
            int index = stack.IndexOf(_ui);

            for (int i = index - 1; i > -1; i--)
            {
                UIBase ui = stack[i];

                if (ui is UIView)
                {
                    UIView view = ui as UIView;

                    view.SetVisible(true);

                    if (view.IsFullScreen())
                    {
                        break;
                    }
                }
                else
                {
                    UIBlock block = ui as UIBlock;

                    if (block.origin.IsFullScreen())
                    {
                        break;
                    }
                }
            }
        }

        bool needSortView = false;

        RemoveUI(_ui, ref needSortView);

        if (needSortView)
        {
            SortView();
        }

        RefreshMask();
    }

    private void RemoveUI(UIBase _ui, ref bool _needSortView)
    {
        stack.Remove(_ui);

        if (_ui.parent != null)
        {
            _ui.parent.children.Remove(_ui);

            _ui.parent = null;
        }

        List<UIBase> children = null;

        if (_ui.children.Count > 0)
        {
            children = new List<UIBase>();

            for (int i = 0; i < _ui.children.Count; i++)
            {
                children.Add(_ui.children[i]);
            }
        }

        if (_ui is UIView)
        {
            UIView view = _ui as UIView;

            bool replaceBlock = false;

            for (int i = stack.Count - 1; i > -1; i--)
            {
                UIBase tmpUI = stack[i];

                if (tmpUI is UIBlock)
                {
                    UIBlock tmpBlock = tmpUI as UIBlock;

                    if (tmpBlock.origin == view)
                    {
                        stack[i] = view;

                        tmpBlock.Revert(view);

                        view.OnEnter(tmpBlock.data);

                        UnityEngine.Object.Destroy(tmpBlock);

                        replaceBlock = true;

                        _needSortView = true;

                        break;
                    }
                }
            }

            if (!replaceBlock)
            {
                view.gameObject.SetActive(false);

                view.OnExit();
            }
        }
        else
        {
            UIBlock block = _ui as UIBlock;

            UnityEngine.Object.Destroy(block);
        }

        if (children != null)
        {
            for (int i = 0; i < children.Count; i++)
            {
                RemoveUI(children[i], ref _needSortView);
            }
        }
    }

    private void SortView()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            UIBase ui = stack[i];

            if (ui is UIView)
            {
                ui.transform.SetAsLastSibling();
            }
        }
    }

    private void RefreshMask()
    {
        if (mask != null)
        {
            if (stack.Count > 0)
            {
                UIBase ui = stack[stack.Count - 1];

                UIView view;

                if (ui is UIView)
                {
                    view = ui as UIView;
                }
                else
                {
                    view = (ui as UIBlock).origin;
                }

                if (!view.IsFullScreen())
                {
                    if (!mask.gameObject.activeSelf)
                    {
                        mask.gameObject.SetActive(true);
                    }

                    mask.SetAsLastSibling();

                    view.transform.SetAsLastSibling();
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
