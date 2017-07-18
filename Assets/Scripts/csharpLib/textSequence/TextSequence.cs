using UnityEngine;
using superTween;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using effect;

public class TextSequence
{
    private Dictionary<Text, SequenceHandler> m_sequenceDic = new Dictionary<Text, SequenceHandler>();

    private static TextSequence m_Instance;
    public static TextSequence Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new TextSequence();
            return m_Instance;
        }
    }

    class SequenceHandler
    {
        private Text text;
        private TextSequenceEffect effect;
        private string str;
        private Action callBack;
        private Action stepCallBack;

        private int tweenIndex;
        private int currentIndex;

        public SequenceHandler(Text _text, TextSequenceEffect _effect, string _str, Action _callBack, float _oneLetterTime, Action _stepCallBack)
        {
            text = _text;
            effect = _effect;
            str = _str;
            callBack = _callBack;
            stepCallBack = _stepCallBack;

            text.text = str;

            Action<int> dele = delegate (int obj)
            {
                tweenIndex = SuperTween.Instance.To(0, obj, obj * _oneLetterTime, Step, Over);
            };

            _effect.Init(dele);
        }

        private void Step(float value)
        {
            int index = Mathf.RoundToInt(value);

            if (index == currentIndex)
            {
                return;
            }

            currentIndex = index;

            effect.SetShowNum(currentIndex);

            text.text = string.Empty;

            text.text = str;

            if (stepCallBack != null)
            {
                stepCallBack();
            }
        }

        private void Over()
        {
            effect.SetShowNum(-1);

            callBack();
        }

        public void Stop()
        {
            SuperTween.Instance.Remove(tweenIndex);

            effect.SetShowNum(-1);

            text.text = string.Empty;

            text.text = str;

            callBack();
        }
    }

    private const float m_SingleDefualtTime = 0.12f;//单个字符出现的时间

    public void AddSequence(Text text, TextSequenceEffect _effect, string str, Action _callBack, Action _stepCallBack)
    {
        AddSequence(text, _effect, str, _callBack, m_SingleDefualtTime, _stepCallBack);
    }

    public void AddSequence(Text text, TextSequenceEffect _effect, string str, Action _callBack, float _oneLetterTime, Action _stepCallBack)
    {
        Action dele = delegate ()
        {
            m_sequenceDic.Remove(text);

            if (_callBack != null)
            {
                _callBack();
            }
        };

        SequenceHandler handler = new SequenceHandler(text, _effect, str, dele, _oneLetterTime, _stepCallBack);

        m_sequenceDic.Add(text, handler);
    }

    public void RemoveSequence(Text text)
    {
        SequenceHandler handler;

        if (m_sequenceDic.TryGetValue(text, out handler))
        {
            handler.Stop();
        }
    }
}
