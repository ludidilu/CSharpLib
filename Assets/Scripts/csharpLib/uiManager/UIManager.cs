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

    private LinkedList<List<UIBase>> stack = new LinkedList<List<UIBase>>();

    private List<UIBase> tmpList = new List<UIBase>();

    private Dictionary<UIView, int> tmpDic = new Dictionary<UIView, int>();

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

    public int ShowInRoot<T>(object _data, int _layerIndex) where T : UIView
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
        IEnumerator<List<UIBase>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIBase> list = enumerator.Current;

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
                view.gameObject.SetActive(true);

                ShowReal(view, _parent, _data, _layerIndex, tmpUid);
            }
        }
        else
        {
            Action<GameObject> dele = delegate (GameObject _go)
            {
                _go.SetActive(true);

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
    }

    private void AddUI(UIBase _ui, UIBase _parent, int _layerIndex)
    {
        _ui.parent = _parent;

        if (_parent != null)
        {
            _parent.children.Add(_ui);

            _ui.layerIndex = _parent.layerIndex;
        }
        else
        {
            _ui.layerIndex = _layerIndex;

            LinkedListNode<List<UIBase>> node = stack.First;

            List<UIBase> list;

            if (node == null)
            {
                list = new List<UIBase>();

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
                        list = new List<UIBase>();

                        stack.AddBefore(node, list);

                        break;
                    }
                    else
                    {
                        node = node.Next;

                        if (node == null)
                        {
                            list = new List<UIBase>();

                            stack.AddLast(list);

                            break;
                        }
                    }
                }
            }

            list.Add(_ui);
        }
    }

    public void Hide(int _uid)
    {
        UIBase ui = GetUi(_uid);

        if (ui.parent == null)
        {
            LinkedListNode<List<UIBase>> node = stack.First;

            while (true)
            {
                List<UIBase> list = node.Value;

                if (list[0].layerIndex == ui.layerIndex)
                {
                    list.Remove(ui);

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
            ui.parent.children.Remove(ui);
        }

        RemoveUI(ui);

        SortView();
    }

    private void RemoveUI(UIBase _ui)
    {
        for (int i = 0; i < _ui.children.Count; i++)
        {
            RemoveUI(_ui.children[i]);
        }

        if (_ui is UIView)
        {
            UIView viwe = _ui as UIView;

            viwe.parent = null;

            viwe.gameObject.SetActive(false);

            viwe.SetVisible(false);

            viwe.children.Clear();
        }
        else
        {
            UnityEngine.Object.Destroy(_ui);
        }
    }

    private void SortView()
    {
        GetUiList();

        for (int i = tmpList.Count - 1; i > -1; i--)
        {
            UIBase ui = tmpList[i];

            if (ui is UIBlock)
            {
                UIBlock block = ui as UIBlock;

                if (!tmpDic.ContainsKey(block.origin))
                {
                    if (block.origin.gameObject.activeSelf)
                    {
                        int index = tmpList.IndexOf(block.origin);

                        tmpList[index] = block;
                    }

                    tmpList[i] = block.origin;

                    block.Replace(stack);

                    tmpDic.Add(block.origin, i);
                }
            }
            else
            {
                UIView view = ui as UIView;

                tmpDic.Add(view, i);
            }
        }

        for (int i = 0; i < tmpList.Count; i++)
        {
            UIBase ui = tmpList[i];

            if (ui is UIView)
            {
                UIView view = ui as UIView;

                view.transform.SetAsLastSibling();
            }
        }

        //List<UIBase> uiList = new List<UIBase>();

        //IEnumerator<List<UIBase>> enumerator = stack.GetEnumerator();

        //while (enumerator.MoveNext())
        //{
        //    List<UIBase> list = enumerator.Current;

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        SortViewReal(list[i], uiList);
        //    }
        //}

        bool show = true;

        bool showMask = false;

        for (int i = tmpList.Count - 1; i > -1; i--)
        {
            UIBase ui = tmpList[i];

            if (ui is UIBlock)
            {

            }
            else
            {
                UIView view = ui as UIView;

                if (show)
                {
                    view.SetVisible(true);

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
                else
                {
                    view.SetVisible(false);
                }
            }
        }

        if (!showMask && mask != null)
        {
            mask.gameObject.SetActive(false);
        }

        tmpList.Clear();

        tmpDic.Clear();
    }

    private void GetUiList()
    {
        IEnumerator<List<UIBase>> enumerator = stack.GetEnumerator();

        while (enumerator.MoveNext())
        {
            List<UIBase> list = enumerator.Current;

            for (int i = 0; i < list.Count; i++)
            {
                GetUiListReal(list[i]);
            }
        }
    }

    private void GetUiListReal(UIBase _ui)
    {
        tmpList.Add(_ui);

        for (int i = 0; i < _ui.children.Count; i++)
        {
            GetUiListReal(_ui.children[i]);
        }
    }

    private void SortViewReal(UIBase _ui, List<UIBase> _viewList)
    {
        if (_ui is UIView)
        {
            _ui.transform.SetAsLastSibling();
        }

        _viewList.Add(_ui as UIView);

        for (int i = 0; i < _ui.children.Count; i++)
        {
            SortViewReal(_ui.children[i], _viewList);
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

                        IEnumerator<List<UIBase>> enumerator = stack.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current[0].layerIndex == ui0.layerIndex)
                            {
                                list = enumerator.Current;

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
