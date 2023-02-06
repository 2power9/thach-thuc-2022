using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ClueManager : Singleton<ClueManager>
{
    public GameObject panel_clue;
    public GameObject img_clue;
    public GameObject window_clue;

    public Button btn_keyword;

    private List<Image> _clues = new List<Image>();
    public UnityEvent OnAnsweredKeywordCorrectly = new UnityEvent();
    public UnityEvent OnAnsweredKeywordWrongly = new UnityEvent();

    private string _keywordAnswer = "";

    private void Start()
    {
        btn_keyword.onClick.AddListener(() =>
        {
            AnswerModule.Instance.OnSubmitAnswer.RemoveAllListeners();
            AnswerModule.Instance.OnSubmitAnswer.AddListener((txt) =>
            {
                if (txt.ToUpper() == _keywordAnswer.ToUpper())
                {
                    // answered correctly
                    OnAnsweredKeywordCorrectly?.Invoke();
                }
                else
                {
                    // answered wrong
                    OnAnsweredKeywordWrongly?.Invoke();
                }
                QuestionManager.Instance.ToggleQuestionFrame(false);
                ToggleClueFrame(true);
            });
            AnswerModule.Instance.ShowInputField();
        });
    }

    public void GenerateClue(Keyword keyword)
    {
        foreach (Transform child in panel_clue.transform)
        {
            Destroy(child.gameObject);
        }
        LoadImage(keyword.imageDir);
        LoadClues(keyword.clues);
        _keywordAnswer = keyword.keyword;
    }

    private void LoadImage(string imageDir)
    {
        Sprite sprite = Resources.Load<Sprite>("Image/" + imageDir);
        panel_clue.GetComponent<Image>().sprite = sprite;
    }

    private void LoadClues(string[] clues)
    {
        for (int i = 0; i < clues.Length; ++i)
        {
            Image img = GameObject.Instantiate(img_clue, panel_clue.transform).GetComponent<Image>();
            img.GetComponentInChildren<TMP_Text>().text = (i+1).ToString();
            _clues.Add(img);
        }
    }

    public void SetClue(int index, bool answeredCorrectly)
    {
        if (answeredCorrectly)
        {
            _clues[index].gameObject.SetActive(false);
        } else
        {
            
        }
    }

    public void ToggleClueFrame(bool state)
    {
        window_clue.SetActive(state);
    }
}
