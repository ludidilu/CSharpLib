using System;
using System.Collections.Generic;

public class QueueAction
{
    private static QueueAction _Instance;

    public static QueueAction Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new QueueAction();
            }

            return _Instance;
        }
    }

    private Queue<Action> queue = new Queue<Action>();

    private bool isAction = false;

    public void Add(Action _action)
    {
        if (isAction)
        {
            queue.Enqueue(_action);
        }
        else
        {
            isAction = true;

            _action();
        }
    }

    public void Over()
    {
        if (!isAction)
        {
            throw new Exception("QueueAction error0");
        }

        if (queue.Count > 0)
        {
            Action action = queue.Dequeue();

            action();
        }
        else
        {
            isAction = false;
        }
    }
}
