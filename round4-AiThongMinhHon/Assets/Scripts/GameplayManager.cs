using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum QuestionState
{
    LOCKED,
    CORRECT,
    WRONG
}

public class GameplayManager : Singleton<GameplayManager>
{
    public TMP_Text txt_score;
    public int question_score_increment = 10;

    public Button btn_start_game;
    public Button btn_quit_game;

    public GameObject panel_menu;
    private int _score;
    private List<QuestionState> _states = new List<QuestionState>();

    private int _unlockedQuestion = 0;
    private bool _guessedKeyword = false;

    private void Start()
    {
        btn_start_game.onClick.AddListener(() =>
        {
            panel_menu.SetActive(false);
            StartMatch();
        });

        btn_quit_game.onClick.AddListener(() =>
        {
            if (panel_menu.activeInHierarchy)
            {
                Application.Quit();
            }
            else
            {
                // Stop the match
                StopMatch();
            }
        });
    }

    public void StopMatch()
    {
        // QuestionManager.Instance.ToggleQuestionFrame(false);
        // ClueManager.Instance.ToggleClueFrame(false);
        // AnswerModule.Instance.ToggleAnswerFrame(false);
        // panel_menu.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartMatch()
    {
        Keyword keyword = JSONReader.Instance.GetRandomKeyword();
        List<Question> questions = JSONReader.Instance.GetQuestions();

        foreach (Question question in questions)
        {
            print(question.question);
        }

        ClueManager.Instance.GenerateClue(keyword);
        QuestionManager.Instance.GenerateQuestion(questions.ToArray(), keyword.clues);

        QuestionManager.Instance.ToggleQuestionFrame(false);
        ClueManager.Instance.ToggleClueFrame(true);
        AnswerModule.Instance.ToggleAnswerFrame(false);

        if (QuestionManager.Instance.OnAnsweredCorrectly != null)
            QuestionManager.Instance.OnAnsweredCorrectly.RemoveAllListeners();

        if (QuestionManager.Instance.OnAnsweredWrongly != null)
            QuestionManager.Instance.OnAnsweredWrongly.RemoveAllListeners();

        if (ClueManager.Instance.OnAnsweredKeywordCorrectly != null)
            ClueManager.Instance.OnAnsweredKeywordCorrectly.RemoveAllListeners();

        if (ClueManager.Instance.OnAnsweredKeywordWrongly != null)
            ClueManager.Instance.OnAnsweredKeywordWrongly.RemoveAllListeners();

        _score = 0;
        UpdateScore();
        _states.Clear();
        for (int i = 0; i < Constants.MAX_CLUE; ++i)
        {
            QuestionState state = QuestionState.LOCKED;
            _states.Add(state);
        }

        QuestionManager.Instance.OnAnsweredCorrectly.AddListener((index) =>
        {
            _states[index] = QuestionState.CORRECT;
            _score += question_score_increment;
            _unlockedQuestion += 1;
            ClueManager.Instance.SetClue(index, true);
            UpdateScore();
        });

        QuestionManager.Instance.OnAnsweredWrongly.AddListener((index) =>
        {
            _states[index] = QuestionState.WRONG;
            _unlockedQuestion += 1;
            UpdateScore();
        });

        ClueManager.Instance.OnAnsweredKeywordCorrectly.AddListener(() =>
        {
            int addition = 0;
            foreach (QuestionState state in _states)
            {
                if (state == QuestionState.LOCKED || state == QuestionState.WRONG) addition += 10;
            }
            _score += addition;
            _guessedKeyword = true;
            UpdateScore();
        });

        ClueManager.Instance.OnAnsweredKeywordWrongly.AddListener(() =>
        {
            int addition = 0;
            foreach (QuestionState state in _states)
            {
                if (state == QuestionState.LOCKED || state == QuestionState.WRONG) addition += -5;
            }
            _score += addition;
            _guessedKeyword = true;
            UpdateScore();
        });
    }

    public void UpdateScore()
    {
        txt_score.text = _score.ToString();
        if (_unlockedQuestion == Constants.MAX_CLUE && _guessedKeyword)
        {
            // end game
            StopMatch();
        }
    }
}
