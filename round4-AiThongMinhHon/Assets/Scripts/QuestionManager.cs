using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class OnAnswerEvent : UnityEvent<int>
{

}

public class QuestionManager : Singleton<QuestionManager>
{
    public GameObject panel_question;
    public GameObject btn_question;

    public GameObject window_question;
    public TMP_Text txt_question;
    public TMP_Text txt_question_number;
    public TMP_Text txt_timer;
    public TMP_Text txt_length;
    public Button btn_answer;

    public float time = 15f;
    private float curTime = 0f;
    private bool isQuestioning = false;
    int currentIndex = 0;

    private List<Button> _clues = new List<Button>();
    private List<string> _clueString = new List<string>();

    public OnAnswerEvent OnAnsweredCorrectly = new OnAnswerEvent();
    public OnAnswerEvent OnAnsweredWrongly = new OnAnswerEvent();

    private void Update()
    {
        if (isQuestioning)
        {
            curTime = curTime - Time.deltaTime;
            UpdateTime(curTime);

            if (curTime <= 0)
            {
                OnAnsweredWrongly?.Invoke(currentIndex);
                _clues[currentIndex].interactable = false;
                ToggleQuestionFrame(false);
                ClueManager.Instance.ToggleClueFrame(true);
                AnswerModule.Instance.ToggleAnswerFrame(false);
                isQuestioning = false;
            }
            
        }
    }

    public void GenerateQuestion(Question[] questions, string[] clues)
    {
        _clueString = new List<string>(clues);
        foreach(GameObject child in panel_question.transform)
        {
            Destroy(child);
        }
        print(questions[11].question);
        for (int i = 0; i < questions.Length; ++i)
        {
            Button b = Instantiate(btn_question, panel_question.transform).GetComponent<Button>();
            b.GetComponentInChildren<TMP_Text>().text = (i+1).ToString();
            int index = i;
            b.onClick.AddListener(() => ShowQuestion(index, questions[index]));
            _clues.Add(b);
        }
    }

    public void ShowQuestion(int index, Question question)
    {
        currentIndex = index;
        ToggleQuestionFrame(true);
        ClueManager.Instance.ToggleClueFrame(false);
        txt_question.text = question.question;

        curTime = time;
        UpdateTime(curTime);

        txt_length.text = "(" + question.answer.Length.ToString() + " ký tự)\nNhấp để trả lời";
        txt_question_number.text = "Câu hỏi số " + (index+1).ToString();
        isQuestioning = true;

        btn_answer.onClick.AddListener(() =>
        {
            AnswerModule.Instance.OnSubmitAnswer.RemoveAllListeners();
            AnswerModule.Instance.OnSubmitAnswer.AddListener((txt) =>
            {
                if (txt.ToUpper() == question.answer.ToUpper())
                {
                    // answered correctly
                    OnAnsweredCorrectly?.Invoke(index);
                    _clues[index].GetComponentInChildren<TMP_Text>().text = _clueString[index];
                    _clues[index].interactable = false;
                } 
                else
                {
                    // answered wrong
                    OnAnsweredWrongly?.Invoke(index);
                    _clues[index].interactable = false;
                }
                isQuestioning = false;
                ToggleQuestionFrame(false);
                ClueManager.Instance.ToggleClueFrame(true);
            });
            AnswerModule.Instance.ShowInputField();
        });
    }

    private void UpdateTime(float seconds)
    {
        // print(seconds.ToString());
        // int min = Mathf.FloorToInt(time / 60F);
        // int sec = Mathf.FloorToInt(time - min * 60);
        txt_timer.text = Mathf.FloorToInt(seconds).ToString();
    }

    public void ToggleQuestionFrame(bool state)
    {
        window_question.SetActive(state);
    }
}
