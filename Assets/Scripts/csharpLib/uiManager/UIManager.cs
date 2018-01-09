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

    private LinkedList<KeyValuePair<int, List<UIBase>>> stack = new LinkedList<KeyValuePair<int, List<UIBase>>>();

    //private List<UIBase> stack = new List<UIBase>();

    private GameObject blockGo;

    private int uid = 1;

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

    public int ShowInLayer<T>(object _data, int _layerIndex) where T : UIView
    {
        return Show<T>(_data, _layerIndex, null);
    }

    public int ShowInParent<T>(object _data, int _parentUid) where T : UIView
    {
        UIBase parent = GetUi(_parentUid);

        if (parent != null)
        {
            return Show<T>(_data, 0, parent);
        }
        else
        {
            return 0;
        }
    }

    private UIBase GetUi(int _uid)
    {
        IEnumerator<KeyValuePair<int, List<UIBase>>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIBase> list = enumerator.Current.Value;

            for (int i = 0; i < list.Count; i++)
            {
                UIBase ui = list[i];

                if (ui.uid == _uid)
                {
                    return ui;
                }
            }
        }

        return null;
    }

    private int Show<T>(object _data, int _layerIndex, UIBase _parent) where T : UIView
    {
        int tmpUid = uid;

        uid++;

        Type type = typeof(T);

        UIView view;

        if (pool.TryGetValue(type, out view))
        {
            if (view.gameObject.activeSelf)
            {
                UIBlock block = blockGo.AddComponent<UIBlock>();

                block.origin = view;

                ShowReal(block, _parent, _data, _layerIndex, tmpUid);
            }
            else
            {
                ShowReal(view, _parent, _data, _layerIndex, tmpUid);
            }
        }
        else
        {
            Action<GameObject> dele = delegate (GameObject _go)
            {
                _go.transform.SetParent(root, false);

                view = _go.GetComponent<T>();

                if (view == null)
                {
                    view = _go.AddComponent<T>();
                }

                pool.Add(typeof(T), view);

                view.Init();

                ShowReal(view, _parent, _data, _layerIndex, tmpUid);
            };

            getAssetCallBack(type, dele);
        }

        return tmpUid;
    }

    private void ShowReal(UIBase _ui, UIBase _parent, object _data, int _layerIndex, int _uid)
    {
        _ui.uid = _uid;

        _ui.data = _data;

        AddUI(_ui, _parent, _layerIndex);

        SortView();

        RefreshMask();

        //_view.OnEnter();
    }

    private void AddUI(UIBase _ui, UIBase _parent, int _layerIndex)
    {
        if (_parent != null)
        {
            _parent.children.Add(_ui);

            _ui.parent = _parent;

            _ui.layerIndex = _parent.layerIndex;
        }
        else
        {
            _ui.layerIndex = _layerIndex;

            LinkedListNode<KeyValuePair<int, List<UIBase>>> node = stack.First;

            List<UIBase> list;

            if (node == null)
            {
                list = new List<UIBase>();

                KeyValuePair<int, List<UIBase>> pair = new KeyValuePair<int, List<UIBase>>(_layerIndex, list);

                stack.AddFirst(pair);
            }
            else
            {
                while (true)
                {
                    if (_layerIndex == node.Value.Key)
                    {
                        list = node.Value.Value;

                        break;
                    }
                    else if (_layerIndex < node.Value.Key)
                    {
                        list = new List<UIBase>();

                        KeyValuePair<int, List<UIBase>> pair = new KeyValuePair<int, List<UIBase>>(_layerIndex, list);

                        stack.AddBefore(node, pair);

                        break;
                    }
                    else
                    {
                        node = node.Next;

                        if (node == null)
                        {
                            list = new List<UIBase>();

                            KeyValuePair<int, List<UIBase>> pair = new KeyValuePair<int, List<UIBase>>(_layerIndex, list);

                            stack.AddLast(pair);

                            break;
                        }
                    }
                }
            }

            list.Add(_ui);
        }

        if (_ui is UIBlock)
        {
            UIBlock block = _ui as UIBlock;

            if (Compare(block, block.origin) == 1)
            {
                block.Replace(block.origin);
            }
        }
    }

    public void Hide(int _uid)
    {
        IEnumerator<KeyValuePair<int, List<UIBase>>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIBase> list = enumerator.Current.Value;

            for (int i = 0; i < list.Count; i++)
            {
                UIBase ui = list[i];

                if (ui.uid == _uid)
                {
                    HideReal(ui);

                    return;
                }
            }
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
            LinkedListNode<KeyValuePair<int, List<UIBase>>> node = stack.Last;

            bool over = false;

            while (!over && node != null)
            {
                List<UIBase> list = node.Value.Value;

                for (int i = list.Count - 1; i > -1; i--)
                {
                    UIBase ui = list[i];

                    if (ui is UIView)
                    {
                        UIView view = ui as UIView;

                        view.SetVisible(true);

                        if (view.IsFullScreen())
                        {
                            over = true;

                            break;
                        }
                    }
                    else
                    {
                        UIBlock block = ui as UIBlock;

                        throw new Exception("error:3214");
                    }
                }

                if (!over)
                {
                    node = node.Previous;
                }
            }
        }

        RemoveUI(_ui);

        SortView();

        RefreshMask();
    }

    private void RemoveUI(UIBase _ui)
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

            view.OnExit();

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

                        UnityEngine.Object.Destroy(tmpBlock);

                        replaceBlock = true;

                        break;
                    }
                }
            }

            if (!replaceBlock)
            {
                view.gameObject.SetActive(false);
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
                RemoveUI(children[i]);
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

    private int Compare(UIBase _ui0, UIBase _ui1)
    {
        if (_ui0.layerIndex > _ui1.layerIndex)
        {
            return 1;
        }
        else if (_ui0.layerIndex < _ui1.layerIndex)
        {
            return -1;
        }
        else
        {
            List<UIBase> chain0 = _ui0.chain;

            List<UIBase> chain1 = _ui1.chain;

            int index = 0;

            while (true)
            {
                UIBase ui0 = chain0[chain0.Count - 1 - index];

                UIBase ui1 = chain1[chain1.Count - 1 - index];

                if (ui0 != ui1)
                {
                    if (index == 0)
                    {
                        List<UIBase> list = null;

                        IEnumerator<KeyValuePair<int, List<UIBase>>> enumerator = stack.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<int, List<UIBase>> pair = enumerator.Current;

                            if (pair.Key == ui0.layerIndex)
                            {
                                list = pair.Value;

                                break;
                            }
                        }

                        int index0 = list.IndexOf(_ui0);

                        int index1 = list.IndexOf(_ui1);

                        if (index0 > index1)
                        {
                            return 1;
                        }
                        else if (index0 < index1)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        int index0 = ui0.parent.children.IndexOf(ui0);

                        int index1 = ui1.parent.children.IndexOf(ui1);

                        if (index0 > index1)
                        {
                            return 1;
                        }
                        else if (index0 < index1)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    index++;

                    if (index == chain0.Count && index == chain1.Count)
                    {
                        return 0;
                    }
                    else if (index == chain0.Count)
                    {
                        return -1;
                    }
                    else if (index == chain1.Count)
                    {
                        return 1;
                    }
                }
            }
        }
    }
}
