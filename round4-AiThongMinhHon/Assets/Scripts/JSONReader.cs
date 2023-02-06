using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : Singleton<JSONReader>
{
    public TextAsset keywordJson;
    public TextAsset questionJson;

    private List<Keyword> listOfKeywords = new List<Keyword>();
    private List<Question> listOfQuestions = new List<Question>();

    private void Start()
    {
        Keywords keywords = JsonUtility.FromJson<Keywords>(keywordJson.text);
        foreach (Keyword keyword in keywords.keywords)
        {
            listOfKeywords.Add(keyword);
        }

        Questions questions = JsonUtility.FromJson<Questions>(questionJson.text);
        foreach (Question question in questions.questions)
        {
            listOfQuestions.Add(question);
        }
    }

    public Keyword GetRandomKeyword()
    {
        int index = Random.Range(0, listOfKeywords.Count);
        Keyword res = listOfKeywords[index];
        listOfKeywords.RemoveAt(index);
        return res;
    }

    public Keyword GetKeyword(string key)
    {
        foreach (Keyword keyword in listOfKeywords)
        {
            if (keyword.keyword == key)
            {
                return keyword;
            }
        }
        return null;
    }

    public List<Question> GetQuestions()
    {
        List<Question> res = new List<Question>();
        for (int i = 0; i < Constants.MAX_CLUE; ++i)
        {
            int index = Random.Range(0, listOfQuestions.Count);
            Question q = listOfQuestions[index];
            listOfQuestions.RemoveAt(index);
            res.Add(q);
        }
        
        return res;
    }
}
