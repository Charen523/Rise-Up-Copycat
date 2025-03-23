using System.Collections;
using TMPro;
using UnityEngine;

public class UIScore : UIBase
{
    [SerializeField] private int score;
    [SerializeField] TextMeshProUGUI scoreTxt;

    Coroutine scoreCoroutine;

    public override void Opened(object[] param)
    {
        score = 0;
        scoreTxt.text = score.ToString();
        scoreCoroutine = StartCoroutine(ShowScore());
    }

    public override void Closed(object[] param)
    {
        StopCoroutine(scoreCoroutine);
    }

    public int GetScore()
    {
        return score;
    }

    private IEnumerator ShowScore()
    {
        WaitForSeconds elapse = new(1f);
        while (true)
        {
            score++;
            scoreTxt.text = score.ToString();
            yield return elapse;
        }
    }
}