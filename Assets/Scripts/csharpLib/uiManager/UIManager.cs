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

    private Dictionary<Type, Queue<UIView>> pool = new Dictionary<Type, Queue<UIView>>();

    private LinkedList<List<UIView>> stack = new LinkedList<List<UIView>>();

    private List<UIView> tmpList = new List<UIView>();

    private int uid = 0;

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

    public int ShowInRoot<T>(ValueType _data, int _layerIndex) where T : UIView
    {
        return Show<T>(_data, _layerIndex, null);
    }

    public int ShowInParent<T>(ValueType _data, int _parentUid) where T : UIView
    {
        UIView parent = GetView(_parentUid);

        if (parent != null)
        {
            return Show<T>(_data, -1, parent);
        }
        else
        {
            return -1;
        }
    }

    private UIView GetView(int _uid)
    {
        IEnumerator<List<UIView>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIView> list = enumerator.Current;

            for (int i = 0; i < list.Count; i++)
            {
                UIView view = list[i];

                if (view.uid == _uid)
                {
                    return view;
                }
                else
                {
                    UIView result = view.FindChild(_uid);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }
        }

        return null;
    }

    private int Show<T>(ValueType _data, int _layerIndex, UIView _parent) where T : UIView
    {
        int tmpUid = uid;

        uid++;

        Type type = typeof(T);

        Queue<UIView> queue;

        if (pool.TryGetValue(type, out queue))
        {
            if (queue.Count > 0)
            {
                T view = queue.Dequeue() as T;

                view.gameObject.SetActive(true);

                ShowReal(view, _parent, _data, _layerIndex, tmpUid);
            }
            else
            {
                Action<GameObject> dele = delegate (GameObject _go)
                {
                    _go.SetActive(true);

                    _go.transform.SetParent(root, false);

                    T view = _go.GetComponent<T>();

                    if (view == null)
                    {
                        view = _go.AddComponent<T>();
                    }

                    view.Init();

                    ShowReal(view, _parent, _data, _layerIndex, tmpUid);
                };

                getAssetCallBack(type, dele);
            }
        }
        else
        {
            queue = new Queue<UIView>();

            pool.Add(type, queue);

            Action<GameObject> dele = delegate (GameObject _go)
            {
                _go.SetActive(true);

                _go.transform.SetParent(root, false);

                T view = _go.GetComponent<T>();

                if (view == null)
                {
                    view = _go.AddComponent<T>();
                }

                view.Init();

                ShowReal(view, _parent, _data, _layerIndex, tmpUid);
            };

            getAssetCallBack(type, dele);
        }

        return tmpUid;
    }

    private void ShowReal(UIView _view, UIView _parent, ValueType _data, int _layerIndex, int _uid)
    {
        _view.uid = _uid;

        _view.data = _data;

        Action action = null;

        AddView(_view, _parent, _layerIndex, ref action);

        SortView(ref action);

        if (action != null)
        {
            action();
        }
    }

    private void AddView(UIView _view, UIView _parent, int _layerIndex, ref Action _action)
    {
        _view.parent = _parent;

        if (_parent != null)
        {
            _parent.children.Add(_view);

            _view.layerIndex = _parent.layerIndex;
        }
        else
        {
            _view.layerIndex = _layerIndex;

            LinkedListNode<List<UIView>> node = stack.First;

            List<UIView> list;

            if (node == null)
            {
                list = new List<UIView>();

                stack.AddFirst(list);
            }
            else
            {
                while (true)
                {
                    int layerIndex = node.Value[0].layerIndex;

                    if (_layerIndex == layerIndex)
                    {
                        list = node.Value;

                        break;
                    }
                    else if (_layerIndex < layerIndex)
                    {
                        list = new List<UIView>();

                        stack.AddBefore(node, list);

                        break;
                    }
                    else
                    {
                        node = node.Next;

                        if (node == null)
                        {
                            list = new List<UIView>();

                            stack.AddLast(list);

                            break;
                        }
                    }
                }
            }

            list.Add(_view);
        }

        _action += _view.OnEnter;
    }

    public void Hide(int _uid)
    {
        UIView view = GetView(_uid);

        if (view.parent == null)
        {
            LinkedListNode<List<UIView>> node = stack.First;

            while (true)
            {
                List<UIView> list = node.Value;

                if (list[0].layerIndex == view.layerIndex)
                {
                    list.Remove(view);

                    if (list.Count == 0)
                    {
                        stack.Remove(node);
                    }

                    break;
                }
                else
                {
                    node = node.Next;
                }
            }
        }
        else
        {
            view.parent.children.Remove(view);
        }

        Action action = null;

        RemoveView(view, ref action);

        SortView(ref action);

        if (action != null)
        {
            action();
        }
    }

    private void RemoveView(UIView _view, ref Action _action)
    {
        for (int i = 0; i < _view.children.Count; i++)
        {
            RemoveView(_view.children[i], ref _action);
        }

        _view.parent = null;

        _view.gameObject.SetActive(false);

        pool[_view.GetType()].Enqueue(_view);

        _view.children.Clear();

        _view.SetVisible(false, ref _action);

        _action += _view.OnExit;
    }

    private void SortView(ref Action _action)
    {
        GetViewList();

        for (int i = 0; i < tmpList.Count; i++)
        {
            UIView view = tmpList[i];

            view.transform.SetAsLastSibling();
        }

        bool show = true;

        bool showMask = false;

        for (int i = tmpList.Count - 1; i > -1; i--)
        {
            UIView view = tmpList[i];

            view.SetVisible(show, ref _action);

            if (show)
            {
                if (view.IsFullScreen())
                {
                    show = false;
                }
                else
                {
                    if (!showMask && mask != null)
                    {
                        showMask = true;

                        mask.gameObject.SetActive(true);

                        mask.SetAsLastSibling();

                        mask.SetSiblingIndex(view.transform.GetSiblingIndex());
                    }
                }
            }
        }

        if (!showMask && mask != null)
        {
            mask.gameObject.SetActive(false);
        }

        tmpList.Clear();
    }

    private void GetViewList()
    {
        IEnumerator<List<UIView>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIView> list = enumerator.Current;

            for (int i = 0; i < list.Count; i++)
            {
                GetViewListReal(list[i]);
            }
        }
    }

    private void GetViewListReal(UIView _view)
    {
        tmpList.Add(_view);

        for (int i = 0; i < _view.children.Count; i++)
        {
            GetViewListReal(_view.children[i]);
        }
    }
}