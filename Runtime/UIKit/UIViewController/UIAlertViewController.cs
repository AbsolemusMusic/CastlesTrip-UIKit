using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CT.UIKit;

public class UIAlertViewController : UIViewController, IAlertButtonView
{
    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private RectTransform btnsParentRT;

    public delegate void AlertCompletion(int buttonID);
    private AlertCompletion completion;

    private List<AlertButtonView> btns = new List<AlertButtonView>();

    public void Present(string descr, AlertCompletion completion, params AnswerAlertData[] answers)
    {
        this.completion = completion;
        descriptionText.text = descr;
        btns.Clear();
        InitAnswers(answers);
        base.Present();
    }

    public override void Start()
    {
        base.Start();

        foreach (AlertButtonView btn in btns)
        {
            btn.transform.Reset();
        }
    }

    private void InitAnswers(params AnswerAlertData[] answers)
    {
        for (int i = 0; i < answers.Length; i++)
        {
            AlertButtonView btn = Nib.LoadViewNib<AlertButtonView>();
            btn.transform.SetParent(btnsParentRT);
            btn.transform.Reset();
            btn.Init(this, answers[i]);
            btns.Add(btn);
        }
    }

    public void OnMainTapped(AlertButtonView view)
    {
        for (int i = 0; i < btns.Count; i++)
        {
            if (!btns[i].Equals(view)) continue;

            completion?.Invoke(i);
            break;
        }

        DidClosed();
    }
}

public struct AnswerAlertData
{
    public string answerText;
    public Color textColor;

    public AnswerAlertData(string text, Color color)
    {
        answerText = text;
        textColor = color;
    }
}