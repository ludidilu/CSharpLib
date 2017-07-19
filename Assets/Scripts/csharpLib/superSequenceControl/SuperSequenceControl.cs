using System.Collections;
using System;
using System.Collections.Generic;
using superTween;

namespace superSequenceControl
{
    public class SuperSequenceControl
    {
        public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8, T9 _t9);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8, T9 _t9, T10 _t10);

        public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8, T9 _t9, T10 _t10, T11 _t11);

        private static Dictionary<int, IEnumerator> dic = new Dictionary<int, IEnumerator>();

        private static int index = 0;

        private static int GetIndex()
        {
            index++;

            return index;
        }

        public static void Start(Func<int, IEnumerator> _del)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1>(Func<int, T1, IEnumerator> _del, T1 _t1)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2>(Func<int, T1, T2, IEnumerator> _del, T1 _t1, T2 _t2)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3>(Func<int, T1, T2, T3, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4>(Func<int, T1, T2, T3, T4, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5>(Func<int, T1, T2, T3, T4, T5, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5, T6>(Func<int, T1, T2, T3, T4, T5, T6, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5, _t6);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5, T6, T7>(Func<int, T1, T2, T3, T4, T5, T6, T7, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5, _t6, _t7);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5, T6, T7, T8>(Func<int, T1, T2, T3, T4, T5, T6, T7, T8, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<int, T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8, T9 _t9)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8, _t9);

            InitIEnumerator(ie, tmpIndex);
        }

        public static void Start<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<int, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerator> _del, T1 _t1, T2 _t2, T3 _t3, T4 _t4, T5 _t5, T6 _t6, T7 _t7, T8 _t8, T9 _t9, T10 _t10)
        {
            int tmpIndex = GetIndex();

            IEnumerator ie = _del(tmpIndex, _t1, _t2, _t3, _t4, _t5, _t6, _t7, _t8, _t9, _t10);

            InitIEnumerator(ie, tmpIndex);
        }

        private static void InitIEnumerator(IEnumerator _ie, int _index)
        {
            SuperTween.Instance.NextFrameCall(InitEnumeratorReal, _ie, _index);
        }

        private static void InitEnumeratorReal(IEnumerator _ie, int _index)
        {
            if (_ie.MoveNext())
            {
                dic.Add(_index, _ie);
            }
        }

        public static void MoveNext(int _index)
        {
            IEnumerator ie;

            if (dic.TryGetValue(_index, out ie))
            {
                if (!ie.MoveNext())
                {
                    dic.Remove(_index);
                }
            }
        }

        public static void Break(int _index)
        {
            dic.Remove(_index);
        }

        public static void To(float _start, float _end, float _time, Action<float> _del, int _index)
        {
            if (_index != 0)
            {
                SuperTween.Instance.To(_start, _end, _time, _del, false, MoveNext, _index);
            }
            else
            {
                SuperTween.Instance.To(_start, _end, _time, _del, false);
            }
        }

        public static void DelayCall(float _time, int _index)
        {
            SuperTween.Instance.DelayCall(_time, false, MoveNext, _index);
        }
    }
}