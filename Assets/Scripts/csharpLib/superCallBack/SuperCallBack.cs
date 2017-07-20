using System;

public interface ISuperCallBack0
{
    void Call();
}

public interface ISuperCallBack1<T>
{
    void Call(T _t);
}

public class SuperCallBack0 : ISuperCallBack0
{
    private Action callBack;

    public SuperCallBack0(Action _callBack)
    {
        callBack = _callBack;
    }

    public void Call()
    {
        callBack();
    }
}

public class SuperCallBack0<T1> : ISuperCallBack0
{
    private Action<T1> callBack;

    private T1 t1;

    public SuperCallBack0(Action<T1> _callBack, T1 _t1)
    {
        callBack = _callBack;
        t1 = _t1;
    }

    public void Call()
    {
        callBack(t1);
    }
}

public class SuperCallBack0<T1, T2> : ISuperCallBack0
{
    private Action<T1, T2> callBack;

    private T1 t1;

    private T2 t2;

    public SuperCallBack0(Action<T1, T2> _callBack, T1 _t1, T2 _t2)
    {
        callBack = _callBack;
        t1 = _t1;
        t2 = _t2;
    }

    public void Call()
    {
        callBack(t1, t2);
    }
}

public class SuperCallBack0<T1, T2, T3> : ISuperCallBack0
{
    private Action<T1, T2, T3> callBack;

    private T1 t1;

    private T2 t2;

    private T3 t3;

    public SuperCallBack0(Action<T1, T2, T3> _callBack, T1 _t1, T2 _t2, T3 _t3)
    {
        callBack = _callBack;
        t1 = _t1;
        t2 = _t2;
        t3 = _t3;
    }

    public void Call()
    {
        callBack(t1, t2, t3);
    }
}

public class SuperCallBack0<T1, T2, T3, T4> : ISuperCallBack0
{
    private Action<T1, T2, T3, T4> callBack;

    private T1 t1;

    private T2 t2;

    private T3 t3;

    private T4 t4;

    public SuperCallBack0(Action<T1, T2, T3, T4> _callBack, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
    {
        callBack = _callBack;
        t1 = _t1;
        t2 = _t2;
        t3 = _t3;
        t4 = _t4;
    }

    public void Call()
    {
        callBack(t1, t2, t3, t4);
    }
}

public class SuperCallBack1<T> : ISuperCallBack1<T>
{
    private Action<T> callBack;

    public SuperCallBack1(Action<T> _callBack)
    {
        callBack = _callBack;
    }

    public void Call(T _t)
    {
        callBack(_t);
    }
}

public class SuperCallBack1<T, T1> : ISuperCallBack1<T>
{
    private Action<T, T1> callBack;

    private T1 t1;

    public SuperCallBack1(Action<T, T1> _callBack, T1 _t1)
    {
        callBack = _callBack;
        t1 = _t1;
    }

    public void Call(T _t)
    {
        callBack(_t, t1);
    }
}

public class SuperCallBack1<T, T1, T2> : ISuperCallBack1<T>
{
    private Action<T, T1, T2> callBack;

    private T1 t1;

    private T2 t2;

    public SuperCallBack1(Action<T, T1, T2> _callBack, T1 _t1, T2 _t2)
    {
        callBack = _callBack;
        t1 = _t1;
        t2 = _t2;
    }

    public void Call(T _t)
    {
        callBack(_t, t1, t2);
    }
}

public class SuperCallBack1<T, T1, T2, T3> : ISuperCallBack1<T>
{
    private Action<T, T1, T2, T3> callBack;

    private T1 t1;

    private T2 t2;

    private T3 t3;

    public SuperCallBack1(Action<T, T1, T2, T3> _callBack, T1 _t1, T2 _t2, T3 _t3)
    {
        callBack = _callBack;
        t1 = _t1;
        t2 = _t2;
        t3 = _t3;
    }

    public void Call(T _t)
    {
        callBack(_t, t1, t2, t3);
    }
}