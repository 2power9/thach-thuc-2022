using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class OnSubmitEvent : UnityEvent<string>
{

}

public class AnswerModule : Singleton<AnswerModule>
{
    public GameObject window_answer;
    public TMP_InputField input_answer;
    public Button img_bg;

    public OnSubmitEvent OnSubmitAnswer = new OnSubmitEvent();

    private void Start()
    {
        input_answer.text = "";
        img_bg.onClick.AddListener(() =>
        {
            window_answer.SetActive(false);
        });
    }

    public void ToggleAnswerFrame(bool state)
    {
        window_answer.SetActive(state);
    }

    public void ShowInputField()
    {
        ToggleAnswerFrame(true);
        input_answer.onSubmit.AddListener((txt) =>
        {
            OnSubmitAnswer?.Invoke(txt);
            input_answer.text = "";
            ToggleAnswerFrame(false);
        });
    }

    
}
